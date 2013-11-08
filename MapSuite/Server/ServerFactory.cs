using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkGeo.MapSuite.Core;

namespace Server
{
    public class ServerFactory
    {
        private static String dataPath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName + @"\Server\Data\";
        
        public static ShapeFileFeatureLayer CreateCitiesLayer()
        {
            String shpFile = @"Ukraine\places.shp";
            ShapeFileFeatureSource.BuildIndexFile(dataPath + shpFile, BuildIndexMode.DoNotRebuild);
            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(dataPath + shpFile);
            return worldLayer;
        }

        public static ShapeFileFeatureLayer CreateRoadsLayer()
        {
            ShapeFileFeatureSource.BuildIndexFile(dataPath + @"Ukraine\roads.shp", BuildIndexMode.DoNotRebuild);
            ShapeFileFeatureLayer capitalPointLayer = new ShapeFileFeatureLayer(dataPath + @"Ukraine\roads.shp");
            return capitalPointLayer;
        }

        public static ShapeFileFeatureLayer CreatePlacesLayer()
        {
            String shpFile = @"Ukraine\pois.shp";
            ShapeFileFeatureSource.BuildIndexFile(dataPath + shpFile, BuildIndexMode.DoNotRebuild);
            ShapeFileFeatureLayer capitalNameLayer = new ShapeFileFeatureLayer(dataPath + shpFile);
            return capitalNameLayer;
        }
    }
}
