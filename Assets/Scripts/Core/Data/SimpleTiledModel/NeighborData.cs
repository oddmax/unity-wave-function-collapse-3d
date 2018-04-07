namespace Core.Data.SimpleTiledModel
{
    public class NeighborData
    {
        public TileConfig LeftNeighborConfig { get; private set; }
        public TileConfig RightNeighborConfig { get; private set; }
        public int LeftRotation { get; private set; }
        public int RightRotation { get; private set; }
        
        public NeighborData(TileConfig leftNeighborConfig, TileConfig rightNeighborConfig, int leftRotation, int rightRotation)
        {
            LeftNeighborConfig = leftNeighborConfig;
            RightNeighborConfig = rightNeighborConfig;
            LeftRotation = leftRotation;
            RightRotation = rightRotation;
        }
    }
}