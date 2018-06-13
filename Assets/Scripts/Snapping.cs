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
    public bool JointUsed = false;


    List<Vector3> validRotation;
    enum cubeStates { corner1, corner2, corner3, corner4 };
    cubeStates cubeState;


    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<SphereCollider>().isTrigger = false;
        GetComponent<SphereCollider>().isTrigger = true;

        //Debug.Log("entered!");*/
    }

    private void OnTriggerStay(Collider other)
    {
        if (attach && !JointUsed && !other.gameObject.GetComponent<Snapping>().JointUsed && GetComponent<SphereCollider>().isTrigger == true)
        {
            if (other.gameObject.transform.parent.transform.parent != null)
            {
                //transform.position = other.bounds.center;
                obj = other.gameObject.transform.root.gameObject;
                parent = obj.transform.root.gameObject;
                //center = other.transform.position;
                center = transform.position;

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
                Debug.Log(other.gameObject.transform.parent.name);
                transform.root.SetParent(parent.transform);
                //center = obj.transform.position;
                //transform.parent.transform.SetPositionAndRotation(center + new Vector3(0f, 0f, -0.5f), obj.transform.rotation);
            }
            JointUsed = true;
        }
    }

    //Vector3 validPos()
    //{
    //    List<Vector3> validPos;
    //    for (int i = 0; i < 6; ++i)
    //    {
    //        if (i < 2)
    //        {
    //            if (i % 2 == 0)
    //            {
    //                validPos.Insert(transform.position + new Vector3();
    //            }
    //            else
    //            {

    //            }

    //        }
    //        else if (i < 4)
    //        {
    //            if (i % 2 == 0)
    //            {

    //            }
    //            else
    //            {

    //            }
    //        }
    //        else if (i < 6)
    //        {
    //            if (i % 2 == 0)
    //            {

    //            }
    //            else
    //            {

    //            }
    //        }
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        if(found && JointUsed)
        {
            center = transform.position;
            obj.transform.parent.transform.SetPositionAndRotation(center + new Vector3(0f,0f,-.5f), obj.transform.rotation);
        }
    }
}