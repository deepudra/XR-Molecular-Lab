using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private HashSet<string> _discovered = new HashSet<string>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterMolecule(MoleculeData data)
    {
        if (_discovered.Contains(data.formula)) return;
        _discovered.Add(data.formula);
        Debug.Log($"📋 Molecule discovered: " +
            $"{data.moleculeName} ({data.formula}) " +
            $"— Bond: {data.bondType}");
    }
}