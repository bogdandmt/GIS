using System;
using System.Collections.ObjectModel;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.WmsServerEdition;

namespace WmsPlugin
{
    public class WorldMapKitPlugin : WmsLayerPlugin
    {
        protected override RectangleShape GetBoundingBoxCore(string crs)
        {
            RectangleShape extent = new RectangleShape(-131.22, 55.05, -54.03, 16.91);
            if (crs.Equals("EPSG:900913", StringComparison.OrdinalIgnoreCase))
            {
                extent = new RectangleShape(-13939426.6371, 6701997.4056, -7812401.86, 2626987.386962);
            }
            return extent;
        }

        protected override MapConfiguration GetMapConfigurationCore(string style, string crs)
        {
            WorldMapKitLayer worldMapKitLayer = new WorldMapKitLayer();
            switch (crs)
            {
                case "EPSG:4326":
                    break;
                case "EPSG:900913":
                    worldMapKitLayer.Projection = WorldMapKitProjection.SphericalMercator;
                    break;
            }

            MapConfiguration mapConfiguration = new MapConfiguration();
            mapConfiguration.Layers.Add("worldMapKitLayer", worldMapKitLayer);
            return mapConfiguration;
        }

        protected override GeographyUnit GetGeographyUnitCore(string crs)
        {
            GeographyUnit geographyUnit = GeographyUnit.DecimalDegree;
            switch (crs)
            {
                case "EPSG:4326":
                    break;
                case "EPSG:900913":
                    geographyUnit = GeographyUnit.Meter;
                    break;
            }

            return geographyUnit;
        }

        protected override string GetNameCore()
        {
            return "Map Suite World Map Kit";
        }

        protected override Collection<string> GetProjectionsCore()
        {
            Collection<string> projections = new Collection<string>();
            projections.Add("EPSG:4326");
            projections.Add("EPSG:900913");
            return projections;

        }
    }
}