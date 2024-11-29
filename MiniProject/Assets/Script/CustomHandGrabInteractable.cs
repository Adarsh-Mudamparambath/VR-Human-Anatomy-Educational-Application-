using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab; // Ensure to include this
using Oculus.Interaction; // Ensure to include this for IInteractorView
using UnityEngine.Events;
public class CustomHandGrabInteractable : HandGrabInteractable
{
    public UnityEvent<Transform> OnObjectGrabbed;
    protected override void Awake()
    {
        base.Awake();
        // Subscribe to the event that gets triggered when the object is grabbed
        WhenSelectingInteractorAdded.Action += OnGrabbed;
    }

    private void OnGrabbed(IInteractorView interactor)
    {
        // Custom event logic goes here
        Debug.Log("Object has been grabbed!");
        // You can also call other methods or trigger custom actions here
        OnObjectGrabbed?.Invoke(transform.parent);
    }

    // Regular OnDestroy method without override
    private void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        WhenSelectingInteractorAdded.Action -= OnGrabbed;
    }
}
