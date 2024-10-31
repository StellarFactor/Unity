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

        public bool Empty => (_questions == null) || (_questions.Length == 0);

        /// <summary>
        /// Returns a random question from the pool.
        /// Returns null if it fails to find a valid index.
        /// </summary>
        /// <returns></returns>
        public QuestionSO GetRandomQuestion()
        {
            if (_questions == null) { return null; }
            if (_questions.Length == 0) {  return null; }

            int randomIndex;

            const int LIMIT = 100;
            int count = 0;

            do
            {
                // Increment count, check limit
                if (++count >= LIMIT) { return null; }

                // Get a random number
                randomIndex = UnityEngine.Random.Range(0, _questions.Length);

                // Try again if the question at this index has been used.
            } while (_usedIndexes.Contains(randomIndex));

            // Add this index to the list of ones we've used.
            _usedIndexes.Add(randomIndex);

            // Return the question at this index.
            return _questions[randomIndex];
        }
    }
}
