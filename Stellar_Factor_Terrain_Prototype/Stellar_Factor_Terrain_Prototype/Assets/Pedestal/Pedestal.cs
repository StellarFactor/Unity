using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StellarFactor
{
    public class Pedestal : MonoBehaviour, IInteractable
    {
        [Header("UI Settings")]
        [SerializeField] private string exactNameOfArtifact;
        [SerializeField] private string actionToPrompt;

        [Header("QuestionSettings")]
        [SerializeField] private QuestionSO definiteQuestion;

        [Header("PlacementSettings")]
        [SerializeField] private Transform placementPoint;
        [SerializeField] private float yOffset;

        [Header("Serialized Events")]
        [SerializeField] private UnityEvent OnPlayerEnter;
        [SerializeField] private UnityEvent OnInteract;
        [SerializeField] private UnityEvent OnPlayerExit;
        [SerializeField] private UnityEvent OnPedestalDefeated;

        [Header("VFX")]
        [SerializeField] private GameObject particleEffect;


        private PlayerControl player;
        private bool wasRecentAttemptCorrect;
        private Artifact currentArtifact;

        private bool IsPlayerHere => player != null;


        public string PedestalName => exactNameOfArtifact;
        public QuestionSO Question => definiteQuestion;
        public bool BeenVisited { get; private set; } = false;
        public bool ArtifactPlaced => currentArtifact != null;
        public string ActionToPrompt => actionToPrompt;


        private void Awake()
        {
            BeenVisited = false;
            if (placementPoint == null)
            {
                placementPoint = transform;
            }
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

        private void Update()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying
                && currentArtifact != null
                && Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                currentArtifact.transform.position += new Vector3(0f, .25f, 0f);
            }
            else if (EditorApplication.isPlaying
                && currentArtifact != null
                && Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                currentArtifact.transform.position += new Vector3(0f, -.25f, 0f);
            }
#endif
        }

        public void Place(Artifact artifact)
        {
            currentArtifact = artifact;
            currentArtifact.transform.SetParent(transform);
            Vector3 dropPos = placementPoint.position + new Vector3(0f, yOffset, 0f);
            Vector3 dropEulers = placementPoint.localEulerAngles + artifact.transform.localEulerAngles;

            if (!artifact.StoredIn.RemoveItem(artifact, dropPos, dropEulers))
            {
                Debug.LogWarning($"{name} couldn't place {artifact.name}", this);
            }
        }


        #region Event Responses
        // =====================================================================
        private void HandleQuestionAnswered(bool answeredCorrectly, IAcquirable toAcquire)
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

            // TODO:
            // "Artifact" here in the string replace should probably be set to the actual
            // name of the artifact that would be deployed.  Original idea was that each
            // pedestal would selectively determine which artifact to place based on an
            // exact match with the string someArtifact.ArtifactName, but we didn't get to it.
            string toSend = ArtifactPlaced ? exactNameOfArtifact : ActionToPrompt.Replace("<Artifact>", "Artifact");
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
                QuestionManager.MGR.StartQuestion(Question, null);

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
