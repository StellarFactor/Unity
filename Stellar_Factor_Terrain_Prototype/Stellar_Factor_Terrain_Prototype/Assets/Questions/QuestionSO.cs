using UnityEngine;

namespace StellarFactor
{
    [CreateAssetMenu(fileName = "New Question", menuName = "Question")]
    public class QuestionSO : ScriptableObject
    {
        [SerializeField, TextArea(minLines:3, maxLines:20)] private string _text;

        [SerializeField] private Answer[] _answers;

        public string Text { get { return _text; } }
        public Answer[] Answers { get { return _answers; } }
    }
}
