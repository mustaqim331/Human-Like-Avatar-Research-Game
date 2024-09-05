using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Block : MonoBehaviour
{
    public int blockNum;
    public bool isSorted = false;
    public TMP_Text blockText;
    private XRGrabInteractable grabInteractable;
    public Material defaultColour;
    public bool isPointer = false;

    void Start () {
        blockText.text = blockNum.ToString();
        // Get the XRGrabInteractable component attached to this object
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void updatePosition (Vector3 pos) {
        transform.position = pos;
    }

    public void EnableGrabInteractable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }
    }

    public void DisableGrabInteractable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }
    }

    public void resetBlockColour () {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = defaultColour;
    }
}
