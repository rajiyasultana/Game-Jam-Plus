using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public float smoothSpeed = 0.1f;
    public Vector3 offset;

    private bool isFollowing = false;

    void LateUpdate()
    {
        // Only follow if the flag is true and we have a target
        if (!isFollowing || target == null)
            return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    // This function needs to be called by the PlayerController
    public void StartFollowing(Transform newTarget, float zoomFactor = 0.5f, float duration = 1f)
    {
        target = newTarget;
        isFollowing = true;
        StartCoroutine(ZoomIn(zoomFactor, duration));
    }

    private IEnumerator ZoomIn(float zoomFactor, float duration)
    {
        Vector3 startOffset = offset;
        Vector3 targetOffset = startOffset * zoomFactor;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            offset = Vector3.Lerp(startOffset, targetOffset, t / duration);
            yield return null;
        }

        offset = targetOffset;
    }
}