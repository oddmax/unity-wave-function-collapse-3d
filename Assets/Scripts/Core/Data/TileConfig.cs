using UnityEngine;

namespace Core.Data
{
    public class TileConfig
    {
        public string Id { get; private set; }
        public GameObject Prefab { get; private set; }

        public TileConfig(GameObject prefab)
        {
            Prefab = prefab;
            if (prefab == null)
            {
                Id = "Empty";
            }
            else
            {
                Id = prefab.name;
            }
        }

        public override string ToString()
        {
            return Id;
        }
    }
}