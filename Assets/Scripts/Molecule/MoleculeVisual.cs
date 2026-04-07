using UnityEngine;
using TMPro;

public class MoleculeVisual : MonoBehaviour
{
    [Header("Info")]
    public string moleculeName;
    public string formula;
    public string bondType;

    [Header("Label References")]
    public TextMeshPro formulaLabel;
    public TextMeshPro nameLabel;

    public void SetupLabels(string mName, string mFormula, string mBondType)
    {
        moleculeName = mName;
        formula = mFormula;
        bondType = mBondType;
        if (nameLabel != null)
        {
            nameLabel.text = mName;
            nameLabel.gameObject.SetActive(true);
        }
        if (formulaLabel != null)
        {
            string formattedFormula = FormatSubscripts(mFormula);
            formulaLabel.text = formattedFormula;
            formulaLabel.gameObject.SetActive(true);
        }
    }
    private string FormatSubscripts(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        string output = "";
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                output += $"<sub>{c}</sub>";
            }
            else
            {
                output += c;
            }
        }
        return output;
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }
}