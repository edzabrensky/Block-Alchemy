using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

// Attach this script to the left hand
public class Energy : MonoBehaviour
{
    private enum EnergyStates { Dormant, StoreEnergy }
    private StateMachine<EnergyStates> fsm;
    private Vector3 origin;
    private Dictionary<Rigidbody, Vector3> storedEnergy;
    private Rigidbody current;

    private void Awake()
    {
        fsm = StateMachine<EnergyStates>.Initialize(this);
        fsm.ChangeState(EnergyStates.Dormant);
    }

    public void StoreEnergy(Rigidbody obj)
    {
        current = obj;
        fsm.ChangeState(EnergyStates.StoreEnergy);
    }

    public void Cancel()
    {
        fsm.ChangeState(EnergyStates.Dormant);
    }

    public void ReleaseEnergy()
    {
        foreach (Rigidbody rb in storedEnergy.Keys)
        {
            rb.AddForce(storedEnergy[rb]);
        }
        storedEnergy.Clear();
    }

    private void StoreEnergy_Enter()
    {
        origin = transform.position;
    }

    private void StoreEnergy_Update()
    {
        // Draw ray here
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {
            storedEnergy.Add(current, origin - transform.position);
            fsm.ChangeState(EnergyStates.Dormant);
        }
    }
}