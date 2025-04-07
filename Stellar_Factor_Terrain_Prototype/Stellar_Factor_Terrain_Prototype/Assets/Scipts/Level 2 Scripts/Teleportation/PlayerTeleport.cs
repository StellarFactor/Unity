using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private Transform currentTeleportPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Teleporter"))
        {
            TeleportDestination teleportDestination = other.GetComponent<TeleportDestination>();
            if (teleportDestination != null)
            {
                TeleportTo(teleportDestination.GetDestination());
            }
        }
    }

    public void TeleportTo(Transform destination)
    {
        if (destination != null)
        {
            transform.position = destination.position;
        }
    }
}
