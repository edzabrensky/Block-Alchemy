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
    Vector3 dist;

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
            Debug.Log(obj.transform.parent.GetComponent<Renderer>().bounds.size/2);
            JointUsed = true;
            center = validPos();
        }
    }

    Vector3 validPos()
    {
        List<Vector3> validPos = new List<Vector3>();
        Vector3 min = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        for (int i = 0; i < 14; ++i)
        {
            Vector3 x = new Vector3();
            float SizeOfRenderer = obj.transform.parent.GetComponent<Renderer>().bounds.size.x / 2 > obj.transform.parent.GetComponent<Renderer>().bounds.size.y / 2 ? obj.transform.parent.GetComponent<Renderer>().bounds.size.x / 2 : obj.transform.parent.GetComponent<Renderer>().bounds.size.y / 2;
            //float SizeOfRenderer = obj.transform.parent.GetComponent<Renderer>().bounds.size.y / 2;
            if (i < 2)
            {
                
                if (i % 2 == 0)
                {
                    x = transform.position + new Vector3(SizeOfRenderer, 0f, 0f);
                }
                else
                {
                    x = transform.position - new Vector3(SizeOfRenderer, 0f, 0f);
                }

            }
            else if (i < 4)
            {
                if (i % 2 == 0)
                {
                    x = transform.position + new Vector3(0f, SizeOfRenderer, 0f);
                }
                else
                {
                    x = transform.position - new Vector3(0f, SizeOfRenderer, 0f);
                }
            }
            else if (i < 6)
            {
                if (i % 2 == 0)
                {
                    x = transform.position + new Vector3(0f, 0f, SizeOfRenderer);
                }
                else
                {
                    x = transform.position - new Vector3(0f, 0f, SizeOfRenderer);
                }
            }
            else if(i < 8)
            {
                if (i % 2 == 0)
                {
                    x = transform.position + new Vector3(SizeOfRenderer, SizeOfRenderer, 0f);
                }
                else
                {
                    x = transform.position - new Vector3(SizeOfRenderer, SizeOfRenderer, 0f);
                }
            }
            else if(i < 10)
            {
                if (i % 2 == 0)
                {
                    x = transform.position + new Vector3(SizeOfRenderer, 0f, SizeOfRenderer);
                }
                else
                {
                    x = transform.position - new Vector3(SizeOfRenderer, 0f, SizeOfRenderer);
                }
            }
            else if(i < 12)
            {
                if (i % 2 == 0)
                {
                    x = transform.position + new Vector3(0f, SizeOfRenderer, SizeOfRenderer);
                }
                else
                {
                    x = transform.position - new Vector3(0f, SizeOfRenderer, SizeOfRenderer);
                }
            }
            else if(i < 14)
            {
                if (i % 2 == 0)
                {
                    x = transform.position + new Vector3(SizeOfRenderer, SizeOfRenderer, SizeOfRenderer);
                }
                else
                {
                    x = transform.position - new Vector3(SizeOfRenderer, SizeOfRenderer, SizeOfRenderer);
                }
            }
            if(!transform.parent.GetComponent<Collider>().bounds.Contains(x))
            {
                validPos.Add(x);
                if(Vector3.Distance(obj.transform.parent.position, min) > Vector3.Distance(obj.transform.parent.position, x) && Vector3.Distance(obj.transform.position, min) > Vector3.Distance(obj.transform.position, x))
                {
                    min = x;
                }
            }
            //Debug.Log(obj.transform.parent.GetComponent<Collider>().bounds.size.y * 2);
        }
        dist = transform.parent.position - min;
        return min;
    }

    // Update is called once per frame
    void Update()
    {
        if(found && JointUsed)
        {
            //center = transform.position;
            //obj.transform.parent.transform.SetPositionAndRotation(center + new Vector3(0f,0f,-.5f), obj.transform.rotation);
            //center = validPos();

            Vector3 offset = center- transform.parent.position + dist;
            //Debug.Log(offset);
            obj.transform.parent.transform.SetPositionAndRotation(center - offset, obj.transform.rotation);

        }
    }
}