using Core.Data;
using Core.InputProviders;
using Core.Model;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class WaveFunctionCollapseGenerator : MonoBehaviour
    {
	    [SerializeField] 
	    private YuMEInputDataProvider dataProvider;

	    [SerializeField] 
	    private WaveFunctionCollapseRenderer renderer;
	    
	    [SerializeField] 
	    private int width;
	    
	    [SerializeField] 
	    private int depth;
	    
	    [SerializeField] 
	    private int patternSize;
	    
	    [SerializeField] 
	    private bool periodicInput = false;

	    [SerializeField] 
	    private bool periodicOutput = false;
	    
	    [SerializeField] 
	    private int symmetry = 1;
	    
	    [SerializeField] 
	    private int foundation = 0;
	    
	    [SerializeField] 
	    private int iterations = 0;
	    
	    public InputOverlappingData ExtractOverlappingData()
	    {
		    var inputOverlappingData = dataProvider.GetInputOverlappingData();
		    return inputOverlappingData;
	    }

	    public void ExtractSimpleTiledData()
	    {
		    throw new System.NotImplementedException();
	    }

	    public void GenerateOverlappingOutput()
	    {
		    var inputData = ExtractOverlappingData();
		    var modelParams = new OverlappingModelParams(width, depth, patternSize);
		    modelParams.PeriodicInput = periodicInput;
		    modelParams.PeriodicOutput = periodicOutput;
		    modelParams.Symmetry = symmetry;
		    modelParams.Ground = foundation;
		    
		    //var overlappingModel = new OverlappingModel(inputData, modelParams);

		    renderer.PrepareOutputTarget(width, depth);
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