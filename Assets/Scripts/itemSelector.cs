using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

// Attach this to the hand
[RequireComponent(typeof(LineRenderer))]
public class ItemSelector : MonoBehaviour
{
    public float rotationSpeed, moveSpeed;
    private LineRenderer line;
    //public OVRPlayerController player;
    private Transform grabbedObject;
    private bool attachComponent = false;
    private enum RaycastStates { Idle, Raycast, GrabbedObject, ManipulateObject };
    private StateMachine<RaycastStates> fsm;

    private void Awake()
    {
        this.line = GetComponent<LineRenderer>();
        this.fsm = StateMachine<RaycastStates>.Initialize(this);
        this.line.widthMultiplier = 0.3f;
        this.fsm.ChangeState(RaycastStates.Idle);
    }

    private void Idle_Update()
    {
        this.line.widthMultiplier = 0;
        //this.line.SetPosition(1, this.transform.position);
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) || Input.GetKeyDown(KeyCode.Z))
        {
            fsm.ChangeState(RaycastStates.Raycast);
        }
    }

    private void Idle_Exit()
    {
        
    }

    private void Raycast_Enter()
    {
        if(this.grabbedObject != null){
            this.grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            this.grabbedObject.parent = null;
            this.grabbedObject = null;
        }
        //player.EnableRotation = true;
    }

    private void Raycast_Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) || Input.GetKeyDown(KeyCode.Z))
        {
            fsm.ChangeState(RaycastStates.Idle);
        }
        else
        {
            RaycastHit hit;
            this.line.SetPosition(0, transform.position);
            Debug.Log("instate Object");
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                5000, LayerMask.GetMask("Selected")))
            {
                Debug.Log("hit Object");
                this.line.SetPosition(1, hit.point);
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.C))
                {
                    //if(hit.transform.GetComponent<Renderer>() != null)
                    //{
                        this.grabbedObject = hit.transform;
                    /*}
                    else
                    {
                        this.grabbedObject = hit.transform.parent.transform;
                    }*/
                    Debug.Log(this.grabbedObject);
                    fsm.ChangeState(RaycastStates.GrabbedObject);
                }
            }
            else
            {
                Debug.Log("no hit Object");
                this.line.SetPosition(1, transform.TransformDirection(Vector3.forward) * 5000);
            }
        }
    }

    private void GrabbedObject_Enter()
    {
        // Set initial position
        this.grabbedObject.position = transform.position + transform.TransformDirection(Vector3.forward) * Vector3.Distance(transform.position, this.grabbedObject.transform.position);
        this.grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        this.grabbedObject.parent = transform;
    }

    private void GrabbedObject_Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.C))
        {
            fsm.ChangeState(RaycastStates.Raycast);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.F))
        {
            fsm.ChangeState(RaycastStates.ManipulateObject);
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.T)) //attach
        {
            if(this.grabbedObject.GetComponent<AttachCollision>().Collided != null)
                this.grabbedObject.gameObject.AddComponent<FixedJoint>().connectedBody = this.grabbedObject.GetComponent<AttachCollision>().Collided;
            
            //is.grabbedObject.GetComponent<FixedJoint>().connectedBody = 
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.Y)) //detach
        {

            if (this.grabbedObject.GetComponent<FixedJoint>() != null)
                Destroy(this.grabbedObject.GetComponent<FixedJoint>());
        }

        // Sets the line
        this.line.SetPosition(0, transform.position);
        if (this.grabbedObject != null)
        this.line.SetPosition(1, this.grabbedObject.position);
    }
    private void ManipulateObject_Enter()
    {
        //player.EnableRotation = false;
        //player.EnableLinearMovement = false;
    }

    private void ManipulateObject_Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.C))
        {
            fsm.ChangeState(RaycastStates.Raycast);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.F))
        {
            fsm.ChangeState(RaycastStates.GrabbedObject);
        }
        // Sets the line
        this.line.SetPosition(0, transform.position);
        this.line.SetPosition(1, this.grabbedObject.position);
        // Position
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp) || Input.GetKey(KeyCode.UpArrow))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.up) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown) || Input.GetKey(KeyCode.DownArrow))
            if (Vector3.Distance(transform.position, this.grabbedObject.transform.position) > 2)
                this.grabbedObject.transform.position += transform.TransformDirection(Vector3.down) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) || Input.GetKey(KeyCode.LeftArrow))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.left) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight) || Input.GetKey(KeyCode.RightArrow))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.right) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.Four) || Input.GetKey(KeyCode.Q))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.forward) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.Three) || Input.GetKey(KeyCode.E))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.back) * moveSpeed;

        // Rotation
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp) || Input.GetKeyDown(KeyCode.I))
            //this.grabbedObject.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
            this.grabbedObject.Rotate(Vector3.right * 90);
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKeyDown(KeyCode.K))
            //this.grabbedObject.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
            this.grabbedObject.Rotate(Vector3.left * 90);
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKeyDown(KeyCode.J))
            //this.grabbedObject.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
            this.grabbedObject.Rotate(Vector3.up * 90);
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight) || Input.GetKeyDown(KeyCode.L))
            //this.grabbedObject.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
            this.grabbedObject.Rotate(Vector3.down * 90);
    }

    private void ManipulateObject_Exit()
    {
        //player.EnableRotation = true;
        //player.EnableLinearMovement = true;
    }
}