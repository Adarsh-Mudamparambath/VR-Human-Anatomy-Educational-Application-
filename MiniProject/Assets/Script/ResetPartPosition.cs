using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetPartPosition : MonoBehaviour
{
    public List<Transform> parts; // List of transforms to store
    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> initialRotations = new Dictionary<Transform, Quaternion>();
    private Dictionary<Transform, Vector3> initialScales = new Dictionary<Transform, Vector3>();

    void Start()
    {
        // Store the initial positions, rotations, and scales of all parts
        foreach (Transform part in parts)
        {
            initialPositions[part] = part.position;
            initialRotations[part] = part.rotation;
            initialScales[part] = part.localScale;
        }
    }

    public void ResetPositions()
    {
        // Reset each part to its initial position using a coroutine for smooth transition
        foreach (Transform part in parts)
        {
            if (initialPositions.ContainsKey(part))
            {
                StartCoroutine(LerpToPosition(part, initialPositions[part]));
            }
        }
    }

    public void ResetRotations()
    {
        // Reset each part to its initial rotation using a coroutine for smooth transition
        foreach (Transform part in parts)
        {
            if (initialRotations.ContainsKey(part))
            {
                StartCoroutine(LerpToRotation(part, initialRotations[part]));
            }
        }
    }

    public void ResetScales()
    {
        // Reset each part to its initial scale using a coroutine for smooth transition
        foreach (Transform part in parts)
        {
            if (initialScales.ContainsKey(part))
            {
                StartCoroutine(LerpToScale(part, initialScales[part]));
            }
        }
    }

    private IEnumerator LerpToPosition(Transform part, Vector3 targetPosition)
    {
        float duration = 1.0f; // Duration of the lerp
        float elapsedTime = 0;
        Vector3 startingPosition = part.position;

        while (elapsedTime < duration)
        {
            part.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        part.position = targetPosition; // Ensure the final position is set accurately
    }

    private IEnumerator LerpToRotation(Transform part, Quaternion targetRotation)
    {
        float duration = 1.0f; // Duration of the lerp
        float elapsedTime = 0;
        Quaternion startingRotation = part.rotation;

        while (elapsedTime < duration)
        {
            part.rotation = Quaternion.Lerp(startingRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        part.rotation = targetRotation; // Ensure the final rotation is set accurately
    }

    private IEnumerator LerpToScale(Transform part, Vector3 targetScale)
    {
        float duration = 1.0f; // Duration of the lerp
        float elapsedTime = 0;
        Vector3 startingScale = part.localScale;

        while (elapsedTime < duration)
        {
            part.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        part.localScale = targetScale; // Ensure the final scale is set accurately
    }
}