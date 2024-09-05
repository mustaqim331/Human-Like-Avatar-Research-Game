using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TempPlacement : MonoBehaviour
{
    public Block currentBlock;
    public bool blockPlaced = false;
    public Material validMaterial;
    public Material invalidMaterial;
    public Material defaultMaterial;
    public GameManager gameManager;
    //public SelectionSort selectionSort;
    public bool isValid = false;
    
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
                if (gameManager.currentGameState == GameManager.GameState.SelectionSort) {
                    //isValid = selectionSort.validatateBlock(currentBlock);
                    updateBlockColour(isValid);
                }
                //updateBlockColour();

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
                if (gameManager.currentGameState == GameManager.GameState.SelectionSort) {
                    //isValid = selectionSort.validatateBlock(currentBlock);
                    updateBlockColour(isValid);
                }
                //updateBlockColour();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is the current block
        if (other.CompareTag("codeBlock"))
        {
            blockPlaced = false;
            //currentBlock = null;
            resetBlockColour();
            //Debug.Log("Block exited placement slot");
        }
    }

    public void updateBlockColour (bool isValid) {
        Renderer renderer = currentBlock.GetComponent<Renderer>();
        if (isValid) {
            renderer.material = validMaterial;
        }
        else {
            renderer.material = invalidMaterial;
        }
    }

    public void resetBlockColour () {
        Renderer renderer = currentBlock.GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }
}
