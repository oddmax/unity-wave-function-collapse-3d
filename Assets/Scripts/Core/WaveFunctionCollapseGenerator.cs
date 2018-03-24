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

	    private OverlappingModel overlappingModel;
	    private InputOverlappingData inputOverlappingData;

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
		    inputOverlappingData = ExtractOverlappingData();
		    var modelParams = new OverlappingModelParams(width, depth, patternSize);
		    modelParams.PeriodicInput = periodicInput;
		    modelParams.PeriodicOutput = periodicOutput;
		    modelParams.Symmetry = symmetry;
		    modelParams.Ground = foundation;
		    
		    renderer.PrepareOutputTarget(width, depth);
		    
		    overlappingModel = new OverlappingModel(inputOverlappingData, modelParams);
		    StartCoroutine(overlappingModel.RunViaEnumerator(0, iterations, OnResult, OnIteration));
	    }
	    
	    private void OnIteration(bool[][] wave)
	    {
		    int FMY = inputOverlappingData.Depth;
		    int FMX = inputOverlappingData.Width;
		    for (int i = 0; i < wave.Length; i++)
		    {
			    int contributors = 0, r = 0, g = 0, b = 0;
			    int x = i % FMY, y = i / FMX;
			    
			    for (int t = 0; t < T; t++)
			    {
				    if (wave[i][t])
				    {
					    observed[i] = t;
					    break;
				    }
			    }
		    }
	    }

	    private void OnResult(bool result)
	    {
		    Debug.Log("Result is : " + result);
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