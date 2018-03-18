namespace Core.Data
{
    public class SimpleTiledModelTileConfig : TileConfig
    {
        public double Weight { get; set; }
        public SimmetryType Symmetry { get; set; }

        public SimpleTiledModelTileConfig(string id) : base(id)
        {
        }
    }
}