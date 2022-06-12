using UnityEditor;
using UnityEngine;

namespace Core
{
    [InitializeOnLoad]
    public static class GeneratorSceneViewButtons {

        static GeneratorSceneViewButtons()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            var generator = Object.FindObjectOfType<WaveFunctionCollapseGenerator>();
            if (generator == null)
            {
                return;
            }
		
            Handles.BeginGUI();
		
            GUILayout.BeginArea(new Rect(sceneView.position.width - 190, sceneView.position.height - 110, 180, 100));
		
            if(GUILayout.Button("Generate Overlapping output")){
                generator.GenerateOverlappingOutput();
            }
            if(GUILayout.Button("Generate Simple tiled output")){
                generator.GenerateSimpleTiledOutput();
            }
            if(GUILayout.Button("Abort and Clear")){
                generator.Abort();
            }
		
            GUILayout.EndArea();
		
            Handles.EndGUI();
        }
    }
}