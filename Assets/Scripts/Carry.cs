using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;

public class Carry : MonoBehaviour
{
    public Transform obj;
    private ViveRoleSetter setter;
    private Rigidbody rbd;
    public ControllerButton button = ControllerButton.Trigger;
    private bool locked = true;

    void Start()
    {
        setter = GetComponent<ViveRoleSetter>();
        rbd = GetComponent<Rigidbody>();
        
        obj.position = transform.position;
    }
    void Update()
    {
        if(ViveInput.GetPress(setter.viveRole, button) == true)
        {
            Destroy(obj.GetComponent<FixedJoint>());
           // obj.gameObject.GetComponent<FixedJoint>().breakForce = 40;
            //obj.gameObject.GetComponent<FixedJoint>().breakTorque = 40;
            obj.GetComponent<Rigidbody>().velocity = rbd.velocity;
        }
    }
    void LetGo()
    {
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.parent = null;
        locked = false;
    }
}
