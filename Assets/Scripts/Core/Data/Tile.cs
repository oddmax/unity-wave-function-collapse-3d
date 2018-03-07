namespace Core.Data
{
    public struct Tile
    {
        public TileConfig Config;
        public int Rotation;

        public Tile(TileConfig config, int rotation)
        {
            Config = config;
            Rotation = rotation;
        }
    }
}