using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unsnap : MonoBehaviour {
    public GameObject ObjectToDetach;
    public bool detach = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(detach)
        {
            for(var i = ObjectToDetach.transform.childCount - 1; i >= 0; i--)
            {
                var ObjectA = ObjectToDetach.transform.GetChild(i);
                ObjectA.GetComponent<Snapping>().JointUsed = false;
            }
            ObjectToDetach.transform.SetParent(null);
        }
	}
}
