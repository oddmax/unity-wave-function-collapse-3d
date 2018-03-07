using System.Collections.Generic;
using Core.Data;

namespace Core
{
    public class InputSimpleTiledModelData
    {
        public int Size { get; set; }
        public bool Unique { get; set; }
        public List<TileConfig> TileConfigs { get; set; }
        public List<NeighborData> NeighborDatas { get; set; }

        public List<string> GetSubset(string subsetName)
        {
            throw new System.NotImplementedException();
        }
    }
}