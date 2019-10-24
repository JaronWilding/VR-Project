using HTC.UnityPlugin.PoseTracker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    
    private Draggable drag;
    
    void Start()
    {
        drag = GetComponent<Draggable>();
    }

}
