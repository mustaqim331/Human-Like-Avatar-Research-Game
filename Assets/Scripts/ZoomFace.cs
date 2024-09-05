using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ZoomFace : MonoBehaviour
{
    public Camera cam;
    public Vector3 targetOffset;
    public Vector3 originalPosition;
    public float speed = 1.0f;
    public Transform facePosition;

    private bool isZooming = false;

    void Start()
    {
        // Initialize the original position with the camera's starting position
        originalPosition = cam.transform.position;
    }

    public void ZoomIn()
    {
        if (!isZooming)
        {
            Vector3 targetPosition = new Vector3(
                targetOffset.x,
                facePosition.position.y,
                targetOffset.z
            );
            StartCoroutine(SmoothZoom(targetPosition));
        }
    }

    public void ZoomOut()
    {
        if (!isZooming)
        {
            StartCoroutine(SmoothZoom(originalPosition));
        }
    }

    private IEnumerator SmoothZoom(Vector3 target)
    {
        isZooming = true;
        Vector3 startPosition = cam.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            cam.transform.position = Vector3.Lerp(startPosition, target, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        cam.transform.position = target; // Ensure the camera reaches the exact target position
        isZooming = false;
    }
}


