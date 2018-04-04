using System.Collections.Generic;

namespace Core.Data
{
    public class TileConfigData<T> where T : TileConfig
    {
        private readonly Dictionary<string, T> ConfigsMap = new Dictionary<string, T>();

        public TileConfigData()
        {
            ConfigsList = new List<T>();
        }

        public List<T> ConfigsList { get; private set; }

        public T GetConfig(string configId)
        {
            if (ConfigsMap.ContainsKey(configId))
            {
                return ConfigsMap[configId];
            }

            return null;
        }

        public void AddConfig(T config)
        {
            ConfigsMap.Add(config.Id, config);
            ConfigsList.Add(config);
        }
    }
}