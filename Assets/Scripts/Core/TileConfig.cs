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

        public SimmetryType Symmetry { get; set; }
        public bool Unique { get; set; }
        public SimmetryType AdditionalRotation { get; set; }
        public double Weight { get; set; }
    }
}