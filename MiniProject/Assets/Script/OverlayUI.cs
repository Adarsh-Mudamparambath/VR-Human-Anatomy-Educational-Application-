using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Canvas))]
public class OverlayUI : MonoBehaviour
{
    private Dictionary<Material, Material> _materialMap = new Dictionary<Material, Material>();

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Wait for 1 frame to load all the materials first.
        yield return new WaitForEndOfFrame();

        ApplyMaterialModifications();
    }
    private void OnEnable()
    {
        Canvas.willRenderCanvases += ApplyMaterialModifications;
    }

    private void OnDisable()
    {
        Canvas.willRenderCanvases -= ApplyMaterialModifications;
    }

    private void ApplyMaterialModifications()
    {
        // Wait for 1 frame to load all the materials first.

        // Get all child CanvasRenderers of the canvas
        CanvasRenderer[] childRenderers = GetComponentsInChildren<CanvasRenderer>();

        // Iterate through each child renderer
        foreach (CanvasRenderer canvasRenderer in childRenderers)
        {
            // Iterate through each material of the canvas renderer
            for (int i = 0; i < canvasRenderer.materialCount; i++)
            {
                Material rendererMat = canvasRenderer.GetMaterial(i);
                Material newMaterial;

                // Cache materials to reduce new instances of materials for same purposes
                if (_materialMap.ContainsKey(rendererMat))
                {
                    newMaterial = _materialMap[rendererMat];
                }
                else
                {
                    // Use Instantiate to preserve stencil properties
                    newMaterial = Instantiate(rendererMat);
                    _materialMap.Add(rendererMat, newMaterial);
                }

                // To render images over all other objects
                newMaterial.renderQueue = (int)RenderQueue.Overlay + 1;
                // No depth testing occurs. Draw all geometry using this material, regardless of distance.
                newMaterial.SetInt("unity_GUIZTestMode", (int)CompareFunction.Always);

                // Set the updated material back to the canvas renderer
                canvasRenderer.SetMaterial(newMaterial, i);
            }
        }
    }
}
