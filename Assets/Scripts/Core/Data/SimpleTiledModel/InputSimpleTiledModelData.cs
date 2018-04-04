using System.Collections.Generic;

namespace Core.Data.SimpleTiledModel
{
    public class InputSimpleTiledModelData
    {
        public TileConfigData<SimpleTiledModelTileConfig> TileConfigData { get; private set; }

        public InputSimpleTiledModelData(TileConfigData<SimpleTiledModelTileConfig> tileConfigData)
        {
            TileConfigData = tileConfigData;
        }

        public List<SimpleTiledModelTileConfig> TileConfigs { get; set; }
        public List<NeighborData> NeighborDatas { get; private set; }

        public List<string> GetSubset(string subsetName)
        {
            throw new System.NotImplementedException();
        }

        public void SetNeighbors(List<NeighborData> neighborDatas)
        {
            NeighborDatas = neighborDatas;
        }
    }
}