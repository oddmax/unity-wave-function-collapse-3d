using Core.Data;
using UnityEngine;

namespace Core
{
    public class YuMEInputDataProvider : MonoBehaviour, IInputDataProvider
    {
        [SerializeField] 
        private YuME_GizmoGrid grid;
        
        private GameObject[] levelsList;
        
        public InputOverlappingData GetInputOverlappingData()
        {
            var width = grid.gridWidth;
            var depth = grid.gridDepth;

            var inputData = new InputOverlappingData(width, depth);
            
            foreach (var levelContainer in levelsList)
            {
                foreach (Transform child in levelContainer.transform)
                {
                    var x = Mathf.RoundToInt(child.localPosition.x);
                    var y = Mathf.RoundToInt(child.localPosition.z);
                    int rotation = (int)((360 - child.localEulerAngles.z)/90);
                    if (rotation == 4) {rotation = 0;};
                    
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