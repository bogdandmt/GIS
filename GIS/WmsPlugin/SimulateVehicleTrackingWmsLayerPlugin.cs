using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Web;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WmsServerEdition;

namespace WmsPlugin
{
    public class SimulateVehicleTrackingWmsLayerPlugin : WmsLayerPlugin
    {
        // The static route data for the vehicles
        private static double[,] vehicle1Positions = { { -95.2555829286728, 38.9606397151787 }, { -95.2555829286728, 38.9600389003593 }, { -95.2555721998367, 38.9594166278679 }, { -95.255089402214, 38.9590625762779 }, { -95.2549928426895, 38.95908403395 }, { -95.2539199590835, 38.9592127799827 }, { -95.2533406019363, 38.9593629836876 }, { -95.253286957756, 38.9595131873924 }, { -95.2540379762802, 38.9598457813102 }, { -95.2544671297226, 38.9602427482444 }, { -95.2546817064438, 38.9606719016869 }, { -95.2551215887222, 38.9606719016869 }, { -95.2555292844925, 38.9606504440147 } };
        private static double[,] vehicle2Positions = { { -95.2534264326248, 38.963225364669 }, { -95.2534908056411, 38.9624743461448 }, { -95.2537053823623, 38.962152481063 }, { -95.2538126707229, 38.9619379043418 }, { -95.253834128395, 38.96161603926 }, { -95.253297686592, 38.9615945815879 }, { -95.25256812574, 38.9615945815879 }, { -95.2523750066909, 38.9620451927024 }, { -95.2523320913467, 38.9626674651939 } };

        // The vehicles current path in the route
        private static int vehicle1PositionIndex = 0;
        private static int vehicle2PositionIndex = 0;

        // This method is only called once per style and crs.  In it you should create your
        // layers and add them to the MapConfiguration.  If you want to use tile caching you
        // can also specif that in the MapConfiguration under the TileCache property.
        // If you have setup multiple styles or projections this method will get called for
        // each unique combination
        protected override MapConfiguration GetMapConfigurationCore(string style, string crs)
        {
            // Get to the sample data
            string path = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName;
            string vehicle1Image = path + Path.DirectorySeparatorChar + "SampleData" + Path.DirectorySeparatorChar + "vehicle_van_1.png";
            string vehicle2Image = path + Path.DirectorySeparatorChar + "SampleData" + Path.DirectorySeparatorChar + "vehicle_van_2.png";

            // Create an in-memeory map shape layer to hold two vehicles
            MapShapeLayer vehicleLayer = new MapShapeLayer();

            // Setup vehicle 1
            MapShape vehicle1MapShape = new MapShape(new Feature(-95.2555829286728, 38.9606397151787));
            vehicle1MapShape.ZoomLevels.ZoomLevel01.DefaultPointStyle = new PointStyle(new GeoImage(vehicle1Image));
            vehicle1MapShape.ZoomLevels.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            vehicleLayer.MapShapes.Add("Vehicle1", vehicle1MapShape);

            //Setup vehicle 2
            MapShape vehicle2MapShape = new MapShape(new Feature(-95.2534264326248, 38.963225364669));
            vehicle2MapShape.ZoomLevels.ZoomLevel01.DefaultPointStyle = new PointStyle(new GeoImage(vehicle2Image));
            vehicle2MapShape.ZoomLevels.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            vehicleLayer.MapShapes.Add("Vehicle2", vehicle2MapShape);

            // Create a MapConfiguration and add the layer to it
            MapConfiguration mapConfiguration = new MapConfiguration();
            mapConfiguration.Layers.Add("VehicleLayer", vehicleLayer);

            return mapConfiguration;
        }

        // This method gets called on every amp request sent to the server.  In the parameters we pass the
        // map request which includes the bounding box, image size etc.  We also pass you the map configuration
        // which includes all the static layers.  In this method you can make any data changes or do anything
        // dynamic you want
        protected override Bitmap GetMapCore(GetMapRequest getMapRequest, MapConfiguration mapConfiguration, HttpContext context)
        {
            // Get the vehicle layer from the MapConfiguration
            MapShapeLayer vehicleLayer = mapConfiguration.Layers["VehicleLayer"] as MapShapeLayer;

            // Set the location of vehicles 1 & 2 to their new locations
            vehicleLayer.MapShapes["Vehicle1"].Feature = new Feature(vehicle1Positions[vehicle1PositionIndex, 0], vehicle1Positions[vehicle1PositionIndex, 1]);
            vehicleLayer.MapShapes["Vehicle2"].Feature = new Feature(vehicle2Positions[vehicle2PositionIndex, 0], vehicle2Positions[vehicle2PositionIndex, 1]);

            // Reset the locations when we run out of data
            if (++vehicle1PositionIndex >= vehicle1Positions.GetLength(0))
                vehicle1PositionIndex = 0;
            if (++vehicle2PositionIndex >= vehicle2Positions.GetLength(0))
                vehicle2PositionIndex = 0;

            // Create a bitmap we will draw the vehicles on
            Bitmap bitmap = new Bitmap(getMapRequest.Width, getMapRequest.Height);

            // Use our canvas to draw the vehicles on the bitmap from the vehicleLayer
            GdiPlusGeoCanvas canvas = new GdiPlusGeoCanvas();
            canvas.BeginDrawing(bitmap, getMapRequest.BoundingBox, GetGeographyUnit(getMapRequest.Crs));
            vehicleLayer.Draw(canvas, new Collection<SimpleCandidate>());
            canvas.EndDrawing();

            return bitmap;
        }

        // In this method you need to return the name of the Layer that WMS will expose.
        // You will use this name on the client to specify the layer you want to consume
        protected override string GetNameCore()
        {
            return "Simulate Vehicle Tracking";
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
            return new RectangleShape(-95.2599710226211, 38.9665298461753, -95.2441567182693, 38.9552645683128);
        }
    }
}
