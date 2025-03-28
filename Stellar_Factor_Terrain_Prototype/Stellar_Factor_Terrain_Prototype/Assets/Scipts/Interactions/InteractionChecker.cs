using System;
using UnityEngine;

namespace StellarFactor
{
    public class InteractionChecker : MonoBehaviour
    {
        private IInteractable _currentInteraction;

        private bool canInteract;

        private PlayerControl player;

        private void OnEnable()
        {
            GameManager.MGR.GamePaused += HandlePause;
            GameManager.MGR.GameResumed += HandleResume;
        }

        private void OnDisable()
        {
            GameManager.MGR.GamePaused -= HandlePause;
            GameManager.MGR.GameResumed -= HandleResume;
        }

        private void Start()
        {
            canInteract = true;

            if (!TryGetComponent(out player))
            {
                Debug.LogError($"{name} should be attached to a(the) " +
                    $"GameObject with a(the) PlayerControl Component.");
            }
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

            _currentInteraction.PlayerEnterRange(player);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out _currentInteraction))
            {
                return;
            }

            _currentInteraction.PlayerExitRange(player);
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