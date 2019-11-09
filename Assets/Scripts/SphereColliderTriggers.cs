using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliderTriggers : MonoBehaviour
{
    private HandControlTriggers handControllers;

    private void Start() 
    {
        Debug.Log("Started");
        handControllers = GetComponentInParent<HandControlTriggers>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col != null)
        {
            Debug.Log("Entered");
            handControllers.SetVariables(col);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        handControllers.RevertVariables(col);
    }

}
