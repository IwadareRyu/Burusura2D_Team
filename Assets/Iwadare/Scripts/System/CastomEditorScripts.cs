using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour),true)]
[CanEditMultipleObjects]
public class CastomEditorScripts : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
