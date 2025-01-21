using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleListSO))]
public class ObstacleListSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Set Pefabs"))
        {
            ((ObstacleListSO)target).SetPrefabs();
        }
        
        GUILayout.Space(10);
        
        DrawDefaultInspector();
    }
}
