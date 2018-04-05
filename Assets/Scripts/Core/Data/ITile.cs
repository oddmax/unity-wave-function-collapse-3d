namespace Core.Data
{
    public interface ITile
    {
        int Rotation { get; }
        
        TileConfig Config { get; }
    }
}