namespace Core.Data
{
    public abstract class Tile<T> where T:TileConfig
    {
        public int Rotation { get; protected set; }
        public T Config { get; protected set; }
    }
}