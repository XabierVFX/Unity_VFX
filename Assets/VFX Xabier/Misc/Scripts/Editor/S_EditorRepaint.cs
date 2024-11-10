using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class S_EditorRepaint
{
    static S_EditorRepaint()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
        if (Application.isPlaying == false)
        {
            SceneView.RepaintAll();
        }
    }
}
