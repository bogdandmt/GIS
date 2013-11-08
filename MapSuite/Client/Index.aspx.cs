using Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WebEdition;

namespace Client
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dataPath = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName + @"\Server\Data\";
            MainMap.MapUnit = GeographyUnit.DecimalDegree;

            ShapeFileFeatureLayer citiesLayer = ServerFactory.CreateCitiesLayer();
            citiesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Urban2;
            citiesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer cityNameLayer = ServerFactory.CreateCitiesLayer();
            //cityNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.City3("NAME");
            cityNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("NAME", new GeoFont(), new GeoSolidBrush(GeoColor.StandardColors.DarkRed));
            //cityNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.Capital3("NAME");
            cityNameLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            cityNameLayer.DrawingMarginPercentage = 50;
            cityNameLayer.Encoding = Encoding.UTF8;

            ShapeFileFeatureLayer roadsLayer = ServerFactory.CreateRoadsLayer();
            roadsLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.LocalRoad2;
            roadsLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer roadsNameLayer = ServerFactory.CreateRoadsLayer();
            roadsNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.LocalRoad1("NAME");
            roadsNameLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            roadsNameLayer.DrawingMarginPercentage = 50;
            roadsNameLayer.Encoding = Encoding.UTF8;

            ShapeFileFeatureLayer placesLayer = ServerFactory.CreatePlacesLayer();
            placesLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.City6;
            placesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            //capitalNameLayer.DrawingMarginPercentage = 50;

            ShapeFileFeatureLayer palceNameLayer = ServerFactory.CreatePlacesLayer();
            //palceNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.Utility2("NAME");
            palceNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("NAME", new GeoFont(), new GeoSolidBrush(GeoColor.StandardColors.DarkBlue));
            palceNameLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            palceNameLayer.DrawingMarginPercentage = 50;
            palceNameLayer.Encoding = Encoding.UTF8;


            MainMap.StaticOverlay.Layers.Add(roadsLayer);
            MainMap.StaticOverlay.Layers.Add(roadsNameLayer);
            //MainMap.StaticOverlay.Layers.Add(citiesLayer);
            MainMap.StaticOverlay.Layers.Add(cityNameLayer);
            MainMap.StaticOverlay.Layers.Add(placesLayer);
            MainMap.StaticOverlay.Layers.Add(palceNameLayer);

            MainMap.CurrentExtent = new RectangleShape(-1.93, 53.68, 50, 46);
            MainMap.CurrentExtent.ScaleTo(0.001);


            ServerFactory c = new ServerFactory();
        }
    }
}