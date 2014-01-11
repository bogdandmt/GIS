using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmsPlugin
{
    public class Util
    {
        public static String dataPath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName + @"\SampleData\";
        public static String citiesShpFile = dataPath + @"Ukraine\places.shp";
        public static String roadsShpFile = dataPath + @"Ukraine\roads.shp";
        public static String poisShpFile = dataPath + @"Ukraine\pois.shp";
    }
}
