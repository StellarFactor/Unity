using System;
using UnityEngine;

namespace StellarFactor
{
    public class InteractionChecker : MonoBehaviour
    {
        private IInteractable _currentInteraction;

        private bool canInteract;

        private void OnEnable()
        {
            GameManager.MGR.Pause += HandlePause;
            GameManager.MGR.Resume += HandleResume;
        }

        private void OnDisable()
        {
            GameManager.MGR.Pause -= HandlePause;
            GameManager.MGR.Resume -= HandleResume;
        }

        private void Start()
        {
            canInteract = true;
        }

        private void Update()
        {
            if (!canInteract) { return; }

            if (Input.GetKeyDown(GameManager.MGR.InteractKey))
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

        private void HandlePause()
        {
            canInteract = false;
        }

        private void HandleResume()
        {
            canInteract = true;
        }
    }
}