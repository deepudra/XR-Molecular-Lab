using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerMenuToggle : MonoBehaviour
{
    public AtomSpawner spawner;

    [Header("Input Actions")]
    [Tooltip("Assign the Right Hand Primary Button action here")]
    public InputActionReference menuButtonAction;

    private void OnEnable()
    {
        if (menuButtonAction != null && menuButtonAction.action != null)
        {
            menuButtonAction.action.Enable();
            menuButtonAction.action.performed += OnMenuButtonPressed;
        }
    }

    private void OnDisable()
    {
        if (menuButtonAction != null && menuButtonAction.action != null)
        {
            menuButtonAction.action.performed -= OnMenuButtonPressed;
        }
    }

    private void OnMenuButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Button Pressed!");
        if (spawner != null)
        {
            spawner.ToggleMenu();
        }
    }
}