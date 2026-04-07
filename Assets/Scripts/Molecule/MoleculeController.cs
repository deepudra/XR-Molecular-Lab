using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using System;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class MoleculeController : MonoBehaviour
{
    private XRGrabInteractable _grab;
    private Rigidbody _rb;
    private BoxCollider _moleculeCollider;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _grab = GetComponent<XRGrabInteractable>();
        _rb.isKinematic = false;
        _rb.useGravity = false;
        _rb.linearDamping = 5f;
        _rb.angularDamping = 8f;
        _moleculeCollider = GetComponent<BoxCollider>();
        if (_moleculeCollider == null)
            _moleculeCollider = gameObject.AddComponent<BoxCollider>();
        _moleculeCollider.center = Vector3.zero;
    }


    public void UpdateMoleculeCollider(List<AtomController> atoms)
    {
        if (atoms == null || atoms.Count == 0) return;
        Bounds worldBounds = new Bounds(atoms[0].transform.position, Vector3.zero);
        foreach (var atom in atoms)
        {
            worldBounds.Encapsulate(atom.transform.position);
        }
        Vector3 localSize = transform.InverseTransformVector(worldBounds.size);
        float maxSide = Mathf.Max(localSize.x, Mathf.Max(localSize.y, localSize.z));
        float padding = 0.15f;
        _moleculeCollider.size = new Vector3(maxSide, maxSide, maxSide) + Vector3.one * padding;
        if (_grab != null)
        {
            _grab.colliders.Clear();
            _grab.colliders.Add(_moleculeCollider);
            _grab.enabled = false;
            _grab.enabled = true;
        }
    }

    public void OnReleased()
    {
        var procedural = GetComponent<ProceduralMolecule>();
        if (procedural != null && procedural.attachedAtoms.Count > 0)
        {
            if (BondManager.Instance != null)
                BondManager.Instance.TryBond(procedural.attachedAtoms[0]);
        }
    }
}