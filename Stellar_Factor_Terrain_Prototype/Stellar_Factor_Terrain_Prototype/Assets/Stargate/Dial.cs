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

        private string actionToPrompt => IsOn
            ? actionToPromptWhenOn
            : actionToPromptWhenOff;

        public bool IsOn { get; private set; }

        public void Interact()
        {
            Toggle();
        }

        public void PlayerEnterRange()
        {
            GameManager.MGR.RequestInteractionPrompt(actionToPrompt);
        }

        public void PlayerExitRange()
        {
            GameManager.MGR.RequestCloseInteractionPrompt();
        }

        public void TurnOff()
        {
            IsOn = false;
            stargate.TurnOff();
        }

        public void TurnOn()
        {
            IsOn = true;
            stargate.TurnOn();
        }

        public void Toggle()
        {
            if (IsOn)
                TurnOff();
            else
                TurnOn();
        }
    }
}