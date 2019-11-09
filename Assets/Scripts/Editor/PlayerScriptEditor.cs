using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerScript))]
public class PlayerScriptEditor : Editor
{
    bool showPosition = false;
    SerializedProperty cameraFloorOffset, vrRig, vrBody, cam, moveSpeed, runSpeed, enableCameraSnap;

    private void OnEnable()
    {
        cameraFloorOffset = serializedObject.FindProperty("cameraFloorOffset");

        vrRig = serializedObject.FindProperty("vrRig");
        vrBody = serializedObject.FindProperty("vrBody");

        cam = serializedObject.FindProperty("cam");

        moveSpeed = serializedObject.FindProperty("moveSpeed");
        runSpeed = serializedObject.FindProperty("runSpeed");

        enableCameraSnap = serializedObject.FindProperty("enableCameraSnap");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PlayerScript pScript = (PlayerScript)target;
        EditorGUILayout.LabelField("Main Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(enableCameraSnap, new GUIContent("Enable Camera Snap"));
        EditorGUILayout.PropertyField(moveSpeed, new GUIContent("Walk Speed"));
        EditorGUILayout.PropertyField(runSpeed, new GUIContent("Run Speed"));

        showPosition = EditorGUILayout.Foldout(showPosition, "Extra Settings");
        if(showPosition)
        {
            EditorGUILayout.PropertyField(vrRig, new GUIContent("VR Rig"));
            EditorGUILayout.PropertyField(vrBody, new GUIContent("VR Body"));
            EditorGUILayout.PropertyField(cameraFloorOffset, new GUIContent("Camera Floor Offset"));
        }
        

        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();
    }
}
