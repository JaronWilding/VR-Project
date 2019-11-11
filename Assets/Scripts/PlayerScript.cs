using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    
    [SerializeField] private Transform cameraFloorOffset = null;
    [SerializeField] private Transform vrRig = null;
    [SerializeField] private Transform vrBody = null;
    [SerializeField] private Camera cam = null;

    

    //Serialized private variables
    [SerializeField, Range(0f, 10f)] private float moveSpeed = 2.0f;
    [SerializeField, Range(0f, 10f)] private float runSpeed = 4.0f;

    [SerializeField] private bool enableCameraSnap = true;

    //Private global variables
    private float leftX_Input;
    private float leftZ_Input;
    private float rightX_Input;
    private float ground;
    
    //private float 

    private bool inputMovement;
    private bool inputRotation;
    private float addedRotation;
    public bool isFloating;

    private Quaternion vrBodyRotation;
    
    private CharacterController charCon = null;

    private void Start()
    {
        charCon = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    private void Update()
    {
        leftX_Input = Input.GetAxis("LHorizontal");
        leftZ_Input = Input.GetAxis("LVertical");
        rightX_Input = Input.GetAxis("RHorizontal");

        inputMovement = leftX_Input <= -0.2f || leftX_Input >= 0.2f || leftZ_Input <= -0.2f || leftZ_Input >= 0.2f ? true : false;

        
        //Move the camera first.
        MoveCam();
        //Movement of the character
        //MoveChar();
        //Secondary camera movement - using inputs
        //ControllerMoveCamera();

        GroundCheck();
    }


    private IEnumerator ResetInputRotation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        inputRotation = false;
    }
    

    private void MoveCam()
    {
        Vector3 hmdWorld = cam.transform.position;
        float hmdRotation = cam.transform.rotation.eulerAngles.y;
        float hmdDown = cam.transform.rotation.eulerAngles.x;


        if (!inputMovement)
        {
            ground = transform.position.y;
        }
        
        //Set the camera floor offset to follow properly.
        cameraFloorOffset.localPosition = new Vector3(-cam.transform.localPosition.x, cameraFloorOffset.localPosition.y, -cam.transform.localPosition.z);

        //Set current position to the camera pos.
        transform.position = new Vector3(hmdWorld.x, ground, hmdWorld.z);

        //Set the vrBody pos properly | Set the rotation of our holsters to the rotation of the camera.
        vrBody.position = hmdWorld;
        

        //Rotate main body in the same place as the camera.
        transform.rotation = Quaternion.Euler(new Vector3(0f, hmdRotation, 0f));
        //Rotate the camera's parent back so it doesn't spin around
        vrRig.transform.localRotation = Quaternion.Euler(new Vector3(0f, (hmdRotation + addedRotation) * -1f, 0f));
        
        //If head looks down towards own body (holsters)
        //if ((hmdDown < 110 && hmdDown > 40) == false)
        //{
            //If not looking down, set proper rotation to the same as the camera
            //Save camera's rotation into a global variable
        //    vrBodyRotation = Quaternion.Euler(new Vector3(0f, hmdRotation + addedRotation, 0f));
        //}

        //One line if statement, may be better \ faster?
        vrBodyRotation = (hmdDown < 110 &&  hmdDown > 40) ? vrBodyRotation : Quaternion.Euler(new Vector3(0f, hmdRotation, 0f));

        vrBody.transform.localRotation = vrBodyRotation;
    }

    private void ControllerMoveCamera()
    {
        if (inputMovement == false && inputRotation == false)
        
            if(enableCameraSnap == true)
            {
                if (rightX_Input <= -0.2f)
                {
                    inputRotation = true;
                    addedRotation += 30f;
                    StartCoroutine(ResetInputRotation(0.35f));
                }
                else if (rightX_Input >= 0.2f)
                {
                    inputRotation = true;
                    addedRotation -= 30f;
                    StartCoroutine(ResetInputRotation(0.35f));
                }
            }
            else
            {
                if (rightX_Input <= -0.2f || rightX_Input >= 0.2f)
                {
                    addedRotation -= rightX_Input;
                }
            }
           
        else if (inputMovement == true)
        {
            if (rightX_Input <= -0.2f || rightX_Input >= 0.2f)
            {
                addedRotation -= rightX_Input;
            }
        }
    }

    private void MoveChar()
    {
        if (inputMovement)
        {
            float currentSpeed;
            if (Input.GetButton("LThumbPress"))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
            //Get our inputs, and add our movespeed + deltaTime.
            Vector3 moveVals = new Vector3(leftX_Input * Time.deltaTime * currentSpeed, -5f * Time.deltaTime, leftZ_Input * Time.deltaTime * currentSpeed);
            //Transform our movement from local to world space.
            moveVals = transform.TransformDirection(moveVals);
            charCon.Move(moveVals);
        }
    }

    public void GroundCheck()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 100f);
        if (inputMovement == false && hitInfo.distance < 0.04f)
        {
            charCon.Move(transform.TransformDirection(new Vector3(0f, -5f * Time.deltaTime, 0f)));
        }
        
    }

}
