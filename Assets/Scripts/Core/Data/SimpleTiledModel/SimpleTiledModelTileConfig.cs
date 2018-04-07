using UnityEngine;

namespace Core.Data.SimpleTiledModel
{
    public class SimpleTiledModelTileConfig : TileConfig
    {
        public double Weight { get; set; }
        public SymmetryType Symmetry { get; set; }

        public SimpleTiledModelTileConfig(GameObject prefab, SymmetryType symmetry) : base(prefab)
        {
            Weight = 1;
            Symmetry = symmetry;
        }
    }
}