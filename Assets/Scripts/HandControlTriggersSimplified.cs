using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandControlTriggersSimplified : MonoBehaviour
{
    //Define hand type for inputs
    public enum Hands { left, right }
    [SerializeField] public Hands hand = Hands.left;

    //Inputs variables!
    [SerializeField] private float leftTrigger_Input;
    [SerializeField] private float rightTrigger_Input;

    //Extra stuff for picking up objects
    [SerializeField] private Vector3 offsetAmount;
    [SerializeField] private LayerMask layers;
    [SerializeField] private LineRenderer lr;
    
    [SerializeField] private bool selectedObjectBool;
    [SerializeField] private bool alreadyCarrying;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private string selectedObjectTag;

    [SerializeField] private List<GameObject> selectedObjects;

    [SerializeField] private bool Trigger_Entered;


    private bool wasSelected;

    private bool left_Grabbing;
    private bool left_Throwing;

    private void Start()
    {
        selectedObjects = new List<GameObject>();
    }

    void Update()
    {
        FarObject();
        if (hand == Hands.left)
        {
            leftTrigger_Input = Input.GetAxis("LTrigger");
            Trigger_Entered = leftTrigger_Input <= -0.1f ? true : false;
        }
        else if(hand == Hands.right)
        {
            rightTrigger_Input = Input.GetAxis("RTrigger");
            Trigger_Entered = rightTrigger_Input <= -0.1f ? true : false;
        }


        if (Trigger_Entered)
        {
            if (alreadyCarrying == false)
            {
                //Pick up
                if (selectedObjectBool == true && selectedObject != null && selectedObjectTag == "Free Grab" && selectedObject.GetComponent<GrabbableObject>().isThrown == false)
                {
                    Grab(selectedObject, transform.parent, ref alreadyCarrying);
                }
            }
        }
        else
        {
            if (alreadyCarrying == true)
            {
                Release(selectedObject, transform.parent, GetComponent<Rigidbody>(), ref alreadyCarrying);
            }
        }
               
           


    }

    private void FarObject()
    {

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Vector3 pos;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layers))
        {
            if (hit.collider.tag == "Free Grab")
            {
                selectedObjects.Add(hit.collider.gameObject);
            }
            else if (alreadyCarrying == false)
            {
                selectedObjects.Clear();
            }
            pos = hit.point;
        }
        else
        {
            pos = transform.forward * 100f;
        }

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, pos);

        if(selectedObjects.Count > 0)
        {
            wasSelected = true;
            selectedObjectBool = true;
            selectedObject = selectedObjects[0];
            selectedObjectTag = selectedObject.tag;
            selectedObject.GetComponent<GrabbableObject>().isSelected = true;
        }
        else if(wasSelected)
        {
            wasSelected = false;
            selectedObjectBool = false;
            selectedObject.GetComponent<GrabbableObject>().isSelected = false;
            selectedObject = null;
            selectedObjectTag = null;
        }
        
    }

    
    private void Grab(GameObject _obj, Transform _hand, ref bool _carry)
    {
        _carry = true;
        _obj.GetComponent<Rigidbody>().isKinematic = true;
        _obj.transform.position = _hand.position;
        _obj.transform.rotation = _hand.rotation * Quaternion.Euler(offsetAmount);
        _obj.transform.SetParent(_hand);
    }
    private void Release(GameObject _obj, Transform _hand, Rigidbody _rbd, ref bool _carry)
    {

        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        _carry = false;
        _obj.transform.SetParent(null);
        _obj.GetComponent<Rigidbody>().isKinematic = false;

        if(hand == Hands.left)
        {
            foreach(XRNodeState nodeState in nodeStates)
            {
                if(nodeState.nodeType == XRNode.LeftHand)
                {
                    Vector3 vel = Vector3.zero;
                    Vector3 angVel = Vector3.zero;

                    nodeState.TryGetVelocity(out vel);
                    nodeState.TryGetAngularVelocity(out angVel);

                    _obj.GetComponent<Rigidbody>().velocity = vel * 3f;
                    _obj.GetComponent<Rigidbody>().angularVelocity = angVel * 0.75f;
                    _obj.GetComponent<Rigidbody>().maxAngularVelocity = _obj.GetComponent<Rigidbody>().angularVelocity.magnitude;
                    _obj.GetComponent<GrabbableObject>().isThrown = true;
                    _obj.GetComponent<GrabbableObject>().Deselect();
                    break;
                }
            }
        }
        else if(hand == Hands.right)
        {
            foreach(XRNodeState nodeState in nodeStates)
            {
                if(nodeState.nodeType == XRNode.RightHand)
                {
                    Vector3 vel = Vector3.zero;
                    Vector3 angVel = Vector3.zero;

                    nodeState.TryGetVelocity(out vel);
                    nodeState.TryGetAngularVelocity(out angVel);

                    _obj.GetComponent<Rigidbody>().velocity = vel * 3f;
                    _obj.GetComponent<Rigidbody>().angularVelocity = angVel * 0.75f;
                    _obj.GetComponent<Rigidbody>().maxAngularVelocity = _obj.GetComponent<Rigidbody>().angularVelocity.magnitude;
                    _obj.GetComponent<GrabbableObject>().isThrown = true;
                    _obj.GetComponent<GrabbableObject>().Deselect();
                    
                    break;
                }
            }

        }
    }
    

    
}
