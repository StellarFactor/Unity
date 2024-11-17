using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaRespawn : MonoBehaviour
{
    private Transform currentCheckpoint; // Tracks the current respawn checkpoint

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player fell into lava
        if (other.CompareTag("Lava"))
        {
            Debug.Log("Player fell in lava.");

            if (currentCheckpoint != null)
            {
                Debug.Log("Teleporting player to checkpoint: " + currentCheckpoint.name);
                Debug.Log("Checkpoint Position: " + currentCheckpoint.position);
                Debug.Log("Player Position Before Teleport: " + other.transform.position);

                // Handle teleportation with Rigidbody
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero; // Stop any current movement
                    rb.angularVelocity = Vector3.zero; // Stop any current rotation
                    rb.isKinematic = true; // Temporarily disable physics
                }

                // Move the player to the checkpoint
                Transform playerTransform = other.transform;
                playerTransform.position = currentCheckpoint.position;
                playerTransform.rotation = currentCheckpoint.rotation; // Optional: Match rotation

                Debug.Log("Player Position After Teleport: " + playerTransform.position);

                if (rb != null)
                {
                    rb.isKinematic = false; // Re-enable physics
                }
            }
            else
            {
                Debug.Log("No checkpoint set. Reloading scene.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // Check if the player entered a checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            currentCheckpoint = other.transform; // Update the checkpoint
            Debug.Log("Checkpoint activated: " + currentCheckpoint.name);
        }
    }
}
