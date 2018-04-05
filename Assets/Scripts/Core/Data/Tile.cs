namespace Core.Data
{
    public abstract class Tile<T> : ITile where T:TileConfig
    {
        protected Tile(T config, int rotation)
        {
            Rotation = rotation;
            Config = config;
        }

        public int Rotation { get; private set; }
        public TileConfig Config { get; private set; }
    }
}