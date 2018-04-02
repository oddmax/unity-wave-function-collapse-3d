using System.Collections.Generic;

namespace Core.Data.SimpleTiledModel
{
    public class InputSimpleTiledModelData
    {
        public bool Unique { get; set; }
        public TileConfigData<SimpleTiledModelTileConfig> TileConfigData { get; private set; }

        public InputSimpleTiledModelData(TileConfigData<SimpleTiledModelTileConfig> tileConfigData)
        {
            TileConfigData = tileConfigData;
        }

        public List<SimpleTiledModelTileConfig> TileConfigs { get; set; }
        public List<NeighborData> NeighborDatas { get; set; }

        public List<string> GetSubset(string subsetName)
        {
            throw new System.NotImplementedException();
        }
    }
}