using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WmsServerEdition;

namespace WmsPlugin
{
    public class CountriesPlugin : WmsLayerPlugin
    {
        protected override RectangleShape GetBoundingBoxCore(string crs)
        {
            return new RectangleShape(-126.826171875, 57.104766845702, -70.83984375, 18.960235595702);
        }

        protected override MapConfiguration GetMapConfigurationCore(string style, string crs)
        {
            string worldLayerFilePath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName + Path.DirectorySeparatorChar + "SampleData" + Path.DirectorySeparatorChar + "Countries02.shp";

            BackgroundLayer backgroundLayer = new BackgroundLayer(new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean));

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(worldLayerFilePath);
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            MapConfiguration mapConfiguration = new MapConfiguration();
            mapConfiguration.Layers.Add("BackgroundLayer", backgroundLayer);
            mapConfiguration.Layers.Add("WorldLayer", worldLayer);
            return mapConfiguration;
        }

        protected override string GetNameCore()
        {
            return "Display A Simple Map";
        }

        protected override System.Collections.ObjectModel.Collection<string> GetProjectionsCore()
        {
            return new System.Collections.ObjectModel.Collection<string> { "EPSG:4326" };
        }
    }
}
