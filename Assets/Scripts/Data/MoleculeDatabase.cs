using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MoleculeDatabase",
    menuName = "MolLab/MoleculeDatabase")]
public class MoleculeDatabase : ScriptableObject
{
    public List<MoleculeData> molecules;

    public MoleculeData FindMatch(
        int h, int o, int c, int n)
    {
        if (molecules == null) return null;

        return molecules.Find(m =>
            m.hydrogenCount == h &&
            m.oxygenCount == o &&
            m.carbonCount == c &&
            m.nitrogenCount == n);
    }
}