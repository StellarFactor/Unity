using StellarFactor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour, IInteractable
{
    [Header("Internal Refs")]
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject blockingWall;

    [Header("General Settings")]
    [SerializeField] private bool startEnabled;

    [Header("UI Settings")]
    [SerializeField] private string bossName;
    [SerializeField] private string actionToPrompt;

    [Header("QuestionSettings")]
    [SerializeField] private QuestionSO definiteQuestion;

    [Header("Serialized Events")]
    [SerializeField] private UnityEvent OnPlayerEnter;
    [SerializeField] private UnityEvent OnFadeInComplete;
    [SerializeField] private UnityEvent OnFadeOutComplete;
    [SerializeField] private UnityEvent OnInteract;
    [SerializeField] private UnityEvent OnPlayerExit;
    [SerializeField] private UnityEvent OnBossDefeated;

    [Header("VFX")]
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private float fadeSpeed;

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

    private void Start()
    {
        if (!startEnabled)
        {
            ExitPlay(false);
        }
    }

    private IEnumerator FadeRend(Renderer rend, bool fadeIn)
    {
        Color startingColor = rend.material.color;
        float startingAlpha = startingColor.a;
        float targetAlpha   = fadeIn ? 0 : 1;
        float totalDistance = targetAlpha - startingAlpha;
        float currentAlpha = startingAlpha;
        do
        {
            float distanceCovered = Mathf.Abs(currentAlpha - startingAlpha);
            float t = 1 - (distanceCovered) / (totalDistance);
            currentAlpha = Mathf.Lerp(startingAlpha, targetAlpha, t) * fadeSpeed;

            Color newColor = new ( startingColor.r, startingColor.g, startingColor.b, currentAlpha);
            rend.material.color = newColor;
            yield return null;

        } while (fadeIn
                ? (currentAlpha > targetAlpha)
                : (currentAlpha < targetAlpha));
    }

    public void EnterPlay(bool withFade)
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in rends)
        {
            if (withFade)
            {
                StartCoroutine(FadeRend(rend, true));
            }
            else
            {
                rend.enabled = false;
            }
        }

        particleEffect.SetActive(true);
    }

    public void ExitPlay(bool withFade)
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in rends)
        {
            rend.enabled = true;
            if (withFade)
            {
                StartCoroutine(FadeRend(rend, false));
            }
        }

        particleEffect.SetActive(false);
    }


    #region Event Responses
    // =====================================================================
    private void HandleQuestionAnswered(bool answeredCorrectly, IAcquirable toAcquire)
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
        GameManager.MGR.RequestClosePrompt();

        if (!PreviouslyAquired)
        {
            Question.QuestionGivenBy = QuestionGivenBy.BOSS;
            QuestionManager.MGR.StartQuestion(Question, null);
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
}
