using StellarFactor;
using StellarFactor.Global;
using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject blockingWall;

    [Header("UI Settings")]
    [SerializeField] private string bossName;
    [SerializeField] private string actionToPrompt;

    [Header("QuestionSettings")]
    [SerializeField] private QuestionSO definiteQuestion;

    [Header("Serialized Events")]
    [SerializeField] private UnityEvent OnPlayerEnter;
    [SerializeField] private UnityEvent OnInteract;
    [SerializeField] private UnityEvent OnPlayerExit;
    [SerializeField] private UnityEvent OnBossDefeated;

    [Header("VFX")]
    [SerializeField] private GameObject particleEffect;

    private PlayerControl player;
    private bool wasRecentAttemptCorrect;

    private bool IsPlayerHere => player != null;

    public string BossName => bossName;
    public QuestionSO Question => definiteQuestion;
    public bool BeenVisited { get; private set; } = false;
    public bool PreviouslyAquired { get; private set; } = false;
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

        wasRecentAttemptCorrect = answeredCorrectly;

        if (!wasRecentAttemptCorrect)
        {
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

        if (wasRecentAttemptCorrect)
        {
            // KILL BOSS whatever this might mean.
            OnBossDefeated.Invoke();
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

        //// If question is null get a question, else don't
        //question = (question != null) ? question : GetQuestion();

        GameManager.MGR.RequestInteractionPrompt(ActionToPrompt);

        OnPlayerEnter?.Invoke();
    }

    public void Interact()
    {
        GameManager.MGR.RequestCloseInteractionPrompt();

        if (!PreviouslyAquired)
        {
            Question.QuestionGivenBy = QuestionGivenBy.BOSS;
            QuestionManager.MGR.StartQuestion(Question);
        }

        OnInteract?.Invoke();
    }

    public void PlayerExitRange(PlayerControl player)
    {
        if (this.player == player)
        {
            this.player = null;
        }

        GameManager.MGR.RequestCloseInteractionPrompt();

        OnPlayerExit?.Invoke();
    }
    #endregion // IInteractable Implementation =============================
}
