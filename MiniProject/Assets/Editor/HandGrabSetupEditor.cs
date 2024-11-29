using UnityEditor;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class HandGrabSetupEditor : EditorWindow
{
    private GameObject[] targetObjects;

    [MenuItem("Tools/Hand Grab Setup")]
    public static void ShowWindow()
    {
        GetWindow<HandGrabSetupEditor>("Hand Grab Setup");
    }

    private void OnGUI()
{
    GUILayout.Label("Hand Grab Setup", EditorStyles.boldLabel);

    // Resize the array with GUILayout
    int arraySize = (targetObjects != null) ? targetObjects.Length : 0;
    arraySize = EditorGUILayout.IntField("Number of Target Objects", arraySize);
    if (targetObjects == null || targetObjects.Length != arraySize)
    {
        targetObjects = new GameObject[arraySize];
    }

    // Iterate through each element to create ObjectFields for individual GameObjects
    for (int i = 0; i < targetObjects.Length; i++)
    {
        targetObjects[i] = EditorGUILayout.ObjectField("Target Object " + (i + 1), targetObjects[i], typeof(GameObject), true) as GameObject;
    }

    if (GUILayout.Button("Apply"))
    {
        if (targetObjects != null && targetObjects.Length > 0)
        {
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    ApplyHandGrabSetup(obj);
                }
            }
        }
        else
        {
            Debug.LogWarning("Please assign at least one target object.");
        }
    }
}

private void ApplyHandGrabSetup(GameObject obj)
{
    // Add Collider if it doesn't exist
    Collider collider = obj.GetComponent<Collider>();
    if (collider == null)
    {
        BoxCollider boxCollider = obj.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }
    else
    {
        collider.isTrigger = true;
    }

    // Add Rigidbody if it doesn't exist
    Rigidbody rb = obj.GetComponent<Rigidbody>();
    if (rb == null)
    {
        rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }
    else
    {
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // Check if the object already has a child named "HandGrabChild"
    Transform existingChild = obj.transform.Find("HandGrabChild");
    if (existingChild != null)
    {
        // Remove the existing child
        GameObject.DestroyImmediate(existingChild.gameObject);
    }

    // Create child object for Hand Grab scripts
    GameObject handGrabChild = new GameObject("HandGrabChild");
    handGrabChild.transform.SetParent(obj.transform);
    handGrabChild.transform.localPosition = Vector3.zero;
    handGrabChild.transform.localRotation = Quaternion.identity;

    // Add required scripts to the child object and set references
    Grabbable grabbable = handGrabChild.AddComponent<Grabbable>();
    grabbable.InjectOptionalRigidbody(rb); // Assign the Rigidbody to Grabbable
    grabbable.TargetTransform = obj.transform;


    // Set "Transfer on Second Selection" to true
    var transferOnSecondSelectionField = typeof(Grabbable).GetField("_transferOnSecondSelection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    if (transferOnSecondSelectionField != null)
    {
        transferOnSecondSelectionField.SetValue(grabbable, true);
    }

    // Set "Target Transform" to be the original object
    grabbable.InjectOptionalTargetTransform(obj.transform);

    CustomHandGrabInteractable handGrabInteractable = handGrabChild.AddComponent<CustomHandGrabInteractable>();
    GrabInteractable grabInteractable = handGrabChild.AddComponent<GrabInteractable>();

    // Set Rigidbody reference in HandGrabInteractable and GrabInteractable
    handGrabInteractable.InjectRigidbody(rb);
    grabInteractable.InjectRigidbody(rb);

    // Set Pointable Element for GrabInteractable
    grabInteractable.InjectOptionalPointableElement(grabbable);

    // Set Hand Alignment for HandGrabInteractable to None (if applicable)
    handGrabInteractable.HandAlignment = HandAlignType.None;

    Debug.Log("Hand Grab setup applied successfully.");
}



}