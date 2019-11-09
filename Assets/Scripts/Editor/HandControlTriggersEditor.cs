using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HandControlTriggers))]
public class HandControlTriggersEditor : Editor
{
    SerializedProperty hand, 
    leftHolster, rightHolster, 
    leftGrip_Input, rightGrip_Input, 
    left_CanGrab, right_CanGrab, 
    leftCarrying, rightCarrying, 
    leftEnter, rightEnter, 
    leftObj, rightObj, 
    leftObjTag, rightObjTag, 
    offsetAmount, hitSphere, layers;

    private void OnEnable()
    {
        hand = serializedObject.FindProperty("hand");
        offsetAmount = serializedObject.FindProperty("offsetAmount");
        hitSphere = serializedObject.FindProperty("hitSphere");
        layers = serializedObject.FindProperty("layers");

        leftHolster = serializedObject.FindProperty("leftHolster");
        rightHolster = serializedObject.FindProperty("rightHolster");

        leftGrip_Input = serializedObject.FindProperty("leftGrip_Input");
        rightGrip_Input = serializedObject.FindProperty("rightGrip_Input");

        left_CanGrab = serializedObject.FindProperty("left_CanGrab");
        right_CanGrab = serializedObject.FindProperty("right_CanGrab");

        leftEnter = serializedObject.FindProperty("leftEnter");
        rightEnter = serializedObject.FindProperty("rightEnter");

        leftCarrying = serializedObject.FindProperty("leftCarrying");
        rightCarrying = serializedObject.FindProperty("rightCarrying");
        
        leftObj = serializedObject.FindProperty("leftObj");
        rightObj = serializedObject.FindProperty("rightObj");

        leftObjTag = serializedObject.FindProperty("leftObjTag");
        rightObjTag = serializedObject.FindProperty("rightObjTag");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        HandControlTriggers h = (HandControlTriggers)target;
        EditorGUILayout.PropertyField(hand);
        EditorGUILayout.PropertyField(offsetAmount);
        EditorGUILayout.PropertyField(hitSphere);
        EditorGUILayout.PropertyField(layers);

        if (h.hand == HandControlTriggers.Hands.left)
        {
            EditorGUILayout.PropertyField(leftHolster, new GUIContent("Left Holster"));
            EditorGUILayout.PropertyField(leftGrip_Input, new GUIContent("Left Grip Input"));
            EditorGUILayout.PropertyField(left_CanGrab, new GUIContent("Left Can Grab"));
            EditorGUILayout.PropertyField(leftEnter, new GUIContent("Left Entered"));
            EditorGUILayout.PropertyField(leftCarrying, new GUIContent("Left Carrying"));
            EditorGUILayout.PropertyField(leftObj, new GUIContent("Left Object"));
            EditorGUILayout.PropertyField(leftObjTag, new GUIContent("Left Object Tag"));
        }
        else
        {
            EditorGUILayout.PropertyField(rightHolster, new GUIContent("Right Holster"));
            //EditorGUILayout.PropertyField(rightGrip_Input, new GUIContent("Right Grip Input"));
            //EditorGUILayout.PropertyField(right_CanGrab, new GUIContent("Right Can Grab"));
            //EditorGUILayout.PropertyField(rightEnter, new GUIContent("Right Entered"));
            //EditorGUILayout.PropertyField(rightCarrying, new GUIContent("Right Carrying"));
            //EditorGUILayout.PropertyField(rightObj, new GUIContent("Right Object"));
            //EditorGUILayout.PropertyField(rightObjTag, new GUIContent("Right Object Tag"));
        }
        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();
    }
}
