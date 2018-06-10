using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour {
    public SphereCollider sc;
    bool attach = true;
    // Use this for initialization
    void Start () {
		
	}
    void OnTriggerEnter(Collider other)
    {
        if(attach)
        {
            HingeJoint hinge = gameObject.AddComponent<HingeJoint>();
            hinge.connectedBody = other.gameObject.GetComponent<Rigidbody>();
            hinge.anchor = transform.localPosition;
            //hinge.axis = new Vector3(0f, 1f, 0f);
            Debug.Log(other.gameObject.name);
        }
        Debug.Log("entered!");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
