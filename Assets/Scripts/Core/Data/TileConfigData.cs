using System.Collections.Generic;

namespace Core.Data
{
    public class TileConfigData<T> where T : TileConfig
    {
        private readonly Dictionary<string, T> Configs = new Dictionary<string, T>();

        public T GetConfig(string configId)
        {
            if (Configs.ContainsKey(configId))
            {
                return Configs[configId];
            }

            return null;
        }

        public void AddConfig(T config)
        {
            Configs.Add(config.Id, config);
        }
    }
}