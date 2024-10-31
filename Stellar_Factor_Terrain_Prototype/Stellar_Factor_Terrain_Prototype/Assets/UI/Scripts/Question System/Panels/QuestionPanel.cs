using System;
using UnityEngine;

namespace StellarFactor
{
    public class QuestionPanel : MonoBehaviour
    {
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
            GameManager.MGR.ArtifactInteraction += onArtifactInteraction;
            QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
        }


        private void OnDisable()
        {
            GameManager.MGR.ArtifactInteraction -= onArtifactInteraction;
            QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
        }

        private void onArtifactInteraction(Artifact artifact)
        {
            QuestionManager.MGR.Clear.Invoke();

            _questionText = artifact.Question.Text;
            _questionBox.Text.Set(_questionText);

            _answers = artifact.Question.Answers;

            for(int i = 0; i < _answers.Length; i++)
            {
                if(i >= _answerFields.Length) { continue; }

                _answerFields[i].FillWith(_answers[i], new AnswerColors(_answerColors), i);
            }

            Initialized = true;
        }

        private void onCorrectAnswer()
        {
            QuestionManager.MGR.Invoke("CloseWindow", 2f);
        }

        private void onIncorrectAnswer()
        {
            QuestionManager.MGR.Invoke("CloseWindow", 2f);
        }
    }
}
