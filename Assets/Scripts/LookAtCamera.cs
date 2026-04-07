using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _mainCameraTransform;

    void Start()
    {
        if (Camera.main != null)
        {
            _mainCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("LookAtCamera: No Main Camera found in the scene!");
        }
    }

    void LateUpdate()
    {
        if (_mainCameraTransform != null)
        {
            transform.LookAt(_mainCameraTransform);
        }
    }
}