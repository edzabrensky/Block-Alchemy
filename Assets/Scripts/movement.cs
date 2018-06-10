using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed, rotationSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.TransformDirection(Vector3.forward) * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.TransformDirection(Vector3.back) * speed;
        }
        //if (input.getkey(keycode.a))
        //{
        //    transform.position += vector3.left * speed;
        //}
        //if (input.getkey(keycode.d))
        //{
        //    transform.position += vector3.right * speed;
        //}
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += transform.TransformDirection(Vector3.up) * speed;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += transform.TransformDirection(Vector3.down) * speed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
            transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.back * Time.deltaTime * rotationSpeed);
    }
}
