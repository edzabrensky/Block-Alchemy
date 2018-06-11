using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour {
    public float rotationSpeed;
    LineRenderer line;
    bool grabbedItem;
    public OVRPlayerController player;
    private RaycastHit hit;
    enum RaycastStates { Initial, RaycastActive, RaycastInactive, grabbedObject, releasedObject };
    private RaycastStates states;

    // Use this for initialization
    void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        grabbedItem = false;
        states = RaycastStates.Initial;
    }

    // Update is called once per frame

  void Update () {
        line.SetPosition(0, transform.position);
        //switch (states)
        //{
        //    case RaycastStates.RaycastInactive:
        //        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        //            states = RaycastStates.RaycastActive;
        //        break;
        //    case RaycastStates.RaycastActive:
        //        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        //            states = RaycastStates.RaycastInactive;
        //        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
        //            5000, LayerMask.GetMask("Selected")))
        //        {
        //            line.SetPosition(1, hit.point);
        //            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        //            {
        //                states = RaycastStates.grabbedObject;
        //            }
        //        }
        //        else
        //        {
        //            line.SetPosition(1, transform.TransformDirection(Vector3.forward) * 5000);
        //        }
        //        break;
        //    case RaycastStates.grabbedObject:
        //        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        //        {
        //            hit.transform.parent = null;
        //            player.EnableRotation = true;
        //            states = RaycastStates.releasedObject;
        //        }
        //        break;
        //}

        //switch (states)
        //{
        //    case RaycastStates.RaycastInactive:
        //        line.SetPosition(1, transform.position);
        //        break;
        //    case RaycastStates.RaycastActive:
        //        ;
        //        break;
        //    case RaycastStates.hitObject:
        //        break;
        //    case RaycastStates.grabbedObject:
        //        hit.transform.position = transform.position + transform.TransformDirection(Vector3.forward) * Vector3.Distance(transform.position, hit.transform.position);
        //        hit.transform.parent = transform;
        //        //grabbedItem = true;
        //        player.EnableRotation = false;
        //        break;
        //}

        // Does the ray intersect any objects excluding the player layer

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
        
    }
}
