/*using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class RunEditorInBackground : EditorWindow
{
    static RunEditorInBackground()
    {
        EditorApplication.playModeStateChanged += Running;
    }

    private static void Running(PlayModeStateChange obj)
    {
        if (EditorApplication.isPlaying)
            Application.runInBackground = true;
    }
}*/