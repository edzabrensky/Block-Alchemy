using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour {
    public float rotationSpeed;
    LineRenderer line;
    bool grabbedItem;

    // Use this for initialization
    void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        grabbedItem = false;
	}
	
	// Update is called once per frame
	void Update () {
        line.SetPosition(0, transform.position) ;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayerMask.GetMask("Item")))
        {
            line.SetPosition(1, hit.point);
            if (Input.GetKeyDown(KeyCode.C) && !grabbedItem)
            {
                //hit.transform.position = new Vector3(transform.position.x, transform.position.y, hit.transform.position.z);
                hit.transform.position = transform.position + transform.TransformDirection(Vector3.forward) * Vector3.Distance(transform.position, hit.transform.position);
                hit.transform.parent = transform;
                grabbedItem = true;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.C) && grabbedItem)
                {
                    hit.transform.parent = null;
                    grabbedItem = false;
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
                //if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp) || Input.GetKey(KeyCode.I))
                //{
                //    hit.transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
                //    Debug.Log("TURN");
                //}
                if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKey(KeyCode.I))
                    hit.transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
                if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKey(KeyCode.K))
                    hit.transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
                //if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) || Input.GetKey(KeyCode.L))
                //    hit.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
                if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) || Input.GetKey(KeyCode.J))
                    hit.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
                if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight) || Input.GetKey(KeyCode.L))
                    hit.transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            line.SetPosition(1, transform.TransformDirection(Vector3.forward) * 5000);
        }
        
    }
}
