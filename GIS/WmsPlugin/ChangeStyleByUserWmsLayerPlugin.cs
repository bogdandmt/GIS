using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WmsServerEdition;

namespace WmsPlugin
{
    public class ChangeStyleByUserWmsLayerPlugin : WmsLayerPlugin
    {
        // This method is only called once per style and crs.  In it you should create your
        // layers and add them to the MapConfiguration.  If you want to use tile caching you
        // can also specif that in the MapConfiguration under the TileCache property.
        // If you have setup multiple styles or projections this method will get called for
        // each unique combination
        protected override MapConfiguration GetMapConfigurationCore(string style, string crs)
        {
            // Get the directory to the sample data
            string worldLayerFilePath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName + Path.DirectorySeparatorChar + "SampleData" + Path.DirectorySeparatorChar + "Countries02.shp";

            // Create a layer from a shape file but do not give it a style
            // We will give it a style on the fly in GetMapCore
            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(worldLayerFilePath);
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            // Create a MapConfiguration and add the layer to it
            MapConfiguration mapConfiguration = new MapConfiguration();
            mapConfiguration.Layers.Add("WorldLayer", worldLayer);

            return mapConfiguration;
        }

        // This method gets called on every amp request sent to the server.  In the parameters we pass the
        // map request which includes the bounding box, image size etc.  We also pass you the map configuration
        // which includes all the static layers.  In this method you can make any data changes or do anything
        // dynamic you want
        protected override Bitmap GetMapCore(GetMapRequest getMapRequest, MapConfiguration mapConfiguration, System.Web.HttpContext context)
        {
            // Get the layer from the MapConfiguration so we can change its style
            ShapeFileFeatureLayer layer = mapConfiguration.Layers["WorldLayer"] as ShapeFileFeatureLayer;

            // Read the UserName from the query string.  The UserName was passed to us by the client            
            NameValueCollection queryString = context.Request.QueryString;
            string userName = queryString["UserName"];

            // Based on the user we are going to apply a different style on the fly
            switch (userName)
            {
                case "User1":
                    layer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
                    break;
                case "User2":
                    layer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
                    layer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Black;
                    break;
                case "User3":
                    layer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
                    layer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.StandardColors.Wheat;
                    break;
                default:
                    layer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.NoData1;
                    break;
            }

            //  Call the base to render the image based on the style we setup above
            return base.GetMapCore(getMapRequest, mapConfiguration, context);
        }

        // In this method you need to return the name of the Layer that WMS will expose.
        // You will use this name on the client to specify the layer you want to consume
        protected override string GetNameCore()
        {
            return "Change Style By User";
        }

        // In this method you need to return the projections that are supported by your data.
        // It is your responsability to project the data in the MapConfiguration for each projection
        // type you specify here.
        protected override Collection<string> GetProjectionsCore()
        {
            return new Collection<string> { "EPSG:4326" };
        }

        // In this method you need to return the bounding box of the layer.
        protected override RectangleShape GetBoundingBoxCore(string crs)
        {
            return new RectangleShape(-126.826171875, 57.104766845702, -70.83984375, 18.960235595702);
        }
    }
}
