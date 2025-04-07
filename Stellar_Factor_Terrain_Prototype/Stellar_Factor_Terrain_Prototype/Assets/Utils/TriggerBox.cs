using UnityEngine;
using UnityEngine.Events;

namespace StellarFactor
{
    public class TriggerBox : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent OnPlayerEnter;
        [SerializeField] private UnityEvent OnPlayerExit;
        [SerializeField] private UnityEvent OnInteract;

        private PlayerControl player;

        public void PlayerEnterRange(PlayerControl player)
        {
            this.player = player;
            OnPlayerEnter?.Invoke();
        }

        public void Interact()
        {
            OnInteract?.Invoke();
        }

        public void PlayerExitRange(PlayerControl player)
        {
            if (this.player == player)
            {
                this.player = null;
            }
            OnPlayerExit?.Invoke();
        }
    }
}