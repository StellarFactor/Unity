using StellarFactor.Global;
using System;
using UnityEngine;

namespace StellarFactor
{
    public class QuestionManager : Singleton<QuestionManager>
    {
        [Header("Question Pools")]
        [SerializeField] private QuestionPool _easy;
        [SerializeField] private QuestionPool _medium;
        [SerializeField] private QuestionPool _hard;

        public Action Open;
        public Action Close;
        public Action Clear;
        public Action<int> SelectAnswer;
        public Action CorrectAnswer;
        public Action IncorrectAnswer;

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
            // Find the right pool.
            QuestionPool pool = difficulty switch
            {
                Difficulty.EASY     => _easy,
                Difficulty.MEDIUM   => _medium,
                Difficulty.HARD     => _hard,
                _                   => _medium
            };

            // Null checks
            if (pool == null) { return null; }
            if (pool.Empty) { return null; }
            
            return pool.GetRandomQuestion();
        }

        public void OpenWindow()
        {
            Open.Invoke();
        }

        public void CloseWindow()
        {
            Close.Invoke();
        }

        public void ResetWindow()
        {
            Clear.Invoke();
        }
    }
}
