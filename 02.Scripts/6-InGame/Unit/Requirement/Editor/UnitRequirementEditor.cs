using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitRequirement))]
public class UnitRequirementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GUILayout.Space(10f);
        if (GUILayout.Button("Find ShortCuts"))
        {
            ((UnitRequirement)target).FindShorcut();
        }
    }
}
