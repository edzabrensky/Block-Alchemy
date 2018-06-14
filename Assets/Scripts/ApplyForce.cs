using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour {

    private Vector3 velocity;
    private Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(velocity * 50);
    }

    void OnCollisionEnter(Collision collision)
    {
        velocity += collision.relativeVelocity;
        //Debug.Log(velocity);
    }

    public void updateForce()
    {
        Debug.Log("FORCE APPLIED");
        Debug.Log("VELOCITY:");
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.AddForce(velocity);
        //velocity = Vector3.zero;
    }
}
