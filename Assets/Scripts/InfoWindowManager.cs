using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class InfoWindowManager : MonoBehaviour
{
    public NearFarInteractor leftInteractor;
    public GameObject infoCanvas;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI formulaText;
    public TextMeshProUGUI bondText;
    public float animDuration = 0.3f;
    private bool _isVisible = false;

    void Start()
    {
        if (infoCanvas != null)
        {
            infoCanvas.transform.localScale = Vector3.zero;
            infoCanvas.SetActive(false);
        }
    }

    public void ToggleInfoWindow()
    {
        if (_isVisible) Hide();
        else Show();
    }

    private void Show()
    {
        if (leftInteractor != null && leftInteractor.hasSelection)
        {
            var interactable = leftInteractor.interactablesSelected[0].transform;
            UpdateUI(interactable);
            infoCanvas.SetActive(true);
            infoCanvas.transform.DOScale(new Vector3(0.001f, 0.001f, 0.001f), animDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
            _isVisible = true;
        }
    }

    private void UpdateUI(Transform target)
    {
        var molecule = target.GetComponent<MoleculeVisual>();
        if (molecule != null)
        {
            nameText.text = $"<b>Name:</b> {molecule.moleculeName}";
            formulaText.text = $"<b>Formula:</b> {molecule.formula}";
            bondText.text = $"<b>Bond:</b> {molecule.bondType}";
            return;
        }
        var atom = target.GetComponent<AtomController>();
        if (atom != null)
        {
            string fullName = GetFullElementName(atom.atomType);
            nameText.text = $"<b>Element:</b> {fullName}";
            formulaText.text = $"<b>Symbol:</b> {atom.atomType}"; 
            bondText.text = "<b>Status:</b> Free Atom"; 
        }
    }

    private string GetFullElementName(AtomType type)
    {
        switch (type)
        {
            case AtomType.H: return "Hydrogen";
            case AtomType.O: return "Oxygen";
            case AtomType.C: return "Carbon";
            case AtomType.N: return "Nitrogen";
            default: return type.ToString();
        }
    }

    public void Hide()
    {
        infoCanvas.transform.DOScale(Vector3.zero, animDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => infoCanvas.SetActive(false))
            .SetUpdate(true);
        _isVisible = false;
    }
}