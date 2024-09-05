using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortManager : MonoBehaviour
{
    public CodePlacement[] blockPlacement;
    public TempPlacement tempPlacement;
    public TrayManager trayManager;
    public Material currentMaterial;
    public Material defaultMaterial;
    public int currentIndex = 0;
    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        if (blockPlacement.Length > 0)
        {
            // Initialize the pointer on the first block
            //updateBlockColour(currentIndex);
        }
    }

    // Method to move the pointer to the next block when the button is pressed
    public void MovePointer()
    {
        //Debug.Log("Move Pointer at index :"+currentIndex);
        // Reset the color of the current block to its original color
        resetBlockColour(currentIndex);

        // Increment the currentIndex to move the pointer to the next block
        if (tempPlacement.isValid && !tempPlacement.blockPlaced && trayManager.canMovePointer()) {
            currentIndex++;
        }

        //If we've reached the end of the list, loop back to the beginning
        if (currentIndex >= blockPlacement.Length)
        {
            Debug.Log("Array is Sorted");
        }

        // Highlight the new current block
        updateBlockColour(currentIndex);
    }

    // Method to update the color of the block at the specified index
    public void updateBlockColour(int index)
    {
        if (blockPlacement[index].blockPlaced)
        {
            renderer = blockPlacement[index].currentBlock.GetComponent<Renderer>();
            renderer.material = currentMaterial;
            blockPlacement[index].currentBlock.DisableGrabInteractable();
        }
    }

    // Method to reset the color of the block at the specified index
    public void resetBlockColour(int index)
    {
        if (blockPlacement[index].blockPlaced)
        {
            renderer = blockPlacement[index].currentBlock.GetComponent<Renderer>();
            renderer.material = defaultMaterial;
            blockPlacement[index].currentBlock.EnableGrabInteractable();
        }
    }
}
