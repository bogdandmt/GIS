using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Web;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WmsServerEdition;

namespace WmsPlugin
{
    public class ShowUserSpecificMapsWmsLayerPlugin : WmsLayerPlugin
    {
        // This method is only called once per style and crs.  In it you should create your
        // layers and add them to the MapConfiguration.  If you want to use tile caching you
        // can also specif that in the MapConfiguration under the TileCache property.
        // If you have setup multiple styles or projections this method will get called for
        // each unique combination
        protected override MapConfiguration GetMapConfigurationCore(string style, string crs)
        {
            // Get the paths to the sample data
            string path = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName;
            string worldLayerFilePath = path + Path.DirectorySeparatorChar + "SampleData" + Path.DirectorySeparatorChar + "Countries02.shp";
            string stateFilePath = path + Path.DirectorySeparatorChar + "SampleData" + Path.DirectorySeparatorChar + "STATES.SHP";
            string citiesFilePath = path + Path.DirectorySeparatorChar + "SampleData" + Path.DirectorySeparatorChar + "cities_a.shp";

            // Setup the various shape file layers
            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(worldLayerFilePath);
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer statesLayer = new ShapeFileFeatureLayer(stateFilePath);
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.Country1("STATE_NAME");
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.FromArgb(255, 243, 239, 228), GeoColor.FromArgb(255, 218, 193, 163), 1);
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer citiesLayer = new ShapeFileFeatureLayer(citiesFilePath);
            citiesLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.City3;
            citiesLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("AREANAME", new GeoFont("Verdana", 9), new GeoSolidBrush(GeoColor.StandardColors.Black));
            citiesLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle.HaloPen = new GeoPen(GeoColor.StandardColors.White, 2);
            citiesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            citiesLayer.DrawingMarginPercentage = 50;

            // Create a MapConfiguration and add the layer to it
            MapConfiguration mapConfiguration = new MapConfiguration();
            mapConfiguration.Layers.Add("WorldLayer", worldLayer);
            mapConfiguration.Layers.Add("StatesLayer", statesLayer);
            mapConfiguration.Layers.Add("CitiesLayer", citiesLayer);

            return mapConfiguration;
        }

        // This method gets called on every amp request sent to the server.  In the parameters we pass the
        // map request which includes the bounding box, image size etc.  We also pass you the map configuration
        // which includes all the static layers.  In this method you can make any data changes or do anything
        // dynamic you want
        protected override Bitmap GetMapCore(GetMapRequest getMapRequest, MapConfiguration mapConfiguration, HttpContext context)
        {
            // Read the UserName from the query string.  The UserName was passed to us by the client            
            NameValueCollection queryString = context.Request.QueryString;
            string userName = queryString["UserName"];

            // Create a new MapConfiguration that will be used just this one time            
            MapConfiguration customMapConfiguration = new MapConfiguration();

            // Based on the user name supplied by the client we will load only certain layers
            switch (userName)
            {
                case "User1":
                    customMapConfiguration.Layers.Add(mapConfiguration.Layers["WorldLayer"]);
                    customMapConfiguration.Layers.Add(mapConfiguration.Layers["StatesLayer"]);
                    customMapConfiguration.Layers.Add(mapConfiguration.Layers["CitiesLayer"]);
                    break;
                case "User2":
                    customMapConfiguration.Layers.Add(mapConfiguration.Layers["WorldLayer"]);
                    customMapConfiguration.Layers.Add(mapConfiguration.Layers["StatesLayer"]);
                    break;
                case "User3":
                    customMapConfiguration.Layers.Add(mapConfiguration.Layers["WorldLayer"]);
                    break;
                default:
                    break;
            }

            //  Call the base to render the image based on the custom MapConfiguration we created
            return base.GetMapCore(getMapRequest, customMapConfiguration, context);
        }

        // In this method you need to return the name of the Layer that WMS will expose.
        // You will use this name on the client to specify the layer you want to consume
        protected override string GetNameCore()
        {
            return "Show User Specific Maps";
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
