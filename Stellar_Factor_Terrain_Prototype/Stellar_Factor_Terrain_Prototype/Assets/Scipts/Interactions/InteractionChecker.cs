using System;
using UnityEngine;

namespace StellarFactor
{
    public class InteractionChecker : MonoBehaviour
    {
        [SerializeField] private DbugLog log;

        private IInteractable currentInteraction;

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
                log.Error($"{name} should be attached to a(the) " +
                    $"GameObject with a(the) PlayerControl Component.");
            }
        }

        private void Update()
        {
            if (!canInteract) { return; }

            if (Input.GetKeyDown(GameManager.MGR.InteractKey))
            {
                currentInteraction?.Interact();
            }

            // This is a bug fix. OnTriggerExit is only being called when the
            // IInteractable object
            // a) Remains active and enabled when interaction is done
            // b) Destroys itself when interaction is done.
            // 
            // So in cases where the IInteractable object disables itself,
            // rather than doing one of the above, OnTriggerExit was not being
            // called. This ensures that currentInteraction is being properly
            // cleared if its gameObject gets set inactive or the component
            // that implements it gets disabled.
            if (currentInteraction is MonoBehaviour interactableMono
                && !interactableMono.isActiveAndEnabled)
            {
                log.Print(
                    $"{name} stopped interacting with a MonoBehaviour that " +
                    $"implements IInteractable ({interactableMono.name}) " +
                    $"without exiting the collider.",
                    interactableMono);
                currentInteraction.PlayerExitRange(player);
                currentInteraction = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IInteractable interactable))
            {
                return;
            }

            log.Print(
                $"{name} entered the collider of a {interactable.GetType()}-type" +
                $" IInteractable object");
            if (currentInteraction != null
                && interactable != currentInteraction)
            {
                currentInteraction.PlayerExitRange(player);
            }

            currentInteraction = interactable;
            currentInteraction.PlayerEnterRange(player);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out IInteractable interactable))
            {
                return;
            }

            log.Print(
                $"{name} exited the collider of a {interactable.GetType()}-type" +
                $" IInteractable object");

            interactable.PlayerExitRange(player);

            if (interactable == currentInteraction)
            {
                currentInteraction = null;
            }
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