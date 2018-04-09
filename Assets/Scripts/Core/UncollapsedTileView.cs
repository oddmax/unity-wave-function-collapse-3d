using System;
using UnityEngine;

namespace Core
{
    public class UncollapsedTileView : MonoBehaviour
    {
        [SerializeField]
        private Transform sphere;
        
        public void UpdateState(double entropyLevel)
        {
            float entropy = 1f - (float)entropyLevel;
            if (entropy > 0)
            {
                sphere.localScale = new Vector3(entropy, entropy, entropy);
            }
        }
    }
}