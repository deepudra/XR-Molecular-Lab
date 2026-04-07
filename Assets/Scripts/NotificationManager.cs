using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public GameObject notificationCanvas;
    public TextMeshProUGUI statusText;

    [Header("Settings")]
    public float displayDuration = 2.0f;
    public float animDuration = 0.3f;

    private HashSet<string> discoveredMolecules = new HashSet<string>();
    private Sequence _activeSequence;

    void Awake()
    {
        Instance = this;
        if (notificationCanvas != null)
        {
            notificationCanvas.transform.localScale = Vector3.zero;
            notificationCanvas.SetActive(false);
        }
    }

    public void ShowDiscoveryNotification(string moleculeName, string formula, Color textColor)
    {
        if (!discoveredMolecules.Contains(moleculeName))
        {
            discoveredMolecules.Add(moleculeName);
            string message = $"New Molecule Found: {moleculeName} ({formula})";
            ExecutePopUp(message, textColor);
        }
    }

    public void ShowSystemMessage(string message, Color textColor)
    {
        ExecutePopUp(message, textColor);
    }

    private void ExecutePopUp(string message, Color textColor)
    {
        if (notificationCanvas == null || statusText == null) return;

        _activeSequence?.Kill();

        statusText.text = message;
        statusText.color = textColor;
        notificationCanvas.SetActive(true);

        _activeSequence = DOTween.Sequence();

        _activeSequence.Append(notificationCanvas.transform.DOScale(new Vector3(0.001f, 0.001f, 0.001f), animDuration).SetEase(Ease.OutBack))
                       .AppendInterval(displayDuration)
                       .Append(notificationCanvas.transform.DOScale(Vector3.zero, animDuration).SetEase(Ease.InBack))
                       .OnComplete(() => notificationCanvas.SetActive(false));

        _activeSequence.SetUpdate(true);
    }
}