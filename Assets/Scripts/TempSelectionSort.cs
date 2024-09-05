using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TempSelectionSort : MonoBehaviour
{
    public SelectionSort selectionSort;
    public Block currentBlock;
    public bool blockPlaced = false;
    public Material validMaterial;
    public Material invalidMaterial;
    public Material defaultMaterial;
    public Material pointerMaterial;
    
    private void OnTriggerEnter(Collider other)
    {
        currentBlock = other.GetComponent<Block>();
        // Check if the object entering the trigger is a placeable object
        if (other.CompareTag("codeBlock") && !blockPlaced)
        {
            // Get the XRGrabInteractable component
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();

            // Check if the object is not being held
            if (!grabInteractable.isSelected)
            {
                // Snap the object to the position of the placement zone
                other.transform.position = transform.position;
                other.transform.rotation = Quaternion.identity; // Optional: Reset rotation if needed
                blockPlaced = true;
                updateBlockColour(selectionSort.isValidTemp(currentBlock.blockNum));
                if (selectionSort.isValidTemp(currentBlock.blockNum)) {
                    selectionSort.DisableBlocks();
                }
                else {
                    selectionSort.DisableAllBlocks();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        currentBlock = other.GetComponent<Block>();
        // Check if the object entering the trigger is a placeable object
        if (other.CompareTag("codeBlock") && !blockPlaced)
        {
            // Get the XRGrabInteractable component
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();

            // Check if the object is not being held
            if (!grabInteractable.isSelected)
            {
                // Snap the object to the position of the placement zone
                other.transform.position = transform.position;
                other.transform.rotation = Quaternion.identity; // Optional: Reset rotation if needed
                blockPlaced = true;
                updateBlockColour(selectionSort.isValidTemp(currentBlock.blockNum));
                if (selectionSort.isValidTemp(currentBlock.blockNum)) {
                    selectionSort.DisableBlocks();
                }
                else {
                    selectionSort.DisableAllBlocks();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is the current block
        if (other.CompareTag("codeBlock"))
        {
            blockPlaced = false;
            if (!currentBlock.isSorted) {
                resetBlockColour();
            }
        }
        selectionSort.EnableBlocks();
    }

    public void updateBlockColour (bool isValid) {
        Renderer renderer = currentBlock.GetComponent<Renderer>();
        if (isValid) {
            renderer.material = validMaterial;
            currentBlock.isSorted = true;
        }
        else {
            renderer.material = invalidMaterial;
            currentBlock.isSorted = false;
        }
    }

    public void resetBlockColour () {
        Renderer renderer = currentBlock.GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
}
