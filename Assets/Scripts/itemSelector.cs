using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

// Attach this to the hand
[RequireComponent(typeof(LineRenderer))]
public class ItemSelector : MonoBehaviour {
    public float rotationSpeed;
    private LineRenderer line;
    public OVRPlayerController player;
    private Transform grabbedObject;

    private enum RaycastStates { Idle, Raycast, GrabbedObject };
    private StateMachine<RaycastStates> fsm;

    private void Awake()
    {
        this.line = GetComponent<LineRenderer>();
        this.fsm = StateMachine<RaycastStates>.Initialize(this);
        this.fsm.ChangeState(RaycastStates.Idle);
    }
    
    private void Idle_Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            fsm.ChangeState(RaycastStates.Raycast);
        }
    }

    private void Raycast_Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            fsm.ChangeState(RaycastStates.Idle);
        }
        else
        {
            RaycastHit hit;
            this.line.SetPosition(0, transform.position);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                5000, LayerMask.GetMask("selected")))
            {
                this.line.SetPosition(1, hit.point);
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    this.grabbedObject = hit.transform;
                    fsm.ChangeState(RaycastStates.GrabbedObject);
                }
            }
            else
            {
                this.line.SetPosition(1, transform.TransformDirection(Vector3.forward) * 5000);
            }
        }
    }

    private void GrabbedObject_Enter()
    {
        this.grabbedObject.parent = transform;
        player.EnableRotation = false;
    }

    private void GrabbedObject_Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            fsm.ChangeState(RaycastStates.Raycast);
        }
        // TODO: Position
        this.grabbedObject.position = transform.position + transform.TransformDirection(Vector3.forward) * 
            Vector3.Distance(transform.position, this.grabbedObject.position);
        // Rotation
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp) || Input.GetKey(KeyCode.I))
            this.grabbedObject.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKey(KeyCode.K))
            this.grabbedObject.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKey(KeyCode.J))
            this.grabbedObject.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) || Input.GetKey(KeyCode.L))
            this.grabbedObject.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
        // Sets the line
        this.line.SetPosition(0, transform.position);
        this.line.SetPosition(1, this.grabbedObject.position);
    }

    private void GrabbedObject_Exit()
    {
        this.grabbedObject.parent = null;
        player.EnableRotation = true;
        this.grabbedObject = null;
    }
}

// Update is called once per frame
/*
void Update () {
    line.SetPosition(0, transform.position);
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayerMask.GetMask("Selected")))
    {
        line.SetPosition(1, hit.point);
        if (Input.GetKeyDown(KeyCode.C) || OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && !grabbedItem)
        {
            hit.transform.position = transform.position + transform.TransformDirection(Vector3.forward) * Vector3.Distance(transform.position, hit.transform.position);
            hit.transform.parent = transform;
            grabbedItem = true;
            player.EnableRotation = false;
        }
        else
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && grabbedItem)
            {
                hit.transform.parent = null;
                grabbedItem = false;
                player.EnableRotation = true;
            }
        }

        if (grabbedItem)
        {
            var d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f)
            {
                hit.transform.position += transform.TransformDirection(Vector3.forward);
            }
            else if (d < 0f)
            {
                if(Vector3.Distance(transform.position, hit.transform.position) > 2)
                    hit.transform.position -= transform.TransformDirection(Vector3.forward);
            }

            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp) || Input.GetKey(KeyCode.I))
                hit.transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKey(KeyCode.K))
                hit.transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKey(KeyCode.J))
                hit.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
            if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) || Input.GetKey(KeyCode.L))
                hit.transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
        }
    }
    else
    {
        line.SetPosition(1, transform.TransformDirection(Vector3.forward) * 5000);
    }
}*/