using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class SelectionSort : MonoBehaviour
{
    public GameManager gameManager;
    public CodePlacement [] blockPlacements = {};
    public Pointer [] pointers = {};
    public TempSelectionSort tempSlot;
    public int [] currentArray;
    public int currentIndex = -1;
    public Material currentPointerColour;
    public Material defaultPointerColour;
    public Block previousBlock = null;
    public TMP_Text errorMessage;
    public GameObject reference;
    public GameObject end;
    public bool arrayIsSorted = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateCurrentArray();
    }

    public bool isValidTemp (int tempBlockNum) {
        int smallestNum = tempBlockNum;
        for (int i=currentIndex; i<currentArray.Length; i++) {
            if (currentArray[i] > 0 && currentArray[i]<smallestNum) {
                smallestNum = currentArray[i];
            }
        }
        return tempBlockNum == smallestNum;
    }

    public void updateCurrentArray () {
        for (int i = 0; i < blockPlacements.Length; i++) {
            if (blockPlacements[i].blockPlaced) {
                currentArray[i] = blockPlacements[i].currentBlock.blockNum;
            }
            else {
                currentArray[i] = -1;
            }
        }
    }

public void movePointer() 
{
    if (!arrayIsSorted) {
        // Condition 1: Do not move the pointer if there is a block in the temp slot
        if (tempSlot.blockPlaced) 
        {
            Debug.Log("Cannot move pointer: Block is placed in temp slot");
            errorMessage.text = "Make sure there is no block in temp slot!!";
            return;
        }

        // Condition 2: Do not move the pointer if the previous element is not sorted
        if (currentIndex >= 0 && !blockPlacements[currentIndex].currentBlock.isSorted) 
        {
            Debug.Log("Cannot move pointer: Previous block is not sorted yet");
            errorMessage.text = "Find the smallest element first!!";
            return;
        }

        // Proceed with moving the pointer
        if (currentIndex >= 0) 
        {
            // Lock the sorted array
            blockPlacements[currentIndex].currentBlock.DisableGrabInteractable();
            resetPointerColour(currentIndex);
        }

        if (currentIndex == currentArray.Length - 1) 
        {
            Debug.Log("Array is Sorted!!");
            errorMessage.text = "Array is Sorted!!";
            reference.SetActive(false);
            end.SetActive(true);
            arrayIsSorted = true;
            return;
        }

        // Reset the color of the previous pointer block, if any
            if (previousBlock != null && !previousBlock.isSorted)
            {
                previousBlock.resetBlockColour();
                previousBlock.isPointer = false;
            }

        // Move the pointer to the next index
        currentIndex++;
        previousBlock = blockPlacements[currentIndex].currentBlock;
        blockPlacements[currentIndex].currentBlock.isPointer = true;
        updatePointerColour(currentIndex);
    }
}


    public void updatePointerColour (int currentIndex) {
        Renderer renderer = pointers[currentIndex].GetComponent<Renderer>();
        renderer.material = currentPointerColour;
    }

    public void resetPointerColour (int currentIndex) {
        Renderer renderer = pointers[currentIndex].GetComponent<Renderer>();
        renderer.material = defaultPointerColour;
    }

    public void DisableBlocks()
    {
        foreach (CodePlacement placement in blockPlacements)
        {
            if (!placement.currentBlock.isSorted && !placement.currentBlock.isPointer && placement.currentBlock != tempSlot.currentBlock)
            {
                placement.currentBlock.DisableGrabInteractable();
            }
        }
    }

    public void DisableAllBlocks()
    {
        foreach (CodePlacement placement in blockPlacements)
        {
            if (placement.currentBlock != tempSlot.currentBlock)
            {
                placement.currentBlock.DisableGrabInteractable();
            }
        }
    }    

    public void EnableBlocks()
    {
        foreach (CodePlacement placement in blockPlacements)
        {
            if (!placement.currentBlock.isSorted)
            {
                placement.currentBlock.EnableGrabInteractable();
            }
        }
    }

    public void loadNextScene (String sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
