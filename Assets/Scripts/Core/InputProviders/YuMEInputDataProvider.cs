using System.Collections.Generic;
using Core.Data;
using Core.Data.OverlappingModel;
using Core.Data.SimpleTiledModel;
using UnityEngine;

namespace Core.InputProviders
{
    public class YuMEInputDataProvider : MonoBehaviour, IInputDataProvider
    {   
        [SerializeField] 
        private int width;
        
        [SerializeField] 
        private int depth;
        
        [SerializeField] 
        private GameObject[] levelsList;
        
        [SerializeField] 
        private GameObject[] tilesPrefabs;

        private Dictionary<string, GameObject> tilesPrefabMap;

        private void Awake()
        {
            FillPrefabMap();
        }

        public InputOverlappingData GetInputOverlappingData()
        {
            if (Application.isPlaying == false)
            {
                FillPrefabMap();
            }

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
                    
                    Debug.Log(string.Format("X: {0}; Y: {1}, id: {2}", x, y, tilesPrefabMap[child.gameObject.name].name));
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

        private void FillPrefabMap()
        {
            tilesPrefabMap = new Dictionary<string, GameObject>();
            foreach (var tilePrefab in tilesPrefabs)
            {
                tilesPrefabMap.Add(tilePrefab.name, tilePrefab);
            }
        }
        
        private void OnDrawGizmos(){
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(new Vector3(-0.5f, 0f, -0.5f),
                new Vector3(width, 1, depth));
        }
    }
}