using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class TrayManager : MonoBehaviour
{
    public int[] unsortedArr;
    public CodePlacement[] blockPlacements = {};

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < blockPlacements.Length; i++) {
            if (blockPlacements[i].blockPlaced) {
                unsortedArr[i] = blockPlacements[i].currentBlock.blockNum;
            }
            else {
                unsortedArr[i] = -1;
            }
        }
    }

    public bool canMovePointer() {
        foreach (int i in unsortedArr) {
            if (i == -1) {
                return false;
            }
        }
        return true;
    }
}
