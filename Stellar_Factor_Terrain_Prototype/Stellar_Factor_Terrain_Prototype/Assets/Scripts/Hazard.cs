using UnityEngine;

namespace StellarFactor
{
    public class Hazard : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerControl player))
            {
                return;
            }

            Debug.Log($"{player.name} fell into lava!");
            player.Die(respawnPoint.position);
        }
    }
}