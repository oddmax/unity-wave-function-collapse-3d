using UnityEngine;

namespace Core
{
    public class UncollapsedTileView : MonoBehaviour
    {
        [SerializeField]
        private Transform sphere;
        
        public void UpdateState(float entropyLevel)
        {
            sphere.localScale = new Vector3(entropyLevel, entropyLevel, entropyLevel);
        }
    }
}