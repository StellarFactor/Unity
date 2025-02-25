using UnityEngine;

namespace StellarFactor
{
    [System.Serializable]
    public class Answer
    {
        [SerializeField] private string _answerText;
        [SerializeField] bool _correct;

        public string AnswerText { get { return _answerText; } }
        public bool Correct { get { return _correct; } }
    }
}
