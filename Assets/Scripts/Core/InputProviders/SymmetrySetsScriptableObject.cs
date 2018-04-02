using Core.Data.SimpleTiledModel;
using Core.Helpers;
using UnityEditor;
using UnityEngine;

namespace Core.InputProviders
{
    public class SymmetrySetsScriptableObject : ScriptableObject
    {
        [SerializeField] 
        public GameObject[] SymmetryX;
        
        [SerializeField] 
        public GameObject[] SymmetryL;
        
        [SerializeField] 
        public GameObject[] SymmetryT;
        
        [SerializeField] 
        public GameObject[] SymmetryI;
        
        [SerializeField] 
        public GameObject[] SymmetrySlash;
        
        [MenuItem("Assets/Create/SymmetrySetsScriptableObject")]
        public static void CreateAsset ()
        {
            ScriptableObjectUtility.CreateAsset<SymmetrySetsScriptableObject>();
        }

        public SymmetryType GetSymmetryByTileName(string name)
        {
            if (Find(SymmetryX, name)) return SymmetryType.X;
            if (Find(SymmetryL, name)) return SymmetryType.L;
            if (Find(SymmetryT, name)) return SymmetryType.T;
            if (Find(SymmetryI, name)) return SymmetryType.I;
            if (Find(SymmetrySlash, name)) return SymmetryType.Slash;

            return SymmetryType.X;
        }

        private bool Find(GameObject[] set, string name)
        {
            foreach (var gameObject in set)
            {
                if (gameObject.name == name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}