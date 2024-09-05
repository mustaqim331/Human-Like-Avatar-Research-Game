using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block[] blocks = {};
    void Start()
    {
        ShuffleBlockPositions();
    }

    void ShuffleBlockPositions()
    {
        // Create a list of the original positions
        List<Vector3> positions = new List<Vector3>();

        foreach (Block block in blocks)
        {
            positions.Add(block.transform.position);  // Store each block's initial position
        }

        // Shuffle the list of positions
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 temp = positions[i];
            int randomIndex = Random.Range(0, positions.Count);
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // Assign the shuffled positions back to the blocks
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].updatePosition(positions[i]);
        }
    }
}
