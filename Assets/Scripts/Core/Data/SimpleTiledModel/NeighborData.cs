namespace Core.Data.SimpleTiledModel
{
    public class NeighborData
    {
        public string LeftNeighborId { get; set; }
        public string RightNeighborId { get; set; }
        public TileConfig LeftNeighborConfig { get; set; }
        public TileConfig RightNeighborConfig { get; set; }
        public int LeftRotation { get; set; }
        public int RightRotation { get; set; }
        
        public NeighborData(TileConfig leftNeighborConfig, TileConfig rightNeighborConfig, int leftRotation, int rightRotation)
        {
            LeftNeighborConfig = leftNeighborConfig;
            RightNeighborConfig = rightNeighborConfig;
            LeftRotation = leftRotation;
            RightRotation = rightRotation;
        }
    }
}