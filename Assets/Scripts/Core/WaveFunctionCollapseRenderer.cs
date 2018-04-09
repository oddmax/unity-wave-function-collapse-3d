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
        private int height;
        private int depth;
        private float gridSize = 1f;
        private IModel3d model;
        private GameObject[,,] tiles;

        public void Init(IModel3d model)
        {
            this.model = model;
            Clear();
            depth = model.ModelParam.Depth;
            height = model.ModelParam.Height;
            width = model.ModelParam.Width;
            
            tiles = new GameObject[width, height, depth];
        }

        public void UpdateStates()
        {
            Debug.Log("Update states");
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            for (var z = 0; z < depth; z++)
            {
                var cellState = model.GetCellStateAt(x, y, z);
                var tileGameObject = tiles[x, y, z];
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
                    tiles[x, y, z] = CreateTile(x, y, z, cellState);
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

        private GameObject CreateTile(int x, int y, int z, CellState cellState)
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
                tileObject.transform.localPosition = new Vector3(x, y, z);
                Debug.Log("Rotation " + tile.Rotation);
                var oldRot = tileObject.transform.localEulerAngles;
                oldRot.y = tile.Rotation * 90;
                tileObject.transform.localEulerAngles = oldRot;
                return tileObject;
            }

            tileObject = Instantiate(uncollapsedTilePrefab, transform);
            tileObject.transform.localPosition = new Vector3(x, y, z);
            return tileObject;
        }
        
        private void OnDrawGizmos(){
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(new Vector3(width*gridSize/2f-gridSize*0.5f, height*gridSize/2f-gridSize*0.5f, depth*gridSize/2f-gridSize*0.5f),
                new Vector3(width*gridSize, height*gridSize, depth*gridSize));
        }
    }
}