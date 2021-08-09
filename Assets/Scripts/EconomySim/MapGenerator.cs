using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tester))]
// Custom Editor for testing generating maps
public class MapGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        Tester tester = (Tester)target;

        DrawDefaultInspector();
        if (GUILayout.Button("Generate"))
        {
            tester.GenerateMap();
        }
    }
}
