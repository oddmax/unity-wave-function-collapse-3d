using UnityEngine;

namespace Core
{
    public class WaveFunctionCollapseRenderer : MonoBehaviour
    {
        private int width;
        private int depth;
        private float gridSize = 1f;

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

        void OnDrawGizmos(){
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(new Vector3(width*gridSize/2f-gridSize*0.5f, 0f, depth*gridSize/2f-gridSize*0.5f),
                new Vector3(width*gridSize, gridSize, depth*gridSize));
        }

        public void PrepareOutputTarget(int width, int depth)
        {
            this.depth = depth;
            this.width = width;
            Clear();
        }
    }
}