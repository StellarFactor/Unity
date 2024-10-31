using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace StellarFactor
{
    public class InteractionChecker : MonoBehaviour
    {
        [SerializeField] private KeyCode _interactKey;

        private IInteractable _currentInteraction;


        private void Update()
        {
            if (Input.GetKeyDown(_interactKey))
            {
                _currentInteraction?.Interact();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out _currentInteraction))
            {
                return;
            }

            _currentInteraction.PlayerEnterRange();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out _currentInteraction))
            {
                return;
            }

            _currentInteraction.PlayerExitRange();
            _currentInteraction = null;
        }
    }
}