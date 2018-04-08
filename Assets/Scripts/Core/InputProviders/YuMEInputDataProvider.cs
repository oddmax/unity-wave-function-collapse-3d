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
            
            var tileConfigsData = CreateTileConfigData(tilePrefab =>
            {
                var symmetry = symmetrySets.GetSymmetryByTileName(tilePrefab.name);
                Debug.Log(tilePrefab.name + " " + symmetry);
                var config = new SimpleTiledModelTileConfig(tilePrefab, symmetry);
                
                return config;
            });
            
            var inputData = new InputSimpleTiledModelData(tileConfigsData);
            var tiles = new SimpleTiledModelTile[width, depth];
            
            ExecuteForEachTile((tileGo, x, z, rotation) =>
            {
                tiles[x, z] = new SimpleTiledModelTile(tileConfigsData.GetConfig(tileGo.name), rotation);
            });

            var neighbors = new Dictionary<string, NeighborData>();

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    var currentTile = tiles[x, z];
                    if (currentTile == null) continue;
                    
                    for (var offset = 0; offset < 2; offset++){
                        var rx = x + 1 - offset;
                        var rz = z + offset;
                        if (rx >= width || rz >= depth) continue;

                        var currentTileRotation = Card(currentTile.Rotation + offset);
                        var nextTile = tiles[rx, rz];
                        
                        if (nextTile == null) continue;
                        
                        var nextTileRotation = Card(nextTile.Rotation + offset);
                        
                        string key = currentTile.Config.Id + "." + currentTileRotation + "|" + nextTile.Config.Id + "." + nextTileRotation ;
                        
                        if (neighbors.ContainsKey(key)) continue;
                        Debug.Log(key);
                        
                        neighbors.Add(key, new NeighborData(currentTile.Config, nextTile.Config, currentTileRotation, nextTileRotation));

                        DrawDebugLine(x, z, rx, rz);
                    }
                }
            }
            
            inputData.SetNeighbors(neighbors.Values.ToList());

            return inputData;
        }
        
        public int Card(int n){
            return (n%4 + 4)%4;
        }

        private void DrawDebugLine(int xs, int zs, int xt, int zt)
        {
            var offsetWidth = Mathf.CeilToInt(width / 2);
            var offsetDepth = Mathf.CeilToInt(depth / 2);

            xs -= offsetWidth;
            zs -= offsetDepth; 
            
            xt -= offsetWidth;
            zt -= offsetDepth;
            
            Debug.DrawLine(
                transform.TransformPoint(new Vector3(xs, 0.5f, zs)),
                transform.TransformPoint(new Vector3(xt, 0.5f, zt)), Color.red, 9.0f, false);
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
                    int rotation = (int)((360 + Mathf.Round(child.localEulerAngles.y))/90);
                    rotation = rotation % 4;

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