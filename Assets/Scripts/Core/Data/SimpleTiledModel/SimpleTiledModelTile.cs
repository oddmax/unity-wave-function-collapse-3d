namespace Core.Data.SimpleTiledModel
{
    public class SimpleTiledModelTile
    {
        public SimpleTiledModelTileConfig Config;
        public int Rotation;

        public SimpleTiledModelTile(SimpleTiledModelTileConfig config, int rotation)
        {
            Config = config;
            Rotation = rotation;
        }

        public override string ToString()
        {
            return Config.Id + " " + Rotation;
        }
    }
}