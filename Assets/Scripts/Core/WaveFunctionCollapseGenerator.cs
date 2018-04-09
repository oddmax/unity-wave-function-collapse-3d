using Core.Data;
using Core.Data.OverlappingModel;
using Core.Data.SimpleTiledModel;
using Core.InputProviders;
using Core.Model;
using Core.Model.New;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class WaveFunctionCollapseGenerator : MonoBehaviour
    {
	    [SerializeField] 
	    private InputDataProvider dataProvider;

	    [SerializeField] 
	    private WaveFunctionCollapseRenderer renderer;
	    
	    [SerializeField] 
	    private int width;
	    
	    [SerializeField] 
	    private int height;
	    
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

	    private OvelappingModel2dWrapper overlappingModel;
	    private SimpleTiledMode3d simpleTiledModel;
	    private InputOverlappingData inputOverlappingData;
	    private Coroutine runningCoroutine;

	    public void GenerateOverlappingOutput()
	    {
		    inputOverlappingData = dataProvider.GetInputOverlappingData();
		    var modelParams = new OverlappingModelParams(width, height, depth, patternSize);
		    modelParams.PeriodicInput = periodicInput;
		    modelParams.PeriodicOutput = periodicOutput;
		    modelParams.Symmetry = symmetry;
		    modelParams.Ground = foundation;
		    
		    overlappingModel = new OvelappingModel2dWrapper(inputOverlappingData, modelParams);
		    renderer.Init(overlappingModel);
		    
		    runningCoroutine = StartCoroutine(overlappingModel.Model.RunViaEnumerator(0, iterations, OnResult, OnIteration));
	    }

	    public void GenerateSimpleTiledOutput()
	    {
		    var inputData = dataProvider.GetInputSimpleTiledData();
		    var modelParams = new SimpleTiledModelParams(width, height, depth, periodicOutput);
		    
		    simpleTiledModel = new SimpleTiledMode3d(inputData, modelParams);
		    renderer.Init(simpleTiledModel);
		    
		    runningCoroutine = StartCoroutine(simpleTiledModel.RunViaEnumerator(0, iterations, OnResult, OnIteration));
	    }
	    
	    private void OnIteration(bool[][] wave)
	    {
		    renderer.UpdateStates();
	    }

	    private void OnResult(bool result)
	    {
		    Debug.Log("Result is : " + result);
	    }

	    public void Abort()
	    {
		    StopCoroutine(runningCoroutine);
		    renderer.Clear();
	    }
    }
    
	#if UNITY_EDITOR
	[CustomEditor (typeof(WaveFunctionCollapseGenerator))]
	public class WaveFunctionCollapseGeneratorEditor : Editor {
		public override void OnInspectorGUI () {
			WaveFunctionCollapseGenerator generator = (WaveFunctionCollapseGenerator)target;
			if(GUILayout.Button("Generate Overlapping output")){
				generator.GenerateOverlappingOutput();
			}
			if(GUILayout.Button("Generate Simple tiled output")){
				generator.GenerateSimpleTiledOutput();
			}
			if(GUILayout.Button("Abort and Clear")){
				generator.Abort();
			}
			DrawDefaultInspector ();
		}
	}
	#endif
}