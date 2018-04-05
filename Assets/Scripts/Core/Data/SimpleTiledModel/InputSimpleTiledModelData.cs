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

        public List<NeighborData> NeighborDatas { get; private set; }
        
        public byte[] tilesConfigIds;

        public List<string> GetSubset(string subsetName)
        {
            if (subsetName == SimpleTiledModelParams.DEFAULT_SUBSET)
            {
                return null;
            }

            return null;
        }

        public void SetNeighbors(List<NeighborData> neighborDatas)
        {
            NeighborDatas = neighborDatas;
        }
    }
}