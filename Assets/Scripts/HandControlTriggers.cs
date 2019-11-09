using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandControlTriggers : MonoBehaviour
{
    public enum Hands { left, right }
    [SerializeField] public Hands hand = Hands.left;

    [SerializeField] private Transform leftHolster;
    [SerializeField] private Transform rightHolster;
    [SerializeField] private GameObject hitSphere;
    private GameObject hitSphereInstance;

    [SerializeField] private Vector3 offsetAmount;
    [SerializeField] private LayerMask layers;
    

    [SerializeField] private bool leftEnter;
    [SerializeField] private bool leftCarrying;
    [SerializeField] private GameObject leftObj;
    [SerializeField] private string leftObjTag;

    [SerializeField] private bool rightEnter;
    [SerializeField] private bool rightCarrying;
    [SerializeField] private GameObject rightObj;
    [SerializeField] private string rightObjTag;

    [SerializeField] private float leftGrip_Input;
    [SerializeField] private float rightGrip_Input;
    [SerializeField] private bool left_CanGrab;
    [SerializeField] private bool right_CanGrab;

    private bool left_Grabbing;
    private bool left_Throwing;
    private bool right_Grabbing;
    private bool right_Throwing;




    void Update()
    {
        if(!leftCarrying || !rightCarrying)
        {
            FarObject();
        }
        if (hand == Hands.left)
        {
            if(Input.GetKeyDown(KeyCode.JoystickButton2)) // X
            {
                if (leftEnter == true && leftObj != null)
                {
                    if (leftObjTag == "Snap Toggle")
                    {
                        SnapToggle(leftObj, ref leftCarrying, transform.parent.parent);
                    }
                }
            }
            if(Input.GetKeyDown(KeyCode.JoystickButton3)) // Y
            {
                if (leftEnter == true && leftObj != null)
                {
                    if (leftObjTag == "Snap Toggle")
                    {
                        Holster(leftObj, ref leftCarrying, transform.parent.parent, leftHolster);
                    }
                }
            }

            leftGrip_Input = Input.GetAxis("LTrigger");
            left_CanGrab = leftGrip_Input <= -0.1f ? true : false;

            if (leftEnter == true && leftObj != null && leftObjTag == "Free Grab")
            {
                if (left_CanGrab == true)
                {
                    Grab(leftObj, transform.parent.parent, ref leftCarrying);
                    Debug.Log("GRAB");
                }
                else if(left_CanGrab == false && leftCarrying == true)
                {
                    Release(leftObj, transform.parent.parent, GetComponent<Rigidbody>(),  ref leftCarrying);
                }
            }

        }
        else if(hand == Hands.right)
        {
            if(Input.GetKeyDown(KeyCode.JoystickButton0)) // A
            {
                if (rightEnter == true && rightObj != null)
                {
                    if (rightObjTag == "Snap Toggle")
                    {
                        SnapToggle(rightObj, ref rightCarrying, transform.parent.parent);
                    }
                }
            }
            if(Input.GetKeyDown(KeyCode.JoystickButton1)) // B
            {
                if (rightEnter == true && rightObj != null)
                {
                    if (rightObjTag == "Snap Toggle")
                    {
                        Holster(rightObj, ref rightCarrying, transform.parent.parent, rightHolster);
                    }
                }
            }
           
            rightGrip_Input = Input.GetAxis("RTrigger");
            right_CanGrab = rightGrip_Input <= -0.1f ? true : false;

            if (rightEnter == true && rightObj != null && rightObjTag == "Free Grab")
            {
                if (right_CanGrab == true)
                {
                    Grab(rightObj, transform.parent.parent, ref rightCarrying);
                }
                else if(right_CanGrab == false && rightCarrying == true)
                {
                    Release(rightObj, transform.parent.parent, GetComponent<Rigidbody>(),  ref rightCarrying);
                }
            }
        }

        
    }

    private void FarObject()
    {
        if(hitSphereInstance == null)
        {
            hitSphereInstance = Instantiate(hitSphere, transform);
        }
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, layers);
        hitSphereInstance.transform.position = hit.point;
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
                    _obj.GetComponent<Rigidbody>().angularVelocity = angVel * 0.25f;
                    _obj.GetComponent<Rigidbody>().maxAngularVelocity = _obj.GetComponent<Rigidbody>().angularVelocity.magnitude;
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

                    _obj.GetComponent<Rigidbody>().velocity = vel * 2f;
                    _obj.GetComponent<Rigidbody>().angularVelocity = angVel * 0.25f;
                    _obj.GetComponent<Rigidbody>().maxAngularVelocity = _obj.GetComponent<Rigidbody>().angularVelocity.magnitude;
                    break;
                }
            }

        }

        
        //_obj.GetComponent<Rigidbody>().AddForce(transform.InverseTransformDirection(_rbd.velocity) * 5f);
    }


    private void SnapToggle(GameObject _obj, ref bool _carrying, Transform _hand)
    {
        if (_carrying == false)
        {
            _carrying = true;
            _obj.GetComponent<Rigidbody>().isKinematic = true;
            _obj.transform.position = _hand.position;
            _obj.transform.rotation = _hand.rotation;
            _obj.transform.SetParent(_hand);
        }
        else
        {
            _carrying = false;
            _obj.GetComponent<Rigidbody>().isKinematic = false;
            _obj.transform.SetParent(null);
        }
    }

    private void Holster(GameObject _obj, ref bool _carrying, Transform _hand, Transform _holster)
    {
        if (_carrying == false)
        {
            _carrying = true;
            _obj.GetComponent<Rigidbody>().isKinematic = true;
            _obj.transform.position = _hand.position;
            _obj.transform.rotation = _hand.rotation;
            _obj.transform.SetParent(_hand);
        }
        else
        {
            _carrying = false;
            _obj.transform.position = _holster.position;
            _obj.transform.rotation = _holster.rotation;
            _obj.transform.SetParent(_holster);
        }
    }
    

    private void OnTriggerEnter(Collider col)
    {
        if(col != null)
        {
            SetVariables(col);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        RevertVariables(col);
    }

    public void SetVariables(Collider col)
    {
        
        if (hand == Hands.left && leftObj == null && leftEnter == false)
        {
            leftEnter = true;
            leftObj = col.gameObject;
            leftObjTag = col.tag;
        }
        else if(hand == Hands.right && rightObj == null && rightEnter == false)
        {
            rightEnter = true;
            rightObj = col.gameObject;
            rightObjTag = col.tag;
        }
    }

    public void RevertVariables(Collider col)
    {
        if (hand == Hands.left)
        {
            leftEnter = false;
            leftObj = null;
            leftObjTag = null;
        }
        else
        {
            rightEnter = false;
            rightObj = null;
            rightObjTag = null;
        }
    }

    
}
