using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    bool preview = false;

    public override void OnInspectorGUI()
    {
        CameraController cameraController = (CameraController)target;

        EditorGUILayout.LabelField((preview ? "(On) " : "(Off) "));

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Preview")) {
            preview = true;
        }

        if (GUILayout.Button("Clear")) {
            preview = false;
            cameraController.preview = preview;
        }
        EditorGUILayout.EndHorizontal();
        if (preview) {
            cameraController.preview = preview;
        }

        base.OnInspectorGUI();

    }

}
