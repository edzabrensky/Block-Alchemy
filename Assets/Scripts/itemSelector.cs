using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour {
    public float rotationSpeed;
    LineRenderer line;
    float playerObjDist;
    bool grabbedItem;

    // Use this for initialization
    void Start () {
        line = gameObject.GetComponent<LineRenderer>();
        playerObjDist = 0;
        grabbedItem = false;
	}
	
	// Update is called once per frame
	void Update () {
        line.SetPosition(0, transform.position + new Vector3(0.15f, -0.15f, 0)) ;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LayerMask.GetMask("Item")))
        {
            line.SetPosition(1, hit.point);
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerObjDist = Vector3.Distance(transform.position,hit.transform.position);
                grabbedItem = !grabbedItem;
            }
            if (grabbedItem)
            {
                var d = Input.GetAxis("Mouse ScrollWheel");
                if (d > 0f)
                {
                    playerObjDist += 1;
                }
                else if (d < 0f)
                {
                    if (playerObjDist > 1)
                        playerObjDist -= 1;
                }
                hit.transform.position = transform.position + transform.TransformDirection(Vector3.forward) * playerObjDist;
                hit.transform.rotation = transform.rotation;
                //if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp) || Input.GetKey(KeyCode.I))
                //    hit.transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
                //if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown) || Input.GetKey(KeyCode.K))
                //    hit.transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
                //if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKey(KeyCode.J))
                //    hit.transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
                //if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) || Input.GetKey(KeyCode.L))
                //    hit.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
                //if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) || Input.GetKey(KeyCode.LeftArrow))
                //    hit.transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
                //if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight) || Input.GetKey(KeyCode.RightArrow))
                //    hit.transform.Rotate(Vector3.back * Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            line.SetPosition(1, transform.TransformDirection(Vector3.forward) * 5000);
        }
        
    }
}
