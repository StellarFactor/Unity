using StellarFactor.Global;
using System;
using UnityEngine;
using UnityStandardAssets.Water;

namespace StellarFactor
{
    public enum QuestionLoadOrder { RANDOM, INSPECTOR_INDEX, INTERACTION_ORDER };

    public class QuestionManager : Singleton<QuestionManager>
    {
        [Header("Settings")]
        [SerializeField] private QuestionLoadOrder questionLoadOrder;
        [SerializeField] private float answerResponseDuration;

        [Header("Question Pools")]
        [SerializeField] private QuestionPool _easy;
        [SerializeField] private QuestionPool _medium;
        [SerializeField] private QuestionPool _hard;

        [Header("External References")]
        [SerializeField] private QuestionPanel questionPanel;
        [SerializeField] private ResponsePanel responsePanel;

        private int currentArtifactInteractionCount;

        public event Action WindowOpened;
        public event Action WindowClosed;
        public event Action WindowReset;
        public event Action<bool> QuestionAnswered; // bool for correctly answered
        public event Action AnsweredCorrectly;
        public event Action AnsweredIncorrectly;

        public QuestionLoadOrder QuestionLoadOrder { get { return questionLoadOrder; } }

        private void OnEnable()
        {
            GameManager.MGR.ArtifactInteractionStarted += HandleArtifactInteractionStarted;
            GameManager.MGR.ArtifactInteractionCanceled += HangleArtifactInteractionCanceled;
        }

        private void OnDisable()
        {
            GameManager.MGR.ArtifactInteractionStarted -= HandleArtifactInteractionStarted;
            GameManager.MGR.ArtifactInteractionCanceled -= HangleArtifactInteractionCanceled;

        }

        private void Start()
        {
            CloseWindow();
        }


        /// <summary>
        /// Gets a random question from the appropriate pool.
        /// If the requested pool doesn't exist or doesnt have
        /// any questions, this function returns null.
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public QuestionSO GetQuestion(Difficulty difficulty)
        {
            Debug.Log($"{name} getting next question via GetQuestion(Difficulty)");
            QuestionPool pool = GetPool(difficulty);

            // Null checks
            if (pool == null) { return null; }
            if (pool.Empty) { return null; }

            return pool.GetRandomQuestion();
        }

        public QuestionSO GetQuestion(Difficulty difficulty, int artifactIndex)
        {
            Debug.Log($"{name} getting next question via GetQuestion(Difficulty, int)");

            QuestionPool pool = GetPool(difficulty);

            // Null checks
            if (pool == null) { return null; }
            if (pool.Empty) { return null; }

            return pool.GetQuestionAt(artifactIndex);
        }

        public QuestionSO GetNextQuestion(Difficulty difficulty)
        {
            Debug.Log($"{name} getting next question via GetNextQuestion(Difficulty)");
            QuestionPool pool = GetPool(difficulty);

            // Null checks
            if (pool == null)
            { Debug.Log($"pool was null"); return null; }
            if (pool.Empty) { Debug.Log($"pool was empty"); return null; }

            return pool.GetQuestionAt(currentArtifactInteractionCount);
        }

        public void OpenWindow()
        {
            questionPanel.Open();

            responsePanel.ResetPanel();
            responsePanel.Close();

            WindowOpened?.Invoke();
        }

        public void CloseWindow()
        {
            questionPanel.ResetPanel();
            questionPanel.Close();


            responsePanel.ResetPanel();
            responsePanel.Close();

            WindowClosed?.Invoke();
        }

        public void ResetWindow()
        {
            questionPanel.ResetPanel();
            questionPanel.Open();

            responsePanel.ResetPanel();
            responsePanel.Close();

            WindowReset?.Invoke();
        }

        public void AnswerQuestion(bool answeredCorrectly)
        {
            Debug.Log($"Answering Question {answeredCorrectly}");

            // Set strategies
            Action responsePanelAction = answeredCorrectly
                ? () => responsePanel.SetCorrect()
                : () => responsePanel.SetIncorrect();

            Action onTimerCompleteAction = answeredCorrectly
                ? () => CloseWindow()
                : () => ResetWindow();

            // Perform Question UI related responses
            questionPanel.HideQuestion();

            responsePanelAction();
            responsePanel.Open();

            // Start a timer
            CountdownTimer timer = new(this, answerResponseDuration);
            timer.Start();

            // Create a process to wait for the timer to finish and then
            // perform the action we set above.
            WaitThenDo waitAndClose = new(
                this,
                () => timer.IsFinished,
                () => timer.BeenCanceled,
                onTimerCompleteAction,
                () => { }
            );

            waitAndClose.Start();

            // Raise the question answered event immediately
            QuestionAnswered?.Invoke(answeredCorrectly);
        }

        private void HandleArtifactInteractionStarted(Artifact artifact)
        {
            if (!artifact.BeenVisited)
            {
                currentArtifactInteractionCount++;
                artifact.Visit();
            }

            OpenWindow();
        }

        private void HangleArtifactInteractionCanceled()
        {
            CloseWindow();
        }

        private QuestionPool GetPool(Difficulty difficulty)
        {
            // Find the right pool.
            QuestionPool pool = difficulty switch
            {
                Difficulty.EASY => _easy,
                Difficulty.MEDIUM => _medium,
                Difficulty.HARD => _hard,
                _ => _medium
            };

            return pool;
        }
    }
}
