using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Renumbrance;

[CustomEditor(typeof(TestingTool))]
[CanEditMultipleObjects]
public class TestingToolCustomInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestingTool tool = (TestingTool)target;

        if (GUILayout.Button("Test")) //10
        {
            tool.Test();
        }
    }

}
