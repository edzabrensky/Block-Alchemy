using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OhSnap : MonoBehaviour
{
    private List<Transform> collection;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        collection = new List<Transform>();
    }

    #region JointCollection
    public void AddItem(Transform item)
    {
        collection.Add(item);
    }

    public void RemoveItem(Transform item)
    {
        collection.Remove(item);
    }
    #endregion

    // Snaps joints based on how many are connected
    // Valid joint positions are recorded in collection
    public void SnapJoints(OhSnap other)
    {
        // Both joints are occupied, can't snap
        if ((other.GetComponent<Joint>() != null) && (GetComponent<Joint>() != null))
        {
            return;
        }
        // No joints to snap with
        if (collection.Count == 0)
        {
            return;
        }

        GameObject toAdd = AddJointToThis(other);
        if (collection.Count == 1)
        {
            // Spring Joint
            SpringJoint spring = toAdd.AddComponent<SpringJoint>();
            spring.connectedBody = (toAdd == gameObject) ? other.rigidBody : rigidBody;
            spring.anchor = collection[0].localPosition;
            spring.enableCollision = true;
        }
        else if (collection.Count == 2)
        {
            // Hinge Joint
            HingeJoint hinge = toAdd.AddComponent<HingeJoint>();
            hinge.connectedBody = (toAdd == gameObject) ? other.rigidBody : rigidBody;
            hinge.anchor = Vector3.Lerp(collection[0].localPosition, collection[1].localPosition, .5f);
            hinge.enableCollision = true;
        }
        else
        {
            // Fixed Joint
            FixedJoint fix = toAdd.AddComponent<FixedJoint>();
            fix.enableCollision = true;
            fix.connectedBody = (toAdd == gameObject) ? other.rigidBody : rigidBody;
            Vector3 avg = Vector3.zero;
            foreach(Transform t in collection)
            {
                avg += t.localPosition;
            }
            avg /= collection.Count;
            fix.anchor = avg;
        }
    }
    
    // Finds out which game object to add joint to
    public GameObject AddJointToThis(OhSnap other)
    {
        return (GetComponent<Joint>() == null) ? gameObject : other.gameObject;
    }
}