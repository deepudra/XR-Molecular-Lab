using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ProceduralMolecule : MonoBehaviour
{
    public List<AtomController> attachedAtoms = new List<AtomController>();
    private List<GameObject> activeBonds = new List<GameObject>();
    public Material bondMaterial;

    [Header("Animation Settings")]
    public float animationDuration = 0.4f;
    public float targetAtomScale = 0.15f;
    public Ease moveEase = Ease.OutQuad;

    [Header("Spacing Settings")]
    public float bondLength = 0.15f;

    [Header("Rotation Settings")]
    public float rotateSpeed = 20f;
    public bool canRotate = true;

    public void Construct(MoleculeData data, List<AtomController> atoms)
    {
        attachedAtoms = new List<AtomController>(atoms);
        var visual = GetComponent<MoleculeVisual>();
        if (visual != null)
            visual.SetupLabels(data.moleculeName, data.formula, data.bondType);
        var controller = GetComponent<MoleculeController>();
        if (controller != null)
            controller.UpdateMoleculeCollider(attachedAtoms);
        transform.localScale = Vector3.one;
        for (int i = 0; i < attachedAtoms.Count; i++)
        {
            AtomController atom = attachedAtoms[i];
            float angle = i * Mathf.PI * 2 / attachedAtoms.Count;
            Vector3 targetLocalPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * bondLength;
            var atomGrab = atom.GetComponent<XRGrabInteractable>();
            if (atomGrab != null) atomGrab.enabled = false;
            var atomCol = atom.GetComponent<Collider>();
            if (atomCol != null) atomCol.isTrigger = true;
            Vector3 worldPosBeforeParent = atom.transform.position;
            atom.transform.SetParent(this.transform, true);
            atom.transform.position = worldPosBeforeParent;
            var rb = atom.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            atom.transform.localScale = Vector3.zero;
            int index = i;
            atom.transform.DOScale(targetAtomScale, animationDuration).SetEase(Ease.OutElastic);
            atom.transform.DOLocalMove(targetLocalPos, animationDuration)
                .SetEase(moveEase)
                .OnComplete(() => {
                    if (index > 0) CreateBondBetween(attachedAtoms[0], attachedAtoms[index]);
                });
        }
        transform.DOPunchScale(Vector3.one * 0.05f, 0.3f);
    }

    void Update()
    {
        if (canRotate) transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void CreateBondBetween(AtomController center, AtomController outer)
    {
        if (center == null || outer == null) return;
        GameObject bond = BondDrawer.DrawBond(center.transform.position, outer.transform.position, bondMaterial, 0.02f, this.transform);
        Vector3 finalScale = bond.transform.localScale;
        bond.transform.localScale = new Vector3(finalScale.x, 0, finalScale.z);
        bond.transform.DOScaleY(finalScale.y, 0.2f);
        activeBonds.Add(bond);
    }

    public void ImmediateCleanup()
    {
        transform.DOKill();
        foreach (AtomController atom in attachedAtoms)
        {
            if (atom != null)
            {
                atom.transform.DOKill();
                atom.transform.SetParent(null);
                var rb = atom.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;
                atom.transform.localScale = Vector3.one * targetAtomScale;
                var grab = atom.GetComponent<XRGrabInteractable>();
                if (grab != null) grab.enabled = true;
                var col = atom.GetComponent<Collider>();
                if (col != null) col.isTrigger = false;
            }
        }
        Destroy(gameObject);
    }

    public void Disassemble()
    {
        canRotate = false;
        transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => {
            ImmediateCleanup();
        });
    }
}