using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name} hit the laser and respawned!");
            other.transform.position = respawnPoint.position;
        }
    }
}
