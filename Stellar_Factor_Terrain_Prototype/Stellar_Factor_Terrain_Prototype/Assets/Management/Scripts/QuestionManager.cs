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

        private int successfulQuestionLoadCount;

        private bool isCurrentlyAnswering;
        private IAcquirable answeringToAcquire;

        public event Action WindowOpened;
        public event Action WindowClosed;
        public event Action WindowReset;
        public event Action<QuestionSO> QuestionStarted;
        public event Action QuestionCanceled;

        /// <summary>
        /// Subscribe to this for any actions that need to be performed as soon
        /// as the QuestionManager reads the answer input.
        /// Distinct from <c><see cref="WindowClosed"/></c>, which will fire
        /// <c><see cref="answerResponseDuration"/></c> seconds after
        /// the answer is read.
        /// </summary>
        public event Action<bool, IAcquirable> QuestionAnswered; // bool for
                                                                 // correctly answered

        public QuestionSO CurrentQuestion { get; private set; }
        public QuestionLoadOrder QuestionLoadOrder { get { return questionLoadOrder; } }

        private void Start()
        {
            InitQuestionDictionary();
            CloseWindow();
        }

        private void Update()
        {
// This can be turned on in Project Settings > Player > Other > Scripting Define Symbols
#if DEBUG_KEYS
            if (isCurrentlyAnswering
                && Input.GetKey(KeyCode.LeftShift)
                && Input.GetKey(KeyCode.RightShift)
                && Input.GetKeyDown(KeyCode.Slash))
            {
                AnswerQuestion(true);
            }
#endif
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

        public void StartQuestion(QuestionSO question, IAcquirable toAcquire)
        {
            Assert.IsNotNull(question,
                $"{name}'s StartQuestion(QuestionSO) was " +
                $"passed NULL instead of a QuestionSO.");

            CurrentQuestion = question;
            CurrentQuestion.OnLoadIntoWindow();

            answeringToAcquire = toAcquire;

            OpenWindow();

            OnQuestionStarted(question);
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
                ? () => responsePanel.SetCorrect(CurrentQuestion.QuestionGivenBy)
                : () => responsePanel.SetIncorrect(CurrentQuestion.QuestionGivenBy);

            Action questionWindowAction = answeredCorrectly
                ? () => CloseWindow()
                : () => ResetWindow();


            // == Perform Question UI related responses ==
            // Set the response panel state immediately
            responsePanelAction();

            // Open the response panel after setting it.
            responsePanel.Open();

            // Hide the question part of the question panel
            questionPanel.HideQuestion();

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
            OnQuestionAnswered(answeredCorrectly, answeringToAcquire);
        }

        public bool TryGetQuestion(Artifact artifact, out QuestionSO question)
        {
            Assert.IsNotNull(artifact,
                $"{name}'s GetQuestion(Artifact) was " +
                $"passed NULL instead of an Artifact.");

            Assert.IsTrue(questionBank.ContainsKey(artifact.Difficulty),
                $"{name}'s questionBank did not contain an entry with " +
                $"a key for {artifact.Difficulty}. Make sure the question bank " +
                $"has a {artifact.Difficulty} key with a matching QuestionPool");

            if (artifact.Question != null)
            {
                Debug.LogError(
                    $"{artifact.name} is already assigned a question, " +
                    $"(\"{artifact.Question}\"). " +
                    $"currentArtifactInteractionCount will NOT be incremented " +
                    $"and the outgoing QuestionSO var will NOT be assigned.");
                question = null;
                return false;
            }

            question = QuestionLoadOrder switch
            {
                QuestionLoadOrder.RANDOM
                    => questionBank[artifact.Difficulty].GetRandomQuestion(),
                QuestionLoadOrder.INTERACTION_ORDER
                    => questionBank[artifact.Difficulty].GetQuestionAt(successfulQuestionLoadCount),
                QuestionLoadOrder.INSPECTOR_INDEX
                    => questionBank[artifact.Difficulty].GetQuestionAt(artifact.Index),

                _   => questionBank[artifact.Difficulty].GetRandomQuestion() //default
            };

            if (question == null)
            {
                Debug.LogError(
                    $"Something went wrong when assigning" +
                    $"a question to {artifact.name}.",
                    this);
                return false;
            }
            else
            {
                successfulQuestionLoadCount++;
                return true;
            }
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

        protected virtual void OnQuestionStarted(QuestionSO question)
        {
            isCurrentlyAnswering = true;
            QuestionStarted.Invoke(question);
        }

        private void OnQuestionCanceled()
        {
            isCurrentlyAnswering = false;
            QuestionCanceled?.Invoke();
        }

        protected virtual void OnQuestionAnswered(bool correctly, IAcquirable toAcquire)
        {
            isCurrentlyAnswering = false;
            QuestionAnswered?.Invoke(correctly, toAcquire);
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
