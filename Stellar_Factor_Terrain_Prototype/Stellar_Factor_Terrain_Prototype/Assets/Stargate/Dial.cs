using UnityEngine;

namespace StellarFactor
{
    public class Dial : MonoBehaviour, IToggleable, IInteractable
    {
        [Header("External Refs")]
        [SerializeField] private Stargate stargate;

        [Header("Settings")]
        [SerializeField] private string actionToPromptWhenOn;
        [SerializeField] private string actionToPromptWhenOff;
        [SerializeField] private bool mustEnableExternally;

        private string actionToPrompt => IsOn
            ? actionToPromptWhenOn
            : actionToPromptWhenOff;

        public bool InteractionEnabled { get; private set; }

        public bool IsOn { get; private set; }

        private void Start()
        {
            InteractionEnabled = !mustEnableExternally;
        }

        public void PlayerEnterRange(PlayerControl player)
        {
            if (!InteractionEnabled) { return; }

            GameManager.MGR.RequestInteractionPrompt(actionToPrompt);
        }

        public void Interact()
        {
            if (!InteractionEnabled) { return; }

            Toggle();
        }

        public void PlayerExitRange(PlayerControl player)
        {
            if (!InteractionEnabled) { return; }

            GameManager.MGR.RequestClosePrompt();
        }

        public void TurnOff()
        {
            IsOn = false;
            GameManager.MGR.RequestClosePrompt();
            GameManager.MGR.RequestInteractionPrompt(actionToPrompt);
            stargate.TurnOff();
        }

        public void TurnOn()
        {
            IsOn = true;
            GameManager.MGR.RequestClosePrompt();
            GameManager.MGR.RequestInteractionPrompt(actionToPrompt);
            stargate.TurnOn();
        }

        public void Toggle()
        {
            if (IsOn)
                TurnOff();
            else
                TurnOn();
        }

        public void EnableInteraction(string senderName)
        {
            Debug.Log($"Interaction with {name} is being enabled by {senderName}.");
            InteractionEnabled = true;
        }
    }
}