namespace Core.Data.OverlappingModel
{
    public class OverlappingModelTile : Tile<TileConfig>
    {
        public static OverlappingModelTile Empty = new OverlappingModelTile(new TileConfig(null), 0);

        public string Id { get; private set; }

        public OverlappingModelTile(TileConfig tileConfig, int rotation) : base(tileConfig, rotation)
        {
            Id = Config.Id + "|" + Rotation;
        }
    }
}