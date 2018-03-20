namespace Core.Data
{
    public class OverlappingModelTile
    {
        public static OverlappingModelTile Empty = new OverlappingModelTile(new TileConfig("Empty"), 0);

        public TileConfig Config { get; private set; }
        public string Id { get; private set; }

        public OverlappingModelTile(TileConfig tileConfig, int rotation)
        {
            Config = tileConfig;
            Rotation = rotation;
            Id = Config.Id + "|" + Rotation;
        }

        public int Rotation { get; set; }
    }
}