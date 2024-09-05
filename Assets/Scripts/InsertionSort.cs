using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class InsertionSort : MonoBehaviour
{
    public CodePlacement [] blockPlacement = {};
    public Pointer [] pointers = {};
    public TempInsertion tempSlot;
    public Material currentPointerColour;
    public Material defaultPointerColour;
    public Material sortedColour;
    public Material incorrectColour;
    public int currentIndex = 0;
    public bool isSorted = false;
    public int [] numArray = {};
    public TMP_Text errorMessage;
    public GameObject reference;
    public GameObject end;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //checkBlockPlacement(tempSlot.currentBlock);
    }

    public void updateArray () {
        for (int i=0; i<blockPlacement.Length; i++) {
            numArray[i] = blockPlacement[i].currentBlock.blockNum;
        }
    }

    public bool canMove (Block tempBlock) {
        return getBlockIndex(tempBlock) == findCorrectIndex(tempBlock);
    }

    public int getBlockIndex (Block tempBlock) {
        int index = currentIndex;
        for (int i=0; i<blockPlacement.Length; i++) {
            if (blockPlacement[i].currentBlock.blockNum == tempBlock.blockNum) {
                index = blockPlacement[i].slotIndex;
            }
        }
        return index;
    }

    public int findCorrectIndex(Block tempBlock) {//block 4
        int index = currentIndex;//3
        int correctIndex = index;//3

        // Iterate from the beginning of the array up to the current index
        for (int i = 0; i <= index; i++) 
        {
            // Find the correct position where the current block should be placed
            if (numArray[i] > tempBlock.blockNum) 
            {
                correctIndex = i;
                break;
            }
        }

        return correctIndex;
    }

    public void checkBlockPlacement (Block tempBlock) {
        if (currentIndex > 0 && getBlockIndex(tempBlock) != findCorrectIndex(tempBlock)) {
            updateIncorrectcolour(tempBlock);
        }
        else {
            updateSortedColour(tempBlock);
        }
    }


    public void movePointer () {
        if (currentIndex == 0) {
            tempSlot.currentBlock = blockPlacement[currentIndex].currentBlock;
        }
        
        if (currentIndex<blockPlacement.Length && canMove(tempSlot.currentBlock)) {
            if (tempSlot.blockPlaced) 
            {
                Debug.Log("Cannot move pointer: Block is placed in temp slot");
                errorMessage.text = "Make sure there is no block in temp slot!!";
                return;
            }
            if (currentIndex == blockPlacement.Length - 1 ) {
                updateSortedColour(blockPlacement[currentIndex].currentBlock);
                resetPointerColour(currentIndex);
                Debug.Log("Array is sorted");
                errorMessage.text = "Array is sorted";
                isSorted = true;
                reference.SetActive(false);
                end.SetActive(true);
                //return;
            }
            resetPointerColour(currentIndex);
            blockPlacement[currentIndex].currentBlock.isPointer = false;
            blockPlacement[currentIndex].currentBlock.EnableGrabInteractable();
            updateSortedColour(blockPlacement[currentIndex].currentBlock);
            if (tempSlot.currentBlock != null) {
                updateSortedColour(tempSlot.currentBlock);
            }

            currentIndex++;
            updatePointerColour(currentIndex);
            blockPlacement[currentIndex].currentBlock.isPointer = true;
            blockPlacement[currentIndex].currentBlock.EnableGrabInteractable();
            updateArray();
        }
        else {
            errorMessage.text = "Block is not in the correct position!!";
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

    public void updateSortedColour (Block block) {
        Renderer renderer = block.GetComponent<Renderer>();
        renderer.material = sortedColour;
    }

    public void updateIncorrectcolour (Block block) {
        Renderer renderer = block.GetComponent<Renderer>();
        renderer.material = incorrectColour;
    }

    public void loadNextScene (String sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
