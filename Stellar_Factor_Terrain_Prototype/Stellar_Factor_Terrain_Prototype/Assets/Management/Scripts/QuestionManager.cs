using StellarFactor.Global;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

        private Dictionary<Difficulty, QuestionPool> questionBank = new();

        private int currentArtifactInteractionCount;

        public event Action WindowOpened;
        public event Action WindowClosed;
        public event Action WindowReset;
        public event Action<Artifact> QuestionStarted;
        public event Action QuestionCanceled;

        /// <summary>
        /// Subscribe to this for any actions that need to be performed as soon
        /// as the QuestionManager reads the answer input.
        /// Distinct from <c><see cref="WindowClosed"/></c>, which will fire
        /// <c><see cref="answerResponseDuration"/></c> seconds after
        /// the answer is read.
        /// </summary>
        public event Action<bool> QuestionAnswered; // bool for
                                                    // correctly answered

        public QuestionLoadOrder QuestionLoadOrder { get { return questionLoadOrder; } }

        private void Start()
        {
            InitQuestionDictionary();
            CloseWindow();
        }

        private void InitQuestionDictionary()
        {
            questionBank = new Dictionary<Difficulty, QuestionPool>()
            {
                { Difficulty.EASY, _easy },
                { Difficulty.MEDIUM, _medium },
                { Difficulty.HARD, _hard },
            };
        }

        public void StartQuestion(Artifact artifact)
        {
            Assert.IsNotNull(artifact,
                $"{name}'s StartQuestion(Artifact) was " +
                $"passed NULL instead of an Artifact.");

            if (!artifact.BeenVisited)
            {
                currentArtifactInteractionCount++;
                artifact.Visit();
            }

            OpenWindow();

            // If the artifact hasn't been assigned a question yet, get one
            artifact.Question ??= GetQuestion(artifact);

            OnQuestionStarted(artifact);
        }

        public void CancelQuestion()
        {
            CloseWindow();

            OnQuestionCanceled();
        }

        public void AnswerQuestion(bool answeredCorrectly)
        {
            // Set strategies
            Action responsePanelAction = answeredCorrectly
                ? () => responsePanel.SetCorrect()
                : () => responsePanel.SetIncorrect();

            Action questionWindowAction = answeredCorrectly
                ? () => CloseWindow()
                : () => ResetWindow();


            // Perform Question UI related responses
            responsePanelAction();      // Set the response panel state immediately
            responsePanel.Open();       // Open the response panel after setting it.
            questionPanel.HideQuestion();   // Hide the question part of the
                                            // question panel

            // Create a timer that will run for some float secs
            CountdownTimer timer = new(this, answerResponseDuration);

            // Create a process that will perform the question window
            // strategy we set above once the timer finishes.
            WaitThenDo waitAndClose = new(
                this,
                () => timer.IsFinished,
                () => timer.BeenCanceled,
                questionWindowAction,
                () => { }
            );

            timer.Start();          // Start the timer
            waitAndClose.Start();   // Start the wait process


            // Raise the question answered event immediately
            // (distinguishing  WindowClosed or WindowReset
            // events. Listeners can subsribe to v_this_v if they want
            // something done right away, or the other events if they want to
            // sync to the delay.
            OnQuestionAnswered(answeredCorrectly);
        }

        public QuestionSO GetQuestion(Artifact artifact)
        {
            Assert.IsNotNull(artifact,
                $"{name}'s GetQuestion(Artifact) was " +
                $"passed NULL instead of an Artifact.");

            Assert.IsTrue(questionBank.ContainsKey(artifact.Difficulty),
                $"{name}'s questionBank did not contain an entry with " +
                $"a key for {artifact.Difficulty}. Make sure the question bank " +
                $"has a {artifact.Difficulty} key with a matching QuestionPool");

            return QuestionLoadOrder switch
            {
                QuestionLoadOrder.RANDOM
                    => questionBank[artifact.Difficulty].GetRandomQuestion(),
                QuestionLoadOrder.INTERACTION_ORDER
                    => questionBank[artifact.Difficulty].GetQuestionAt(currentArtifactInteractionCount),
                QuestionLoadOrder.INSPECTOR_INDEX
                    => questionBank[artifact.Difficulty].GetQuestionAt(artifact.Index),

                _   => questionBank[artifact.Difficulty].GetRandomQuestion() //default
            };
        }

        ///// <summary>
        ///// Gets a random question from the appropriate pool.
        ///// If the requested pool doesn't exist or doesnt have
        ///// any questions, this function returns null.
        ///// </summary>
        ///// <param name="difficulty"></param>
        ///// <returns></returns>
        //public QuestionSO GetQuestion(Difficulty difficulty)
        //{
        //    QuestionPool pool = GetPool(difficulty);

        //    // Null checks
        //    if (pool == null) { return null; }
        //    if (pool.Empty) { return null; }

        //    return pool.GetRandomQuestion();
        //}

        //public QuestionSO GetQuestion(Difficulty difficulty, int artifactIndex)
        //{
        //    QuestionPool pool = GetPool(difficulty);

        //    // Null checks
        //    if (pool == null) { return null; }
        //    if (pool.Empty) { return null; }

        //    return pool.GetQuestionAt(artifactIndex);
        //}

        //public QuestionSO GetNextQuestion(Difficulty difficulty)
        //{
        //    Debug.Log($"{name} getting next question via GetNextQuestion(Difficulty)");
        //    QuestionPool pool = GetPool(difficulty);

        //    // Null checks
        //    if (pool == null) { return null; }
        //    if (pool.Empty) { return null; }

        //    return pool.GetQuestionAt(currentArtifactInteractionCount);
        //}

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

        protected virtual void OnQuestionStarted(Artifact artifact)
        {
            QuestionStarted?.Invoke(artifact);
        }

        private void OnQuestionCanceled()
        {
            QuestionCanceled?.Invoke();
        }

        protected virtual void OnQuestionAnswered(bool correctly)
        {
            QuestionAnswered?.Invoke(correctly);
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
