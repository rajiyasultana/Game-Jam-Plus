using UnityEngine;

public class CameraPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraFollow camScript = Camera.main.GetComponent<CameraFollow>();
            if (camScript != null)
            {
                // Start following the player and zoom in
                camScript.StartFollowing(other.transform, 0.5f, 1f); // 50% zoom
            }

            Destroy(gameObject);
        }
    }
}