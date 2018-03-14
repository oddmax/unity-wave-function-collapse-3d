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
                    inputData.SetTile(x, y, child.gameObject.name, rotation);
                } 
            }

            return inputData;
        }

        public InputSimpleTiledModelData GetInputSimpleTiledData()
        {
            throw new System.NotImplementedException();
        }
    }
}