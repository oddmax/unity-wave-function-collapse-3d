using UnityEngine;

namespace Core.Data
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