using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public enum AtomType { H, O, C, N }

public class AtomController : MonoBehaviour
{
    [Header("Atom Settings")]
    public AtomType atomType;
    public float bondRadius = 0.2f;

    private XRGrabInteractable _grab;
    private Renderer _renderer;
    private Color _originalColor;
    private Rigidbody _rb;
    private bool _isGrabbed = false;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        _rb = GetComponent<Rigidbody>();
        _grab = GetComponent<XRGrabInteractable>();
        _grab.movementType = XRBaseInteractable.MovementType.Instantaneous;
        _grab.selectEntered.AddListener(OnGrabbed);
        _grab.selectExited.AddListener(OnReleased);
    }

    void Start()
    {
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        _isGrabbed = true;
        _renderer.material.color = Color.yellow;
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    void OnReleased(SelectExitEventArgs args)
    {
        _isGrabbed = false;
        _renderer.material.color = _originalColor;
        StartCoroutine(FreezeAfterDrop());
    }

    System.Collections.IEnumerator FreezeAfterDrop()
    {
        yield return null;
        yield return null;
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        if (BondManager.Instance != null)
            BondManager.Instance.TryBond(this);
    }

    void OnDestroy()
    {
        if (_grab != null)
        {
            _grab.selectEntered.RemoveListener(OnGrabbed);
            _grab.selectExited.RemoveListener(OnReleased);
        }
    }
}