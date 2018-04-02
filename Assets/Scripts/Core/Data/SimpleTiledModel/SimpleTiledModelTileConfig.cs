using UnityEngine;

namespace Core.Data.SimpleTiledModel
{
    public class SimpleTiledModelTileConfig : TileConfig
    {
        public double Weight { get; set; }
        public SimmetryType Symmetry { get; set; }

        public SimpleTiledModelTileConfig(GameObject prefab) : base(prefab)
        {
        }
    }
}