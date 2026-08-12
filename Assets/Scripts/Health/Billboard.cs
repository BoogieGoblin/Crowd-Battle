using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Cache the main camera for better performance
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        FaceCamera();
    }

    void FaceCamera()
    {
        if (mainCamera != null)
        {
            // Forces the canvas to match the camera's rotation
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
