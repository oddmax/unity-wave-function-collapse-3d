namespace Core.Data.SimpleTiledModel
{
    public class NeighborData
    {
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