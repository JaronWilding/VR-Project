using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    [SerializeField] public MeshRenderer axeHead_Mesh;
    [SerializeField] public Material axeHead_Mat;
    [SerializeField] public MeshRenderer axeBody_Mesh;
    [SerializeField] public Material axeBody_Mat;
    [SerializeField] public Material selected_Mat;

    [SerializeField] public bool isSelected;
    [SerializeField] public bool isThrown;

    private Rigidbody rbd;

    private Material[] axeHead_Mats;
    private Material[] axeBody_Mats;

    private Material[] axeHead_Mats_init;
    private Material[] axeBody_Mats_init;

    private void Start()
    {
        axeHead_Mats = new Material[] { axeHead_Mat, selected_Mat };
        axeBody_Mats = new Material[] { axeBody_Mat, selected_Mat };

        axeHead_Mats_init = new Material[] { axeHead_Mat };
        axeBody_Mats_init = new Material[] { axeBody_Mat };

        rbd = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        if (isThrown == false)
        {
            if (isSelected)
            {
                Select();

            }
            else
            {
                Deselect();
            }
        }
    }
    public void Select()
    {
        axeHead_Mesh.materials = axeHead_Mats;
        axeBody_Mesh.materials = axeBody_Mats;
    }
    public void Deselect()
    {
        axeHead_Mesh.materials = axeHead_Mats_init;
        axeBody_Mesh.materials = axeBody_Mats_init;
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.collider != null && col.collider.tag != "Hands" && isThrown == true)
        {
            rbd.isKinematic = true;
            rbd.velocity = Vector3.zero;
            rbd.angularVelocity = Vector3.zero;
            isThrown = false;
        }
    }
}
