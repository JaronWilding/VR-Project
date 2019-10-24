using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ScoreGiver : MonoBehaviour
{
    [SerializeField] private UnityEvent j_OnAxeCollide;

    private Start()
    {
        j_OnAxeCollide.AddListener();
    }
    private void OnTriggerEnter(Collider col) 
    {
        Debug.Log("Entered");
    }
}
