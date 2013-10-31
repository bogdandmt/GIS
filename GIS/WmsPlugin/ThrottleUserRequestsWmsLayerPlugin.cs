using System;
using System.Collections.ObjectModel;
using System.IO;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WmsServerEdition;

namespace WmsPlugin
{
    public class ThrottleUserRequestsWmsLayerPlugin : WmsLayerPlugin
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

            // Create the world layer from a shapefile and add a style to it
            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(worldLayerFilePath);
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            // Create a MapConfiguration and add the layer to it
            MapConfiguration mapConfiguration = new MapConfiguration();
            mapConfiguration.Layers.Add("WorldLayer", worldLayer);

            return mapConfiguration;
        }

        // In this method you need to return the name of the Layer that WMS will expose.
        // You will use this name on the client to specify the layer you want to consume
        protected override string GetNameCore()
        {
            return "Throttle User Requests";
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
            return new RectangleShape(-180, 90, 180, -90);
        }
    }
}
