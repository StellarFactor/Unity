using StellarFactor.Global;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace StellarFactor
{
    public class Artifact : MonoBehaviour, IInteractable, IAcquirable
    {
        [Header("UI Settings")]
        [SerializeField] private string artifactName;
        [SerializeField] private string actionToPrompt;
        [SerializeField] private string actionToPromptAfterAquired;

        [Header("QuestionSettings")]
        [SerializeField] private Difficulty _difficulty;
        [SerializeField] private int _index;

        [Header("Serialized Events")]
        [SerializeField] private UnityEvent OnPlayerEnter;
        [SerializeField] private UnityEvent OnInteract;
        [SerializeField] private UnityEvent OnPlayerExit;

        [Header("VFX")]
        [SerializeField] private GameObject particleEffect;

        private QuestionSO question;
        private PlayerControl player;
        private bool wasRecentAttemptCorrect;

        [field: SerializeField] public bool CanStack { get; private set; }
        public Inventory StoredIn { get; private set; }

        private bool IsPlayerHere => player != null;

        public string ArtifactName => artifactName;
        public Difficulty Difficulty => _difficulty;
        public int Index => _index;
        public QuestionSO Question
        {
            get { return question; }
            set
            {
                Debug.Log($"{name}'s Question is being set externally.");
                question = value;
            }
        }
        public bool BeenVisited { get; private set; } = false;
        public bool PreviouslyAquired { get; private set; } = false;
        public string ActionToPrompt => PreviouslyAquired
            ? actionToPromptAfterAquired
            : actionToPrompt;

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
        private void HandleQuestionAnswered(bool answeredCorrectly, IAcquirable toAcquire)
        {
            if (!IsPlayerHere) { return; }

            wasRecentAttemptCorrect = answeredCorrectly;

            if (!wasRecentAttemptCorrect)
            {
                // TODO
                // Anything we might want in here?
                // A cooldown? (i.e. gotta go try a different one first)

                // Maybe load a new question? ==============================
                // Dr T. said this was too complicated,
                // but I'll leave it here as a comment just in case.

                //_question = QuestionManager.MGR.GetQuestion(_difficulty);
                // =========================================================
            }
        }

        private void HandleQuestionWindowClosed()
        {
            if (!IsPlayerHere) { return; }

            if (!wasRecentAttemptCorrect)// Player got the answer wrong in the window that just closed.
            {
                // But the window doesn't close automatically when the player
                // answers incorrectly, so we know that this is the moment
                // where they've closed the window to come back to it later.
                GameManager.MGR.RequestInteractionPrompt(ActionToPrompt);
            }
        }
        #endregion // ==========================================================


        #region IInteractable Implementation
        // =====================================================================
        public void PlayerEnterRange(PlayerControl player)
        {
            this.player = player;

            GameManager.MGR.RequestInteractionPrompt(ActionToPrompt);

            OnPlayerEnter?.Invoke();
        }

        public void Interact()
        {
            GameManager.MGR.RequestClosePrompt();

            if (!PreviouslyAquired)
            {
                if (Question == null
                    && !QuestionManager.MGR.TryGetQuestion(this, out question))
                {
                    Debug.LogError(
                        $"Something went v wrong in trying to assign" +
                        $"a question to {name}. Its Question is still null.",
                        this);

                    return;
                }

                Question.QuestionGivenBy = QuestionGivenBy.ARTIFACT;
                QuestionManager.MGR.StartQuestion(Question, this);
            }
            else
            {
                player?.Inventory.AcquireItem(this);
            }

            OnInteract?.Invoke();
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


        #region IAcquirable Implementation
        // =====================================================================
        public void OnAcquired(Inventory acquiredBy)
        {
            PreviouslyAquired = true;
            StoredIn = acquiredBy;
            ArtifactInventoryUI.MGR.FillArtifactSlot(this);

            // TODO:
            // animate?
            // anything else?
        }

        void IAcquirable.OnRemoved(Inventory removedFrom, Vector3 dropPos, Vector3 dropEulers)
        {
            if (StoredIn != removedFrom)
            {
                Debug.LogError(
                    $"{removedFrom.name} is attempting to remove {name}, " + 
                    $"but {name} is stored by {StoredIn}.");
            }

            transform.position = dropPos;
            transform.localEulerAngles = dropEulers;
            StoredIn = null;
            ArtifactInventoryUI.MGR.EmptyArtifactSlot(this);
        }
        #endregion // ==========================================================

        public void Visit()
        {
            BeenVisited = true;
        }
    }
}
