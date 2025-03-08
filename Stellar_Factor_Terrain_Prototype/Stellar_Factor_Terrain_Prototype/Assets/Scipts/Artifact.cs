using StellarFactor.Global;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace StellarFactor
{
    public class Artifact : MonoBehaviour, IInteractable
    {
        [SerializeField] private Difficulty _difficulty;
        [SerializeField] private int _index;

        [SerializeField] private UnityEvent OnPlayerEnter;
        [SerializeField] private UnityEvent OnInteract;
        [SerializeField] private UnityEvent OnPlayerExit;

        [SerializeField] private GameObject particleEffect;

        [Header("Prompt")]
        [SerializeField] private string actionToPrompt;

        private QuestionSO _question;
        private bool isPlayerHere;
        private bool wasPreviousAttemptCorrect;

        public QuestionSO Question { get { return _question; } }

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

        private void HandleQuestionAnswered(bool answeredCorrectly)
        {
            if (!isPlayerHere) { return; }

            wasPreviousAttemptCorrect = answeredCorrectly;

            // Anything we might want in here if wrong answer?
            // A cooldown? (i.e. gotta go try a different one first)

            // Maybe load a new question? ==============================
            // Dr T. said this was too complicated,
            // but I'll leave it here as a comment just in case.

            //_question = QuestionManager.MGR.GetQuestion(_difficulty);
            // =========================================================
        }

        private void HandleQuestionWindowClosed()
        {
            if (!isPlayerHere) { return; }

            if (wasPreviousAttemptCorrect)
            {
                Destroy(gameObject);
                // TODO:
                // animate?
                // anything else?

                //// When a particle system gets assigned to the artifact, itll stop and destroy itself
                //// once the question is answered correctly.
                //if (particleEffect != null)
                //{
                //    ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
                //    if (ps != null)
                //    {
                //        ps.Stop();
                //    }
                //}
            }
            // Got it wrong last time, so we know player is
            // closing window to come back to it later.
            else
            {
                GameManager.MGR.RequestInteractionPrompt(actionToPrompt);
            }
        }

        public void PlayerEnterRange()
        {
            isPlayerHere = true;

            // If question is null get a question, else don't
            _question = (_question != null) ? _question : GetQuestion();

            GameManager.MGR.RequestInteractionPrompt(actionToPrompt);

            OnPlayerEnter?.Invoke();
        }

        public void Interact()
        {
            GameManager.MGR.RequestCloseInteractionPrompt();

            GameManager.MGR.StartArtifactInteraction(this);

            OnInteract?.Invoke();
        }

        public void PlayerExitRange()
        {
            isPlayerHere = false;

            GameManager.MGR.RequestCloseInteractionPrompt();

            OnPlayerExit?.Invoke();
        }

        private QuestionSO GetQuestion()
        {
            switch (QuestionManager.MGR.QuestionLoadOrder)
            {
                default:
                case QuestionLoadOrder.RANDOM:
                    return QuestionManager.MGR.GetQuestion(_difficulty);

                case QuestionLoadOrder.INSPECTOR_INDEX:
                    return QuestionManager.MGR.GetQuestion(_difficulty, _index);

                case QuestionLoadOrder.INTERACTION_ORDER:
                    return QuestionManager.MGR.GetNextQuestion(_difficulty);
            }
        }
    }
}
