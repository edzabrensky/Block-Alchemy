using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightJoint : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        meshRenderer.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Joint"))
            meshRenderer.enabled = true;
    }

    public void OnTriggerExit(Collider other)
    {
        meshRenderer.enabled = false;
    }
}