using System;
using UnityEngine;

namespace StellarFactor
{
    public class QuestionPanel : MonoBehaviour
    {
        [SerializeField] private Textbox _questionBox;
        [SerializeField] private AnswerField[] _answerFields;

        private string _questionText;
        private Answer[] _answers;

        public bool Initialized { get; private set; }

        private void OnEnable()
        {
            QuestionCanvas.MGR.InitializeQuestion += onInitialize;
            QuestionCanvas.MGR.UnselectAll += onUnselectAll;
        }

        private void OnDisable()
        {
            QuestionCanvas.MGR.InitializeQuestion -= onInitialize;
            QuestionCanvas.MGR.UnselectAll -= onUnselectAll;
        }

        private void onInitialize(QuestionSO scriptableQuestion, AnswerColors answerColors)
        {
            _questionText = scriptableQuestion.Text;
            _questionBox.Text.Set(_questionText);

            _answers = scriptableQuestion.Answers;

            for(int i = 0; i < _answers.Length; i++)
            {
                if(i >= _answerFields.Length) { continue; }

                _answerFields[i].SetWith(_answers[i], answerColors, i);
            }

            Initialized = true;
        }

        private void onUnselectAll()
        {
            QuestionCanvas.MGR.NewQuestion();
        }
    }
}
