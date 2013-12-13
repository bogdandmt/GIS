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
        
        public static ShapeFileFeatureLayer CreateWorldLayer()
        {
            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(dataPath + @"World\cntry02.shp");
            return worldLayer;
        }

        public static ShapeFileFeatureLayer CreateCapitalPointLayer()
        {
            ShapeFileFeatureLayer capitalPointLayer = new ShapeFileFeatureLayer(dataPath + @"World\capital.shp");
            return capitalPointLayer;
        }

        public static ShapeFileFeatureLayer CreateCapitalNameLayer()
        {
            ShapeFileFeatureLayer capitalNameLayer = new ShapeFileFeatureLayer(dataPath + @"World\capital.shp");
            return capitalNameLayer;
        }
    }
}
