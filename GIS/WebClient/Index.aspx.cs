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
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //MainMap.MapBackground.BackgroundBrush = new GeoSolidBrush(GeoColor.FromHtml("#E5E3DF"));
                //MainMap.CurrentExtent = new RectangleShape(-131.22, 55.05, -54.03, 16.91);
                //MainMap.MapUnit = GeographyUnit.DecimalDegree;

                //WmsOverlay wmsOverlay = new WmsOverlay("WMS Overlay");
                /////wmsOverlay.Parameters.Add("LAYERS", "Map Suite World Map Kit");
                //wmsOverlay.Parameters.Add("LAYERS", "Display A Simple Map");
                ////wmsOverlay.Parameters.Add("STYLES", "DEFAULT");

                ///* MainMap.CurrentExtent = new RectangleShape(-125, 72, 50, -46);*/
                //// WorldMapKitWmsWebOverlay worldMapKitOverlay = new WorldMapKitWmsWebOverlay();
                ////MainMap.CustomOverlays.Add(worldMapKitOverlay);


                ////Here you add your WMS Server uri
                //wmsOverlay.ServerUris.Add(new Uri("http://localhost:62626/WmsHandler.axd"));

                //String shpFile = @"Ukraine\places.shp";
                //ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(Util.dataPath + shpFile);

                //MainMap.CustomOverlays.Add(wmsOverlay);


                MainMap.MapUnit = GeographyUnit.DecimalDegree;

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

                MainMap.StaticOverlay.Layers.Add(roadsLayer);
                MainMap.StaticOverlay.Layers.Add(roadsNameLayer);
                MainMap.StaticOverlay.Layers.Add(cityNameLayer);
                MainMap.StaticOverlay.Layers.Add(placesLayer);
                MainMap.StaticOverlay.Layers.Add(palceNameLayer);

                MainMap.CurrentExtent = new RectangleShape(-1.93, 53.68, 50, 46);
                MainMap.CurrentExtent.ScaleTo(0.001);
            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {

        }
    }

    //public partial class Index : System.Web.UI.Page
    //{
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        if (!Page.IsPostBack)
    //        {
    //            MainMap.MapBackground.BackgroundBrush = new GeoSolidBrush(GeoColor.GeographicColors.Dirt);
    //            MainMap.CurrentExtent = new RectangleShape(-95.2599710226211, 38.9665298461753, -95.2441567182693, 38.9552645683128);
    //            MainMap.MapUnit = GeographyUnit.DecimalDegree;

    //            WorldMapKitWmsWebOverlay worldMapKitOverlay = new WorldMapKitWmsWebOverlay();
    //            MainMap.CustomOverlays.Add(worldMapKitOverlay);

    //            WmsOverlay wmsOverlay = new WmsOverlay("WMS Overlay");
    //            wmsOverlay.TileType = TileType.SingleTile;
    //            wmsOverlay.IsBaseOverlay = false;
    //            wmsOverlay.Parameters.Add("LAYERS", "Simulate Vehicle Tracking");
                
    //            //Here you add your WMS Server uri
    //            wmsOverlay.ServerUris.Add(new Uri("http://localhost:62626/WmsHandler.axd"));
    //            MainMap.CustomOverlays.Add(wmsOverlay);
    //        }
    //    }
    //}

    //public partial class Index : System.Web.UI.Page
    //{
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        string mapSuitePath = @"D:\ProgramFiles\Map Suite Web Evaluation Edition 7.0";

    //        // Set the Map Unit. The reason for setting it to DecimalDegrees is that is what the shapefile's unit of measure is inherently in.
    //        MainMap.MapUnit = GeographyUnit.DecimalDegree;
    //        // We create a new Layer and pass the path to a Shapefile into its constructor.
    //        ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(mapSuitePath + @"\Samples\CSharp Samples\SampleData\World\cntry02.shp");
    //        // Set the worldLayer with a preset Style, as AreaStyles.Country1 has YellowGreen background and black border, our worldLayer will have the same render style. 
    //        worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
    //        // This setting will apply from ZoonLevel01 to ZoomLevel20, that means we can see the world the same style with ZoomLevel01 all the time no matter how far we zoom out/in.
    //        worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

    //        ShapeFileFeatureLayer capitalLayer = new ShapeFileFeatureLayer(mapSuitePath + @"\Samples\CSharp Samples\SampleData\World\capital.shp");
    //        // Similarly, we use the presetPointStyle for cities.
    //        capitalLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle = PointStyles.Capital3;
    //        // These settings will apply from ZoonLevel01 to ZoomLevel20, that means we can see city symbols the same style with ZoomLevel01 all the time no matter how far we zoom out/in.
    //        capitalLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

    //        // We create a new Layer for labeling the capitals.
    //        ShapeFileFeatureLayer capitalLabelLayer = new ShapeFileFeatureLayer(mapSuitePath + @"\Samples\CSharp Samples\SampleData\World\capital.shp");
    //        // We use the preset TextStyle. Here we passed in the “CITY_NAME”, which is the name of the field we want to label on map.
    //        capitalLabelLayer.ZoomLevelSet.ZoomLevel01.DefaultTextStyle = TextStyles.Capital3("CITY_NAME");
    //        capitalLabelLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
    //        // As the map is drawn by tiles, it needs to draw on the margin to make sure the text is complete after we joining the tiles together.
    //        // Change the number to another one (for example 0) and you can see the difference.
    //        capitalLabelLayer.DrawingMarginPercentage = 50;

    //        // We need to add the world layer to map's Static Overlay.
    //        MainMap.StaticOverlay.Layers.Add(worldLayer);
    //        MainMap.StaticOverlay.Layers.Add(capitalLayer);
    //        MainMap.StaticOverlay.Layers.Add(capitalLabelLayer);

    //        // Set a proper extent for the map, that's the place you want it to display.
    //        MainMap.CurrentExtent = new RectangleShape(5, 78, 30, 26);

    //        // Set the background color to make the map beautiful.
    //        MainMap.MapBackground.BackgroundBrush = new GeoSolidBrush(GeoColor.GeographicColors.ShallowOcean);
    //    }
    //}
}