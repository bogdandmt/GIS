﻿using System;
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
        private ShapeFileFeatureLayer placesLayer;


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

            placesLayer = new ShapeFileFeatureLayer(Util.poisShpFile);
            placesLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.City6;
            placesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer placeNameLayer = new ShapeFileFeatureLayer(Util.poisShpFile);
            placeNameLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = new TextStyle("NAME", new GeoFont(), new GeoSolidBrush(GeoColor.StandardColors.DarkBlue));
            placeNameLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            placeNameLayer.DrawingMarginPercentage = 50;
            placeNameLayer.Encoding = Encoding.UTF8;

            map.StaticOverlay.Layers.Add(roadsLayer);
            map.StaticOverlay.Layers.Add(roadsNameLayer);
            map.StaticOverlay.Layers.Add(cityNameLayer);
            map.StaticOverlay.Layers.Add(placesLayer);
            map.StaticOverlay.Layers.Add(placeNameLayer);

            map.CurrentExtent = new RectangleShape(-1.93, 53.68, 50, 46);
            map.CurrentExtent.ScaleTo(0.001);
        }

        protected void EqButtonClick(object sender, EventArgs e)
        {

        }

        protected void NotEqButtonClick(object sender, EventArgs e)
        {

        }

        protected void GreaterButtonClick(object sender, EventArgs e)
        {

        }

        protected void GreaterEqButtonClick(object sender, EventArgs e)
        {

        }

        protected void LessButtonClick(object sender, EventArgs e)
        {

        }

        protected void LessEqButtonClick(object sender, EventArgs e)
        {

        }
    }
}