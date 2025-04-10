using UnityEngine;
using UnityEngine.Assertions;

namespace StellarFactor
{
    public class QuestionPanel : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [Header("Question")]
        [SerializeField] private Textbox _questionBox;

        [Header("Answers")]
        [SerializeField] private AnswerField[] _answerFields;
        [SerializeField] private AnswerColorsSO _answerColors;


        private string _questionText;
        private Answer[] _answers;

        public bool Initialized { get; private set; }

        private void OnEnable()
        {
            QuestionManager.MGR.QuestionStarted += HandleQuestionStarted;
        }

        private void OnDisable()
        {
            QuestionManager.MGR.QuestionStarted -= HandleQuestionStarted;
        }


        private void HandleQuestionStarted(QuestionSO question)
        {
            Assert.IsNotNull(question,
                $"{name}'s HandleQuestionStarted(QuestionSO) was " +
                $"passed NULL instead of a QuestionSO.");

            _questionBox.enabled = true;

            _questionText = question.Text;
            _questionBox.Text.Set(_questionText);

            _answers = question.Answers;

            for(int i = 0; i < _answers.Length; i++)
            {
                if(i >= _answerFields.Length) { continue; }

                _answerFields[i].FillWith(_answers[i], new AnswerColors(_answerColors), i);
            }

            Initialized = true;
        }

        public void Open()
        {
            _canvas.enabled = true;
        }

        public void Close()
        {
            _canvas.enabled = false;
        }

        public void ResetPanel()
        {
            ShowQuestion();

            for (int i = 0; i < _answers?.Length; i++)
            {
                if (i >= _answerFields.Length) { continue; }

                _answerFields[i].Clear();
            }
        }

        public void ShowQuestion()
        {
            _questionBox.enabled = true;
        }

        public void HideQuestion()
        {
            _questionBox.enabled = false;
        }
    }
}
