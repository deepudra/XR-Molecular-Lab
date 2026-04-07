using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BondManager : MonoBehaviour
{
    public static BondManager Instance;

    [Header("Database")]
    public MoleculeDatabase database;

    [Header("Spawn Settings")]
    public float bondRadius = 0.35f;

    [Header("Procedural Settings")]
    public GameObject genericMoleculePrefab;

    void Awake()
    {
        Instance = this;
    }

    public void TryBond(AtomController droppedAtom)
    {
        Collider[] hits = Physics.OverlapSphere(droppedAtom.transform.position, bondRadius);
        List<AtomController> nearbyAtoms = new List<AtomController>();

        foreach (Collider hit in hits)
        {
            AtomController atom = hit.GetComponent<AtomController>();
            if (atom != null && atom != droppedAtom)
            {
                ProceduralMolecule existingMol = atom.GetComponentInParent<ProceduralMolecule>();
                if (existingMol != null)
                {
                    foreach (var a in existingMol.attachedAtoms)
                    {
                        if (!nearbyAtoms.Contains(a)) nearbyAtoms.Add(a);
                    }
                    existingMol.ImmediateCleanup();
                }
                else
                {
                    if (!nearbyAtoms.Contains(atom)) nearbyAtoms.Add(atom);
                }
            }
        }

        if (nearbyAtoms.Count == 0) return;
        nearbyAtoms.Add(droppedAtom);
        int h = nearbyAtoms.Count(a => a.atomType == AtomType.H);
        int o = nearbyAtoms.Count(a => a.atomType == AtomType.O);
        int c = nearbyAtoms.Count(a => a.atomType == AtomType.C);
        int n = nearbyAtoms.Count(a => a.atomType == AtomType.N);
        MoleculeData match = database.FindMatch(h, o, c, n);
        if (match != null)
        {
            SpawnMolecule(match, nearbyAtoms);
        }
        if (match == null && nearbyAtoms.Count > 1)
        {
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ShowSystemMessage("Combination not found!", Color.red);
            }
        }
    }

    void SpawnMolecule(MoleculeData data, List<AtomController> atoms)
    {
        Vector3 center = Vector3.zero;
        foreach (AtomController atom in atoms)
            center += atom.transform.position;
        center /= atoms.Count;
        GameObject molObj = Instantiate(genericMoleculePrefab, center, Quaternion.identity);
        ProceduralMolecule generator = molObj.GetComponent<ProceduralMolecule>();
        if (generator != null)
        {
            generator.Construct(data, atoms);
        }
        if (data.bondSound != null) AudioSource.PlayClipAtPoint(data.bondSound, center);
        if (UIManager.Instance != null) UIManager.Instance.RegisterMolecule(data);
        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowDiscoveryNotification(data.moleculeName, data.formula, Color.green);
        }
    }
}