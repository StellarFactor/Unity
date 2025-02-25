using UnityEngine;
using UnityEngine.Events;

namespace StellarFactor
{
    public class TriggerBox : MonoBehaviour, IInteractable
    {
        [SerializeField] private UnityEvent OnPlayerEnter;
        [SerializeField] private UnityEvent OnPlayerExit;
        [SerializeField] private UnityEvent OnInteract;

        public void Interact()
        {
            OnInteract?.Invoke();
        }

        public void PlayerEnterRange()
        {
            OnPlayerEnter?.Invoke();
        }

        public void PlayerExitRange()
        {
            OnPlayerExit?.Invoke();
        }
    }
}