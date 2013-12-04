using System;
using System.Configuration;
using System.Collections.ObjectModel;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WebEdition;

namespace WebClient
{
    public partial class SelectFeature : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Map1.MapBackground.BackgroundBrush = new GeoSolidBrush(GeoColor.FromHtml("#E5E3DF"));
                Map1.CurrentExtent = new RectangleShape(-97.7583, 30.2714, -97.7444, 30.2632);
                Map1.MapUnit = GeographyUnit.DecimalDegree;

                Map1.MapTools.MouseCoordinate.Enabled = true;

                LayerOverlay streetOverlay = new LayerOverlay("StreetOverlay", false, TileType.SingleTile);
                streetOverlay.IsBaseOverlay = false;

                string shapePath = MapPath("App_Data\\Streets.shp");
                ShapeFileFeatureLayer streetLayer = new ShapeFileFeatureLayer(shapePath);
                streetLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.LocalRoad1;
                streetLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

                streetOverlay.Layers.Add("StreetLayer", streetLayer);
                Map1.CustomOverlays.Add(streetOverlay);

                //InMemoryFeature to show the selected feature (the feature clicked on).
                InMemoryFeatureLayer selectLayer = new InMemoryFeatureLayer();
                selectLayer.Open();
                selectLayer.Columns.Add(new FeatureSourceColumn("FENAME"));
                selectLayer.Close();
                selectLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle = LineStyles.CreateSimpleLineStyle(GeoColor.FromArgb(150, GeoColor.StandardColors.Red), 10, true);
                selectLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.LocalRoad1("FENAME");
                selectLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

                LayerOverlay selectOverlay = new LayerOverlay();
                selectOverlay.Layers.Add("SelectLayer", selectLayer);
                selectOverlay.IsBaseOverlay = false;

                Map1.CustomOverlays.Add(selectOverlay);

            }
        }

        protected void Map1_Click(object sender, MapClickedEventArgs e)
        {
            //Here we use a buffer of 15 in screen coordinate. This means that regardless of the zoom level, we will always find the nearest feature
            //within 15 pixels to where we clicked.
            int screenBuffer = 15;
            //Here we have to use the ToScreencoordinate function because the MapClickedEventArgs does not provide the mouse screen position, only the 
            //mouse world position.
            ScreenPointF clickedPointF = ExtentHelper.ToScreenCoordinate(Map1.CurrentExtent, e.Position, (float)Map1.Width.Value, (float)Map1.Height.Value);
            ScreenPointF bufferPointF = new ScreenPointF(clickedPointF.X + screenBuffer, clickedPointF.Y);

            //Logic for converting screen coordinate values to world coordinate for the spatial query. Notice that the distance buffer for the spatial query
            //will change according to the zoom level while it remains the same for the screen buffer distance.
            double distanceBuffer = ExtentHelper.GetWorldDistanceBetweenTwoScreenPoints(Map1.CurrentExtent, clickedPointF, bufferPointF,
                                                                (float)Map1.Width.Value, (float)Map1.Height.Value, Map1.MapUnit, DistanceUnit.Meter);


            LayerOverlay streetOverlay = (LayerOverlay)Map1.CustomOverlays[0];
            ShapeFileFeatureLayer streetLayer = (ShapeFileFeatureLayer)streetOverlay.Layers["StreetLayer"];

            Collection<string> columnNames = new Collection<string>();
            columnNames.Add("FENAME");

            streetLayer.Open();
            Collection<Feature> features = streetLayer.FeatureSource.GetFeaturesNearestTo(new PointShape(e.Position.X, e.Position.Y),
                                           Map1.MapUnit, 1, columnNames, distanceBuffer, DistanceUnit.Meter);
            streetLayer.Close();

            //Adds the feature clicked on to the selected layer to be displayed as highlighed and with the name labeled.
            LayerOverlay selectOverlay = (LayerOverlay)Map1.CustomOverlays[1];
            InMemoryFeatureLayer selectLayer = (InMemoryFeatureLayer)selectOverlay.Layers["SelectLayer"];

            selectLayer.InternalFeatures.Clear();

            if (features.Count > 0)
            {
                selectLayer.InternalFeatures.Add(features[0]);
            }

            selectOverlay.Redraw();
        }
    }
}