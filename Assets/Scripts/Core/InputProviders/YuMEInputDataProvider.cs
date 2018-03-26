using System.Collections.Generic;
using Core.Data;
using UnityEngine;

namespace Core.InputProviders
{
    public class YuMEInputDataProvider : MonoBehaviour, IInputDataProvider
    {
        [SerializeField] 
        private YuME_GizmoGrid grid;
        
        [SerializeField] 
        private GameObject[] levelsList;
        
        [SerializeField] 
        private GameObject[] tilesPrefabs;

        private Dictionary<string, GameObject> tilesPrefabMap;

        private void Awake()
        {
            tilesPrefabMap = new Dictionary<string, GameObject>();
            foreach (var tilePrefab in tilesPrefabs)
            {
                tilesPrefabMap.Add(tilePrefab.name, tilePrefab);
            }
        }

        public InputOverlappingData GetInputOverlappingData()
        {
            var width = grid.gridWidth;
            var depth = grid.gridDepth;

            var offsetWidth = Mathf.CeilToInt(width / 2);
            var offsetDepth = Mathf.CeilToInt(depth / 2);

            var inputData = new InputOverlappingData(width, depth);
            
            foreach (var levelContainer in levelsList)
            {
                foreach (Transform child in levelContainer.transform)
                {
                    var x = Mathf.RoundToInt(child.localPosition.x) + offsetWidth;
                    var y = Mathf.RoundToInt(child.localPosition.z) + offsetDepth;
                    int rotation = (int)((360 - child.localEulerAngles.z)/90);
                    if (rotation == 4)
                    {
                        rotation = 0;
                    }
                    
                    Debug.Log(string.Format("X: {0}; Y: {1}, id: {2}", x, y, child.gameObject.name));
                    inputData.SetTile(x, y, tilesPrefabMap[child.gameObject.name], rotation);
                } 
            }

            return inputData;
        }

        public InputSimpleTiledModelData GetInputSimpleTiledData()
        {
            throw new System.NotImplementedException();
        }

        private GameObject GetPrefab(string name)
        {
            foreach (var tilePrefab in tilesPrefabs)
            {
                if (tilePrefab.name == name)
                {
                    return tilePrefab;
                }
            }

            return null;
        }
        
        private void OnDrawGizmos(){
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(new Vector3(-0.5f, 0f, -0.5f),
                new Vector3(grid.gridWidth, 1, grid.gridDepth));
        }
    }
}