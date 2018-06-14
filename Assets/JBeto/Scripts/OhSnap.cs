using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OhSnap : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Dictionary<OhSnap, List<Transform>> ownerToCollection;
    private List<Joint> connections;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        connections = new List<Joint>();
        ownerToCollection = new Dictionary<OhSnap, List<Transform>>();
    }

    #region JointCollection
    public void AddItem(Transform item, OhSnap os)
    {
        if (!ownerToCollection.ContainsKey(os))
        {
            ownerToCollection.Add(os, new List<Transform>());
        }
        ownerToCollection[os].Add(item);
    }

    public void RemoveItem(Transform item, OhSnap os)
    {
        ownerToCollection[os].Remove(item);
    }
    #endregion

    // Snaps joints based on how many are connected
    // Valid joint positions are recorded in collection
    public void SnapJoints()
    {
        foreach (OhSnap other in ownerToCollection.Keys)
        {
            // Dont snap to something already connected
            if (connections.Contains(other.GetComponent<Joint>()))
            {
                continue;
            }
            Debug.Log("Snapping joint");
            List<Transform> collection = ownerToCollection[other];
            // Both joints are occupied, can't snap
            if ((other.GetComponent<Joint>() != null) && (GetComponent<Joint>() != null))
            {
                return;
            }
            // No joints to snap with
            if (collection.Count == 0)
            {
                Debug.Log("No joint to snap");
                return;
            }

            GameObject toAdd = other.AddJointToThis(this);
            if (collection.Count == 1)
            {
                // Spring Joint
                // SpringJoint spring = toAdd.AddComponent<SpringJoint>();
                FixedJoint spring = toAdd.AddComponent<FixedJoint>();
                // spring.spring = 50;
                connections.Add(spring);
                spring.connectedBody = (toAdd == gameObject) ? other.rigidBody : rigidBody;
                spring.anchor = collection[0].localPosition;
                spring.enableCollision = true;
            }
            else if (collection.Count == 2)
            {
                // Hinge Joint
                //HingeJoint hinge = toAdd.AddComponent<HingeJoint>();
                FixedJoint hinge = toAdd.AddComponent<FixedJoint>();
                connections.Add(hinge);
                // hinge.
                hinge.connectedBody = (toAdd == gameObject) ? other.rigidBody : rigidBody;
                hinge.anchor = Vector3.Lerp(collection[0].localPosition, collection[1].localPosition, .5f);
                hinge.enableCollision = true;
            }
            else
            {
                // Fixed Joint
                FixedJoint fix = toAdd.AddComponent<FixedJoint>();
                connections.Add(fix);
                fix.enableCollision = true;
                fix.connectedBody = (toAdd == gameObject) ? other.rigidBody : rigidBody;
                Vector3 avg = Vector3.zero;
                foreach (Transform t in collection)
                {
                    avg += t.localPosition;
                }
                avg /= collection.Count;
                fix.anchor = avg;
            }
        }
    }
    
    public void BreakJoints()
    {
        foreach(Joint j in connections)
        {
            Debug.Log("Destroyed");
            Destroy(j);
        }
        connections.Clear();
    }

    // Finds out which game object to add joint to
    private GameObject AddJointToThis(OhSnap other)
    {
        return (GetComponent<Joint>() == null) ? gameObject : other.gameObject;
    }
}