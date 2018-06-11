using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour
{
    bool attach = true;
    GameObject obj;
    GameObject parent;
    Vector3 offset;
    bool found = false;
    Vector3 center;
    bool JointUsed = false;
    // Use this for initialization
    void Start()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (attach && !JointUsed && !other.gameObject.GetComponent<Snapping>().JointUsed)
        {
            if(other.gameObject.transform.parent.transform.parent != null)
            {
                //transform.position = other.bounds.center;
                obj = other.gameObject.transform.root.gameObject;
                parent = obj.transform.root.gameObject;
                //center = other.transform.position;


                obj.transform.parent.transform.SetParent(parent.transform);
                Debug.Log(other.gameObject.name);
                transform.root.SetParent(parent.transform);
            }
            else
            {
                obj = other.gameObject;
                center = other.bounds.center;
                //center = other.gameObject.transform.position;
                Debug.Log(center);
                found = true;

                //obj.transform.root.transform.SetPositionAndRotation(center, obj.transform.rotation);
                parent = new GameObject("Combined Obj");
                //parent.transform.position = new Vector3(0, 0, 0);

                obj.transform.parent.transform.SetParent(parent.transform);
                Debug.Log(other.gameObject.name);
                transform.root.SetParent(parent.transform);
        
            }
            JointUsed = true;
        }
        Debug.Log("entered!");
    }

    // Update is called once per frame
    void Update()
    {
        if(found)
        {
            center = transform.position;
            obj.transform.parent.transform.SetPositionAndRotation(center + new Vector3(0f,0f,-0.5f), obj.transform.rotation);
        }
    }
}