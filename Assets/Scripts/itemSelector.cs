﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

// Attach this to the hand
[RequireComponent(typeof(LineRenderer))]
public class ItemSelector : MonoBehaviour {
    public float rotationSpeed, moveSpeed;
    private LineRenderer line;
    public OVRPlayerController player;
    private Transform grabbedObject;

    private enum RaycastStates { Idle, Raycast, GrabbedObject, manipulateObject };
    private StateMachine<RaycastStates> fsm;

    private void Awake()
    {
        this.line = GetComponent<LineRenderer>();
        this.fsm = StateMachine<RaycastStates>.Initialize(this);
        this.fsm.ChangeState(RaycastStates.Idle);
    }

    private void Idle_Update()
    {
        this.line.widthMultiplier = 0;
        //this.line.SetPosition(1, this.transform.position);
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            fsm.ChangeState(RaycastStates.Raycast);
        }
    }

    private void Idle_Exit()
    {
        this.line.widthMultiplier = 0.3f;
    }

    private void Raycast_Enter()
    {
        if(this.grabbedObject != null){
            this.grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            this.grabbedObject.parent = null;
            this.grabbedObject = null;
        }
        player.EnableRotation = true;
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
            Debug.Log("instate Object");
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                5000, LayerMask.GetMask("Selected")))
            {
                Debug.Log("hit Object");
                this.line.SetPosition(1, hit.point);
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    this.grabbedObject = hit.transform;
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
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            fsm.ChangeState(RaycastStates.Raycast);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            fsm.ChangeState(RaycastStates.manipulateObject);
        }
        // Sets the line
        this.line.SetPosition(0, transform.position);
        this.line.SetPosition(1, this.grabbedObject.position);
    }
    private void manipulateObject_Enter()
    {
        player.EnableRotation = false;
        player.EnableLinearMovement = false;
    }

    private void manipulateObject_Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            fsm.ChangeState(RaycastStates.Raycast);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            fsm.ChangeState(RaycastStates.GrabbedObject);
        }
        // Sets the line
        this.line.SetPosition(0, transform.position);
        this.line.SetPosition(1, this.grabbedObject.position);
        // Position
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.up) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
            if (Vector3.Distance(transform.position, this.grabbedObject.transform.position) > 2)
                this.grabbedObject.transform.position += transform.TransformDirection(Vector3.down) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.left) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.right) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.Four))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.forward) * moveSpeed;
        if (OVRInput.Get(OVRInput.Button.Three))
            this.grabbedObject.transform.position += transform.TransformDirection(Vector3.back) * moveSpeed;

        // Rotation
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp) || Input.GetKey(KeyCode.I))
            this.grabbedObject.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKey(KeyCode.K))
            this.grabbedObject.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKey(KeyCode.J))
            this.grabbedObject.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) || Input.GetKey(KeyCode.L))
            this.grabbedObject.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
    }

    private void manipulateObject_Exit()
    {
        player.EnableRotation = true;
        player.EnableLinearMovement = true;
    }
}