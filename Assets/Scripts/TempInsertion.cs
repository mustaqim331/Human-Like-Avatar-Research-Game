using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TempInsertion : MonoBehaviour
{
    public Block currentBlock;
    public bool blockPlaced = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is the current block
        if (other.CompareTag("codeBlock"))
        {
            blockPlaced = false;
        }
    }
}
