using Core.InputProviders;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class WaveFunctionCollapseGenerator : MonoBehaviour
    {
	    [SerializeField] 
	    private YuMEInputDataProvider dataProvider;
	    
	    public void ExtractOverlappingData()
	    {
		    var inputOverlappingData = dataProvider.GetInputOverlappingData();
		   	Debug.Log(inputOverlappingData);
	    }

	    public void ExtractSimpleTiledData()
	    {
		    throw new System.NotImplementedException();
	    }

	    public void GenerateOverlappingOutput()
	    {
		    throw new System.NotImplementedException();
	    }
    }
    
	#if UNITY_EDITOR
	[CustomEditor (typeof(WaveFunctionCollapseGenerator))]
	public class WaveFunctionCollapseGeneratorEditor : Editor {
		public override void OnInspectorGUI () {
			WaveFunctionCollapseGenerator generator = (WaveFunctionCollapseGenerator)target;
			if(GUILayout.Button("Extract Overlapping data")){
				generator.ExtractOverlappingData();
			}
			if(GUILayout.Button("Extract Simple tiled data")){
				generator.ExtractSimpleTiledData();
			}
			if(GUILayout.Button("Generate Overlapping output")){
				generator.GenerateOverlappingOutput();
			}
			DrawDefaultInspector ();
		}
	}
	#endif
}