using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            ShapeFileFeatureLayer worldLayer = ServerFactory.CreateWorldLayer();
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer capitalPointLayer = ServerFactory.CreateCapitalPointLayer();
            capitalPointLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.Capital3;
            capitalPointLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer capitalNameLayer = ServerFactory.CreateCapitalNameLayer();
            capitalNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.Capital3("CITY_NAME");
            capitalNameLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            capitalNameLayer.DrawingMarginPercentage = 50;


            MainMap.StaticOverlay.Layers.Add(worldLayer);
            MainMap.StaticOverlay.Layers.Add(capitalPointLayer);
            MainMap.StaticOverlay.Layers.Add(capitalNameLayer);

            MainMap.CurrentExtent = new RectangleShape(5, 78, 30, 26);

            MainMap.MapBackground.BackgroundBrush = new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean);

            ServerFactory c = new ServerFactory(); ;
        }
    }
}