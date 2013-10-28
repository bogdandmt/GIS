using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ThinkGeo.MapSuite.Core;

namespace ThinkGeo.MapSuite.WmsServerEdition
{

    /// <summary>
    /// Represent the Default page.
    /// </summary>
    public partial class Admin : System.Web.UI.Page
    {
        private readonly string pluginsCacheKey = "Plugins";
        private string pluginsPath = ConfigurationManager.AppSettings["WmsLayerPluginsPath"];

        [ImportMany(typeof(WmsLayerPlugin), AllowRecomposition = true)]
        private Collection<WmsLayerPlugin> wmsPlugins { get; set; }

        /// <summary>Initialize an instance of the Admin class.</summary>
        /// <overloads>Initialize an instance of the Admin class.</overloads>
        public Admin()
        { }

        /// <summary>
        /// This method is called when the load event of Admin page is triggered. 
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RefreshPlugins();
            }
        }

        private void RefreshPlugins()
        {
            if (pluginsPath.StartsWith("\\", StringComparison.OrdinalIgnoreCase))
            {
                pluginsPath = AppDomain.CurrentDomain.BaseDirectory + pluginsPath;
            }
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }
            string[] pluginNames = Directory.GetFiles(Path.GetDirectoryName(pluginsPath).Replace(@"file:\", ""), "*Plugin.dll", SearchOption.AllDirectories);

            DataTable dataSource = new DataTable();
            dataSource.Locale = CultureInfo.InvariantCulture;
            dataSource.Columns.Add("Name");
            dataSource.Columns.Add("FullName");
            foreach (string pluginName in pluginNames)
            {
                FileInfo fileInfo = new FileInfo(pluginName);
                DataRow row = dataSource.NewRow();
                row["Name"] = fileInfo.Name;
                row["FullName"] = fileInfo.FullName;
                dataSource.Rows.Add(row);
            }

            PluginsRepeater.DataSource = dataSource;
            PluginsRepeater.DataBind();
        }

        /// <summary>
        /// Fires after an item has been databound. 
        /// </summary>
        /// <param name="sender">Represent the event trigger control.</param>
        /// <param name="e">The event argument for the Repeater control item data bound event.</param>
        protected void PluginsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlImage image = (HtmlImage)e.Item.FindControl("PreviewImage");
                string fullName = ((HtmlGenericControl)e.Item.FindControl("FullNameDiv")).InnerText.Trim();
                Collection<WmsLayerPlugin> wmsPlugins = LoadPluginsFromOneAssembly(fullName);

                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.InvariantCulture;
                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("FullName");
                foreach (WmsLayerPlugin plugin in wmsPlugins)
                {
                    DataRow row = dataTable.NewRow();
                    row["Name"] = plugin.GetName();
                    row["FullName"] = fullName;
                    dataTable.Rows.Add(row);
                }

                GridView gridView = (GridView)e.Item.FindControl("PluginGridView");
                gridView.DataSource = dataTable;
                gridView.DataBind();

                if (wmsPlugins.Count > 0)
                {
                    image.Src = GetSampleAddress(wmsPlugins[0], "EPSG:4326");
                    image.Visible = true;
                }
            }
        }

        /// <summary>
        /// Fires after a row has been databound. 
        /// </summary>
        /// <param name="sender">Represent the event trigger control.</param>
        /// <param name="e">The event argument for the GridView control row data bound event.</param>
        protected void PluginGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Hide full name field.
            e.Row.Cells[3].Style.Add("display", "none");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string name = e.Row.Cells[0].Text;
                string fullName = e.Row.Cells[3].Text;
                WmsLayerPlugin plugin = LoadPlugin(fullName, name);

                foreach (WmsLayerStyle style in plugin.GetStyles())
                {
                    e.Row.Cells[1].Text += style.Name + "/";
                }
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.TrimEnd('/');

                foreach (string projectionName in plugin.GetProjections())
                {
                    e.Row.Cells[2].Text += projectionName + "/";
                }
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.TrimEnd('/');

                string crs = plugin.GetProjections()[0];
                string units = string.Empty;
                switch (plugin.GetGeographyUnit(crs))
                {
                    case GeographyUnit.DecimalDegree:
                        units = "degrees";
                        break;
                    case GeographyUnit.Meter:
                        units = "m";
                        break;
                    case GeographyUnit.Feet:
                        units = "ft";
                        break;
                    default:
                        break;
                }

                string imageID = e.Row.Parent.Parent.Parent.FindControl("PreviewImage").ClientID;
                string previewUrl = "MapView.htm?serverUrl=" + String.Format(CultureInfo.InvariantCulture, "WmsHandler.axd&layer={0}&style=DEFAULT&projection={1}", name, crs);
                string imageUrl = GetSampleAddress(plugin, crs);

                HtmlAnchor link = (HtmlAnchor)e.Row.Cells[4].FindControl("PreviewLink");
                link.HRef = "javascript:OpenViewLayer(this, '" + previewUrl + "&units=" + units + "&bbox=" + ConvertBoundingBoxToString(plugin.GetBoundingBox(crs)) + "');";
                link.Attributes.Add("onmouseover", String.Format(CultureInfo.InvariantCulture, "HoverToPreview('{0}', '{1}')", imageID, imageUrl));
            }
        }

        private static string GetSampleAddress(WmsLayerPlugin plugin, string crs)
        {
            RectangleShape bbox = plugin.GetBoundingBox(crs);
            StringBuilder parameters = new StringBuilder();
            parameters.Append("WmsHandler.axd?");
            parameters.AppendFormat(CultureInfo.InvariantCulture, "LAYERS={0}&", plugin.GetName());
            parameters.AppendFormat(CultureInfo.InvariantCulture, "STYLES=DEFAULT&");
            parameters.Append("SERVICE=WMS&");
            parameters.Append("VERSION=1.1.1&");
            parameters.Append("REQUEST=GetMap&");
            parameters.Append("FORMAT=image/png&");
            parameters.AppendFormat(CultureInfo.InvariantCulture, "SRS={0}&", crs);
            parameters.Append("WIDTH=128&");
            parameters.Append("HEIGHT=128&");
            parameters.AppendFormat(CultureInfo.InvariantCulture, "BBOX={0}", ConvertBoundingBoxToString(bbox));
            return parameters.ToString();
        }

        private static string ConvertBoundingBoxToString(RectangleShape boundingBox)
        {
            return String.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3}", boundingBox.LowerLeftPoint.X, boundingBox.LowerLeftPoint.Y, boundingBox.UpperRightPoint.X, boundingBox.UpperRightPoint.Y);
        }

        private Collection<WmsLayerPlugin> LoadPluginsFromOneAssembly(string assemblyFilePathName)
        {
            FileInfo fileInfo = new FileInfo(assemblyFilePathName);
            if (HttpContext.Current.Application[pluginsCacheKey + fileInfo.Name] != null)
            {
                return (Collection<WmsLayerPlugin>)HttpContext.Current.Application[pluginsCacheKey + fileInfo.Name];
            }
            else
            {
                FileStream stream = new FileStream(assemblyFilePathName, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                Assembly currentAssembly = Assembly.Load(buffer);
                stream.Dispose();

                AssemblyCatalog catalog = new AssemblyCatalog(currentAssembly);
                CompositionContainer container = new CompositionContainer(catalog);
                container.ComposeParts(this);

                Collection<WmsLayerPlugin> tempWmsPlugins = new Collection<WmsLayerPlugin>();
                foreach (WmsLayerPlugin item in wmsPlugins)
                {
                    tempWmsPlugins.Add(item);
                }

                HttpContext.Current.Application[pluginsCacheKey + fileInfo.Name] = tempWmsPlugins;
                return wmsPlugins;
            }
        }

        private WmsLayerPlugin LoadPlugin(string assemblyFilePathName, string pluginName)
        {
            Collection<WmsLayerPlugin> tempWmsPlugins = LoadPluginsFromOneAssembly(assemblyFilePathName);
            WmsLayerPlugin resultPlugin = null;
            foreach (WmsLayerPlugin plugin in tempWmsPlugins)
            {
                if (plugin.GetName() == pluginName)
                {
                    resultPlugin = plugin;
                }
            }
            return resultPlugin;
        }
    }
}
