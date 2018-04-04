using System;
using System.Collections.Generic;
using System.Linq;
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
            
            var tileConfigData = CreateTileConfigData(tilePrefab => new TileConfig(tilePrefab));

            var inputData = new InputOverlappingData(tileConfigData, width, depth);

            ExecuteForEachTile((tile, x, z, rotation) =>
            {
                inputData.SetTile(tileConfigData.GetConfig(tile.name), x, z, rotation);
            });

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
            var tiles = new SimpleTiledModelTile[width, depth];
            
            ExecuteForEachTile((tileGo, x, z, rotation) =>
            {
                tiles[x, z] = new SimpleTiledModelTile(tileConfigData.GetConfig(tileGo.name), rotation);
            });

            var neighbors = new Dictionary<string, NeighborData>();
            ExecuteForEachTile((tileGo, x, z, rotation) =>
            {
                var currentTile = tiles[x, z];
                for (int offset = 0; offset < 2; offset++){
                    int rx = x + 1 - offset;
                    int rz = z + offset;
                    if (rx < width && rz < depth)
                    {
                        var currentTileRotation = (currentTile.Rotation + offset)%4;
                        var nextTile = tiles[x, z];
                        var nextTileRotation = (nextTile.Rotation + offset)%4;
                        string key = currentTile.Config.Id + "." + currentTileRotation + "|" + nextTile.Config.Id + "." + nextTileRotation ;
                        
                        if (neighbors.ContainsKey(key)) continue;
                        
                        neighbors.Add(key, new NeighborData(currentTile.Config, nextTile.Config, currentTileRotation, nextTileRotation));
                        Debug.DrawLine(
                            transform.TransformPoint(new Vector3(x + 0f, 1f, z + 0f)),
                            transform.TransformPoint(new Vector3(x + 1f - offset, 1f, z + 0f + offset)), Color.red, 9.0f, false);
                    }
                }
            });

            inputData.SetNeighbors(neighbors.Values.ToList());

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