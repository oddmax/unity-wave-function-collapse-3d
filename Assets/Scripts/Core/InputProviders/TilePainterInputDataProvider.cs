using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Core.Data.OverlappingModel;
using Core.Data.SimpleTiledModel;
using UnityEngine;

namespace Core.InputProviders
{
    public class TilePainterInputDataProvider : InputDataProvider
    {   
        [SerializeField] 
        private int width;
        
        [SerializeField] 
        private int height;
        
        [SerializeField] 
        private int depth;
        
        [SerializeField] 
        private TilePainter tilePainter;
        
        [SerializeField] 
        private GameObject tilesParent;
        
        [SerializeField] 
        private GameObject[] tilesPrefabs;

        [SerializeField] 
        private SymmetrySetsScriptableObject symmetrySets;

        private Dictionary<string, GameObject> tilesPrefabMap;

        private void Awake()
        {
            FillPrefabMap();
        }

        public override InputOverlappingData GetInputOverlappingData()
        {
            if (Application.isPlaying == false)
            {
                FillPrefabMap();
            }
            
            var tileConfigData = CreateTileConfigData(tilePrefab => new TileConfig(tilePrefab));

            var inputData = new InputOverlappingData(tileConfigData, width, depth);

            ExecuteForEachTile((tile, pos, rotation) =>
            {
                inputData.SetTile(tileConfigData.GetConfig(tile.name), (int)pos.x, (int)pos.z, rotation);
            });

            return inputData;
        }

        public override InputSimpleTiledModelData GetInputSimpleTiledData()
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
            var tiles = new SimpleTiledModelTile[width, height, depth];
            
            ExecuteForEachTile((tileGo, pos, rotation) =>
            {
                tiles[(int)pos.x, (int)pos.y, (int)pos.z] = new SimpleTiledModelTile(tileConfigsData.GetConfig(tileGo.name), rotation);
            });

            var neighbors = new Dictionary<string, NeighborData>();

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            for (int z = 0; z < depth; z++)
            {
                var currentTile = tiles[x, y, z];
                if (currentTile == null) continue;

                SimpleTiledModelTile nextTile;
                string key;
                
                for (var offset = 0; offset < 2; offset++){
                    var rx = x + 1 - offset;
                    var rz = z + offset;
                    if (rx >= width || rz >= depth) continue;

                    var currentTileRotation = Card(currentTile.Rotation + offset);
                    nextTile = tiles[rx, y, rz];
                    
                    if (nextTile == null) continue;
                    
                    var nextTileRotation = Card(nextTile.Rotation + offset);
                    
                    key = currentTile.Config.Id + "." + currentTileRotation + " | " + nextTile.Config.Id + "." + nextTileRotation ;
                    
                    if (neighbors.ContainsKey(key)) continue;
                    Debug.Log(key);
                    
                    neighbors.Add(key, new NeighborData(currentTile.Config, nextTile.Config, currentTileRotation, nextTileRotation));
                    DrawDebugLine(new Vector3(x, y - 0.5f, z), new Vector3(rx, y - 0.5f, rz));
                }
                
                if(y == 0) continue;
                nextTile = tiles[x, y - 1, z];
                if (nextTile == null) continue;
                    
                key = currentTile.Config.Id + "." + currentTile.Rotation + " | " + nextTile.Config.Id + "." + nextTile.Rotation + " |vertical" ;
                if (neighbors.ContainsKey(key)) continue;
                Debug.Log(key);
                
                neighbors.Add(key, new NeighborData(currentTile.Config, nextTile.Config, currentTile.Rotation, nextTile.Rotation, false));
                DrawDebugLine(new Vector3(x, y, z), new Vector3(x, y - 1, z ));
            }
            
            
            inputData.SetNeighbors(neighbors.Values.ToList());

            return inputData;
        }
        
        public int Card(int n){
            return (n%4 + 4)%4;
        }

        private void DrawDebugLine(Vector3 start, Vector3 target)
        {
            Debug.DrawLine(transform.TransformPoint(start), transform.TransformPoint(target), Color.red, 9.0f, false);
        }

        private TileConfigData<T> CreateTileConfigData<T>(Func<GameObject, T> creator) where T : TileConfig
        {
            var tileConfigData = new TileConfigData<T>();
            ExecuteForEachTile((tile, pos, rotation) =>
            {
                var tileConfig = tileConfigData.GetConfig(tile.name);
                if (tileConfig == null)
                {
                    tileConfigData.AddConfig(creator(GetPrefab(tile.name)));
                }
            });
            return tileConfigData;
        }

        private void ExecuteForEachTile(Action<GameObject, Vector3, int> action)
        {
            foreach (Transform child in tilesParent.transform)
            {
                var x = Mathf.RoundToInt(child.localPosition.x);
                var y = Mathf.RoundToInt(child.localPosition.y);
                var z = Mathf.RoundToInt(child.localPosition.z);
                int rotation = (int)((360 + Mathf.Round(child.localEulerAngles.y))/90);
                rotation = rotation % 4;

                action(child.gameObject, new Vector3(x, y, z), rotation);
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
            Gizmos.DrawWireCube(new Vector3(-0.5f + width/2, 0f, -0.5f + depth/2),
                new Vector3(width, 1, depth));
        }
    }
}