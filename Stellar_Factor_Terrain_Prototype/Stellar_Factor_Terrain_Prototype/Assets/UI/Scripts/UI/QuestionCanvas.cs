using System;
using UnityEngine;

namespace StellarFactor
{
    public class QuestionCanvas : MonoBehaviour
    {
        public static QuestionCanvas MGR;

        [SerializeField] QuestionPanel questionPanel;

        [SerializeField] QuestionSO[] _questionPool;
        [SerializeField] AnswerColorsSO _answerColors;

        public Action<QuestionSO, AnswerColors> InitializeQuestion;
        public Action<int> SelectAnswer;
        public Action UnselectAll;

        private void Awake()
        {
            if (MGR != null)
            {
                Destroy(gameObject);
            }
            else
            {
                MGR = this;
            }
        }

        private void Start()
        {
            NewQuestion();
        }

        public void NewQuestion()
        {
            int randomIndex = UnityEngine.Random.Range(0, _questionPool.Length);
            InitializeQuestion.Invoke(_questionPool[randomIndex], new AnswerColors(_answerColors));
        }
    }
}
