using UnityEngine;

[System.Serializable]
public class MoleculeData
{
    public string moleculeName;
    public string formula;
    public string bondType;

    [Header("Atom Counts")]
    public int hydrogenCount;
    public int oxygenCount;
    public int carbonCount;
    public int nitrogenCount;

    [Header("Visuals & Audio")]
    public GameObject moleculePrefab;
    public AudioClip bondSound;
}