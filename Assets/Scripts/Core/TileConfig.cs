namespace Core
{
    public class TileConfig
    {
        public static TileConfig Empty = new TileConfig("empty");
        
        public string Id;

        public TileConfig(string id)
        {
            Id = id;
        }
    }
}