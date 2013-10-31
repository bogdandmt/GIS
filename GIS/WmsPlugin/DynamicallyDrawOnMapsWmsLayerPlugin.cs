using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WmsServerEdition;

namespace WmsPlugin
{
    public class DynamicallyDrawOnMapsWmsLayerPlugin : WmsLayerPlugin
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
            mapConfiguration.Layers.Add(worldLayer);

            return mapConfiguration;
        }

        // This method gets called on every amp request sent to the server.  In the parameters we pass the
        // map request which includes the bounding box, image size etc.  We also pass you the map configuration
        // which includes all the static layers.  In this method you can make any data changes or do anything
        // dynamic you want
        protected override Bitmap GetMapCore(GetMapRequest getMapRequest, MapConfiguration mapConfiguration, System.Web.HttpContext context)
        {
            // Call the base and render the map image so we can draw on top of it below
            Bitmap bitmap = base.GetMapCore(getMapRequest, mapConfiguration, context);

            // Create a canvas so we can draw on the image
            GdiPlusGeoCanvas canvas = new GdiPlusGeoCanvas();
            canvas.BeginDrawing(bitmap, getMapRequest.BoundingBox, this.GetGeographyUnit(getMapRequest.Crs));

            // Draw the text "Watermark" on the image and end drawing
            PointShape centerPoint = getMapRequest.BoundingBox.GetCenterPoint();
            canvas.DrawTextWithWorldCoordinate("Watermark", new GeoFont("Arial", 11, DrawingFontStyles.Bold), new GeoSolidBrush(new GeoColor(100, GeoColor.StandardColors.Red)), centerPoint.X, centerPoint.Y, DrawingLevel.LevelOne);
            canvas.EndDrawing();

            // Return the bitmap we have drawn the watermark on
            return bitmap;
        }

        // In this method you need to return the name of the Layer that WMS will expose.
        // You will use this name on the client to specify the layer you want to consume
        protected override string GetNameCore()
        {
            return "Dynamically Draw On Maps";
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
