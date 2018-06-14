using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCollision : MonoBehaviour
{

    public Rigidbody Collided;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Collided = collision.gameObject.GetComponent<Rigidbody>();
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        Collided = null;
    }
}