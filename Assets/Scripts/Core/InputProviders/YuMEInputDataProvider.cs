using System;
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

        [SerializeField] 
        private SymmetrySetsScriptableObject symmetrySets;

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

            var inputData = new InputOverlappingData(width, depth);
            
            var offsetWidth = Mathf.CeilToInt(width / 2);
            var offsetDepth = Mathf.CeilToInt(depth / 2);
            
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
            if (Application.isPlaying == false)
            {
                FillPrefabMap();
            }
            
            var tileConfigData = CreateTileConfigData(tilePrefab =>
            {
                var symmetry = symmetrySets.GetSymmetryByTileName(tilePrefab.name);
                var config = new SimpleTiledModelTileConfig(tilePrefab, symmetry);
                
                return config;
            });
            
            var inputData = new InputSimpleTiledModelData(tileConfigData);

            ExecuteForEachTile((tile, x, z, rotation) =>
            {
                //TODO implement
            });

            return inputData;
        }

        private TileConfigData<T> CreateTileConfigData<T>(Func<GameObject, T> creator) where T : TileConfig
        {
            var tileConfigData = new TileConfigData<T>();
            ExecuteForEachTile((tile, x, z, rotation) =>
            {
                var tileConfig = tileConfigData.GetConfig(tile.name);
                if (tileConfig == null)
                {
                    tileConfigData.AddConfig(creator(GetPrefab(tile.name)));
                }
            });
            return tileConfigData;
        }

        private void ExecuteForEachTile(Action<GameObject, int, int, int> action)
        {
            var offsetWidth = Mathf.CeilToInt(width / 2);
            var offsetDepth = Mathf.CeilToInt(depth / 2);
            
            foreach (var levelContainer in levelsList)
            {
                foreach (Transform child in levelContainer.transform)
                {
                    var x = Mathf.RoundToInt(child.localPosition.x) + offsetWidth;
                    var z = Mathf.RoundToInt(child.localPosition.z) + offsetDepth;
                    int rotation = (int)((360 - child.localEulerAngles.z)/90);
                    if (rotation == 4)
                    {
                        rotation = 0;
                    }

                    action(child.gameObject, x, z, rotation);
                } 
            }
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