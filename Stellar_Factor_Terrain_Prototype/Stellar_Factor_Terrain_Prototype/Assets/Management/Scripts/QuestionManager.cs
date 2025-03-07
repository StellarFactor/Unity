using StellarFactor.Global;
using System;
using UnityEngine;

namespace StellarFactor
{
    public enum QuestionLoadOrder { RANDOM, INSPECTOR_INDEX, INTERACTION_ORDER };
    public class QuestionManager : Singleton<QuestionManager>
    {
        [Header("Settings")]
        [SerializeField] private QuestionLoadOrder questionLoadOrder;

        [Header("Question Pools")]
        [SerializeField] private QuestionPool _easy;
        [SerializeField] private QuestionPool _medium;
        [SerializeField] private QuestionPool _hard;

        private int currentArtifactInteractionCount;

        public Action WindowOpened;
        public Action WindowClosed;
        public Action WindowReset;
        public Action<int> AnswerSelected;
        public Action AnsweredCorrectly;
        public Action AnsweredIncorrectly;

        public QuestionLoadOrder QuestionLoadOrder { get { return questionLoadOrder; } }

        private void OnEnable()
        {
            GameManager.MGR.LevelLoaded += onLevelLoaded;
            GameManager.MGR.ArtifactInteraction += onArtifactFound;
        }

        private void OnDisable()
        {
            GameManager.MGR.ArtifactInteraction -= onArtifactFound;
        }

        private void onLevelLoaded(int obj)
        {
            CloseWindow();
        }

        private void onArtifactFound(Artifact artifact)
        {
            currentArtifactInteractionCount++;
            OpenWindow();
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
            QuestionPool pool = getPool(difficulty);

            // Null checks
            if (pool == null) { return null; }
            if (pool.Empty) { return null; }

            return pool.GetRandomQuestion();
        }

        public QuestionSO GetQuestion(Difficulty difficulty, int artifactIndex)
        {
            QuestionPool pool = getPool(difficulty);

            // Null checks
            if (pool == null) { return null; }
            if (pool.Empty) { return null; }

            return pool.GetQuestionAt(artifactIndex);
        }

        public QuestionSO GetNextQuestion(Difficulty difficulty)
        {
            QuestionPool pool = getPool(difficulty);

            // Null checks
            if (pool == null) { return null; }
            if (pool.Empty) { return null; }

            return pool.GetQuestionAt(currentArtifactInteractionCount);

        }

        public void OpenWindow()
        {
            WindowOpened?.Invoke();
        }

        public void CloseWindow()
        {
            WindowClosed?.Invoke();
        }

        public void ResetWindow()
        {
            WindowReset?.Invoke();
        }

        private QuestionPool getPool(Difficulty difficulty)
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
