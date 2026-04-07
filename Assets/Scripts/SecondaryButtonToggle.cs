using UnityEngine;
using UnityEngine.InputSystem;

public class SecondaryButtonToggle : MonoBehaviour
{
    public InfoWindowManager infoManager;
    public InputActionReference secondaryButtonAction;

    private void OnEnable()
    {
        if (secondaryButtonAction != null)
        {
            secondaryButtonAction.action.Enable();
            secondaryButtonAction.action.performed += (ctx) => infoManager.ToggleInfoWindow();
        }
    }

    private void OnDisable()
    {
        if (secondaryButtonAction != null)
            secondaryButtonAction.action.performed -= (ctx) => infoManager.ToggleInfoWindow();
    }
}