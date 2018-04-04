namespace Core.Data.SimpleTiledModel
{
    public class NeighborData
    {
        public string LeftNeighborId { get; set; }
        public string RightNeighborId { get; set; }
        public SimpleTiledModelTileConfig LeftNeighborConfig { get; set; }
        public SimpleTiledModelTileConfig RightNeighborConfig { get; set; }
        public int LeftRotation { get; set; }
        public int RightRotation { get; set; }
        
        public NeighborData(SimpleTiledModelTileConfig leftNeighborConfig, SimpleTiledModelTileConfig rightNeighborConfig, int leftRotation, int rightRotation)
        {
            LeftNeighborConfig = leftNeighborConfig;
            RightNeighborConfig = rightNeighborConfig;
            LeftRotation = leftRotation;
            RightRotation = rightRotation;
        }
    }
}