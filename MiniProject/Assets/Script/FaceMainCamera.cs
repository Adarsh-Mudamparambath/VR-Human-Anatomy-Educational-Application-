using UnityEngine;

public class FaceMainCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // Get the main camera transform
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("No main camera found in the scene.");
        }
    }

    void Update()
    {
        // Make the object face the main camera
        if (mainCameraTransform != null)
        {
            transform.LookAt(mainCameraTransform);
        }
    }
}
