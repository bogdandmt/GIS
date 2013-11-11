using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WebEdition;
using WmsPlugin;

namespace WebClient
{
    public partial class SQLQuery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            map.MapUnit = GeographyUnit.DecimalDegree;

            ShapeFileFeatureLayer cityNameLayer = new ShapeFileFeatureLayer(Util.citiesShpFile);
            cityNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("NAME", new GeoFont(), new GeoSolidBrush(GeoColor.StandardColors.DarkRed));
            cityNameLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            cityNameLayer.DrawingMarginPercentage = 50;
            cityNameLayer.Encoding = Encoding.UTF8;

            ShapeFileFeatureLayer roadsLayer = new ShapeFileFeatureLayer(Util.roadsShpFile);
            roadsLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.LocalRoad2;
            roadsLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer roadsNameLayer = new ShapeFileFeatureLayer(Util.roadsShpFile);
            roadsNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.LocalRoad1("NAME");
            roadsNameLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            roadsNameLayer.DrawingMarginPercentage = 50;
            roadsNameLayer.Encoding = Encoding.UTF8;

            ShapeFileFeatureLayer placesLayer = new ShapeFileFeatureLayer(Util.poisShpFile);
            placesLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.City6;
            placesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer palceNameLayer = new ShapeFileFeatureLayer(Util.poisShpFile);
            palceNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("NAME", new GeoFont(), new GeoSolidBrush(GeoColor.StandardColors.DarkBlue));
            palceNameLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            palceNameLayer.DrawingMarginPercentage = 50;
            palceNameLayer.Encoding = Encoding.UTF8;

            map.StaticOverlay.Layers.Add(roadsLayer);
            map.StaticOverlay.Layers.Add(roadsNameLayer);
            map.StaticOverlay.Layers.Add(cityNameLayer);
            map.StaticOverlay.Layers.Add(placesLayer);
            map.StaticOverlay.Layers.Add(palceNameLayer);

            map.CurrentExtent = new RectangleShape(-1.93, 53.68, 50, 46);
            map.CurrentExtent.ScaleTo(0.001);
        }
    }
}