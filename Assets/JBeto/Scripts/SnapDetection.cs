using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Register the joint to the TransformSet, 
// The script, OhSnap, reads off this TransformSet and figures out which joint to connect
public class SnapDetection : MonoBehaviour
{
    private OhSnap joints;

    private void Awake()
    {
        joints = Utility.GetSafeComponent<OhSnap>(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        // If the other collider is also a joint
        if (other.GetComponent<SnapDetection>() != null)
        {
            this.joints.AddItem(transform);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // If the other collider is also a joint
        if (other.GetComponent<SnapDetection>() != null)
        {
            this.joints.RemoveItem(transform);
        }
    }
}