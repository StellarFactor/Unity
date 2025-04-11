using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace StellarFactor
{
    public class Pedestal : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameObject artifact;

        [Header("UI Settings")]
        [SerializeField] private string exactNameOfArtifact;
        [SerializeField] private string actionToPrompt;

        [Header("QuestionSettings")]
        [SerializeField] private QuestionSO definiteQuestion;

        [Header("Serialized Events")]
        [SerializeField] private UnityEvent OnPlayerEnter;
        [SerializeField] private UnityEvent OnInteract;
        [SerializeField] private UnityEvent OnPlayerExit;
        [SerializeField] private UnityEvent OnPedestalDefeated;

        [Header("VFX")]
        [SerializeField] private GameObject particleEffect;


        private PlayerControl player;
        private bool wasRecentAttemptCorrect;

        private bool IsPlayerHere => player != null;


        public string PedestalName => exactNameOfArtifact;
        public QuestionSO Question => definiteQuestion;
        public bool BeenVisited { get; private set; } = false;
        public bool ArtifactPlaced => artifact != null;
        public string ActionToPrompt => actionToPrompt;


        private void Awake()
        {
            BeenVisited = false;
        }

        private void OnEnable()
        {
            QuestionManager.MGR.QuestionAnswered += HandleQuestionAnswered;
            QuestionManager.MGR.WindowClosed += HandleQuestionWindowClosed;
        }

        private void OnDisable()
        {
            QuestionManager.MGR.QuestionAnswered -= HandleQuestionAnswered;
            QuestionManager.MGR.WindowClosed -= HandleQuestionWindowClosed;
        }


        #region Event Responses
        // =====================================================================
        private void HandleQuestionAnswered(bool answeredCorrectly)
        {
            if (!IsPlayerHere) { return; }
            if (wasRecentAttemptCorrect) { return; }

            wasRecentAttemptCorrect = answeredCorrectly;
        }

        private void HandleQuestionWindowClosed()
        {
            if (!IsPlayerHere) { return; }

            if (wasRecentAttemptCorrect)
            {
                PedestalManager.MGR.CompletePedestal(this);
                // TODOOOOOO if (!player.Inventory.ContainsItem()) {}

                // Remove artifact from player
                // Place on pedestal
                OnPedestalDefeated.Invoke();
            }
            // Got it wrong last time, so we know player is
            // closing window to come back to it later.
            else
            {
                GameManager.MGR.RequestInteractionPrompt(ActionToPrompt);
            }
        }
        #endregion // ==========================================================


        #region IInteractable Implementation
        // =====================================================================
        public void PlayerEnterRange(PlayerControl player)
        {
            this.player = player;

            string toSend = ArtifactPlaced ? exactNameOfArtifact : ActionToPrompt;
            GameManager.MGR.RequestInteractionPrompt(toSend);

            OnPlayerEnter?.Invoke();
        }

        public void Interact()
        {
            GameManager.MGR.RequestClosePrompt();
            // Assert.IsTrue(player.Inventory.ContainsArtifact(exactNameOfArtifact));

            if (!ArtifactPlaced)
            {
                Question.QuestionGivenBy = QuestionGivenBy.PEDESTAL;
                QuestionManager.MGR.StartQuestion(Question);

                OnInteract?.Invoke();
            }
        }

        public void PlayerExitRange(PlayerControl player)
        {
            if (this.player == player)
            {
                this.player = null;
            }

            GameManager.MGR.RequestClosePrompt();

            OnPlayerExit?.Invoke();
        }
        #endregion // IInteractable Implementation =============================
    }
}
