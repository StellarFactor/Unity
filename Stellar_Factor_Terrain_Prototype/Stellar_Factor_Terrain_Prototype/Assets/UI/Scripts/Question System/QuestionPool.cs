using StellarFactor.Global;
using System.Collections.Generic;
using UnityEngine;

namespace StellarFactor
{
    [System.Serializable]
    public class QuestionPool
    {
        [SerializeField] private Difficulty _difficulty;

        [SerializeField] private QuestionSO[] _questions;
        
        private List<int> _usedIndexes = new List<int>();

        public bool Empty
        {
            get
            {
                return _questions.Length == 0;
            }
        }

        public bool AllQuestionsUsed
        {
            get
            {
                if (Empty)
                {
                    return true;
                }

                return _usedIndexes.Count == _questions.Length;
            }
        }

        /// <summary>
        /// Returns a random question from the pool.
        /// Returns null if it fails to find a valid index.
        /// </summary>
        /// <returns></returns>
        public QuestionSO GetRandomQuestion()
        {
            if (Empty) { return null; }

            int randomIndex;

            const int LIMIT = 100;
            int count = 0;

            do
            {
                // Increment count, check limit
                if (++count >= LIMIT) { return null; }

                // Get a random number
                randomIndex = Random.Range(0, _questions.Length);

                // Try again if the question at this index has been used.
            } while (_usedIndexes.Contains(randomIndex));

            // Add this index to the list of ones we've used.
            _usedIndexes.Add(randomIndex);

            // Return the question at this index.
            return _questions[randomIndex];
        }

        public QuestionSO GetNextQuestionInOrder()
        {
            if (AllQuestionsUsed) { return null; }

            QuestionSO question = null;

            for (int questionIndex = 0; questionIndex < _questions.Length; questionIndex++)
            {
                bool used = false;

                for (int usedListIndex = 0; usedListIndex < _usedIndexes.Count; usedListIndex++)
                {
                    if (questionIndex == _usedIndexes[usedListIndex])
                    {
                        used = true;
                        break;
                    }
                }

                if (used) { continue; }

                _usedIndexes.Add(questionIndex);
                question = _questions[questionIndex];
                break;
            }

            return question;
        }

        public QuestionSO GetQuestionAt(int index)
        {
            if (Empty) { return null; }
            if (AllQuestionsUsed) { return null; }

            if (_usedIndexes.Contains(index))
            {
                Debug.LogWarning($"Already used the question at { index }");
                return null;
            }

            _usedIndexes.Add(index);
            return _questions[index];
        }
    }
}
