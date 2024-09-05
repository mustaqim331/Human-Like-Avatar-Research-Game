using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Button : UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable
{
    private Renderer buttonRenderer;
    public Material pressedColour;
    public Material releasedColour;
    internal object onClick;

    protected override void Awake()
    {
        base.Awake();
        buttonRenderer = GetComponent<Renderer>();
    }

    // This method is called when the interactor first selects the button
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        //Debug.Log("Button Pressed!");

        // Change the color of the button when selected
        buttonRenderer.material = pressedColour;
        UpdateYPosition(-0.010f);
    }

    // This method is called when the interactor exits the selection of the button
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Reset the color of the button when deselected
        buttonRenderer.material = releasedColour;
        UpdateYPosition(0.010f);
    }

    private void UpdateYPosition(float yOffset)
    {
        Vector3 newPosition = transform.position;
        newPosition.y += yOffset;
        transform.position = newPosition;
    }
}
