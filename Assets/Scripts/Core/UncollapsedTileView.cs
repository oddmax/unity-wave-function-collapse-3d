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
            float entropy = (float)entropyLevel;
            if (entropy > 0)
            {
                sphere.gameObject.SetActive(true);
                sphere.localScale = new Vector3(entropy, entropy, entropy);
            }
            else
            {
                sphere.gameObject.SetActive(false);
            }
        }
    }
}