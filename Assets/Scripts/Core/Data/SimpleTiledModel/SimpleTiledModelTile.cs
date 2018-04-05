namespace Core.Data.SimpleTiledModel
{
    public class SimpleTiledModelTile : Tile<SimpleTiledModelTileConfig>
    {
        public SimpleTiledModelTile(SimpleTiledModelTileConfig config, int rotation) : base(config, rotation)
        {
        }

        public override string ToString()
        {
            return Config.Id + " " + Rotation;
        }
    }
}