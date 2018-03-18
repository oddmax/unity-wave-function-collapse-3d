namespace Core.Data
{
    public class OverlappingModelTile
    {
        public static OverlappingModelTile Empty = new OverlappingModelTile(new TileConfig("Empty"), 0);

        public TileConfig config;
        public string Id;

        public OverlappingModelTile(TileConfig tileConfig, int rotation)
        {
            config = tileConfig;
            this.Rotation = Rotation;
        }

        public int Rotation { get; set; }
    }
}