using Core.Data;
using Core.Model;
using UnityEngine;

namespace Core
{
    public class WaveFunctionCollapseRenderer : MonoBehaviour
    {
        [SerializeField] 
        private GameObject uncollapsedTilePrefab;
        
        private int width;
        private int depth;
        private float gridSize = 1f;
        private IModel model;
        private GameObject[,] tiles;

        public void Init(IModel model)
        {
            this.model = model;
            Clear();
            depth = model.ModelParam.Depth;
            width = model.ModelParam.Width;
            
            tiles = new GameObject[width, depth];
        }

        public void UpdateStates()
        {
            Debug.Log("Update states");
            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < depth; z++)
                {
                    var cellState = model.GetCellStateAt(x, z);
                    var tileGameObject = tiles[x, z];
                    if (tileGameObject != null)
                    {
                        var uncollapsedTileView = tileGameObject.GetComponent<UncollapsedTileView>();
                        if (uncollapsedTileView != null)
                        {
                            uncollapsedTileView.UpdateState(cellState.EntropyLevel);
                            if (cellState.Collapsed)
                            {
                                if (Application.isPlaying)
                                {
                                    Destroy(tileGameObject);
                                }
                                else
                                {
                                    DestroyImmediate(tileGameObject);
                                }
                                tileGameObject = null;
                            }
                        }
                    }

                    if (tileGameObject == null)
                    {
                        tiles[x, z] = CreateTile(x, z, cellState);
                    }
                }
            }
        }
        
        public void Clear()
        {
            for (int i = 0; i < transform.childCount; i++){
                GameObject go = transform.GetChild(i).gameObject;
                if (Application.isPlaying)
                {
                    Destroy(go);
                }
                else
                {
                    DestroyImmediate(go);
                }
            }
        }

        private GameObject CreateTile(int x, int z, CellState cellState)
        {
            GameObject tileObject;
            if (cellState.Collapsed)
            {
                var tile = cellState.Tile;
                if (tile.Config.Prefab == null)
                {
                    return null;
                }
                tileObject = Instantiate(tile.Config.Prefab, transform);
                tileObject.transform.localPosition = new Vector3(x, 0, z);
                Debug.Log("Rotation " + tile.Rotation);
                tileObject.transform.localEulerAngles = new Vector3(0, tile.Rotation * 90, 0);
                return tileObject;
            }

            tileObject = Instantiate(uncollapsedTilePrefab, transform);
            tileObject.transform.localPosition = new Vector3(x, 0, z);
            return tileObject;
        }
        
        private void OnDrawGizmos(){
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(new Vector3(width*gridSize/2f-gridSize*0.5f, 0f, depth*gridSize/2f-gridSize*0.5f),
                new Vector3(width*gridSize, gridSize, depth*gridSize));
        }
    }
}