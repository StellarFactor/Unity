using UnityEngine;

namespace StellarFactor
{
    public class ResponsePanel : Textbox
    {
        [SerializeField] private string _correctMessage;
        [SerializeField] private string _incorrectMessage;
        [SerializeField] private AnswerColorsSO _answerColors;

        private void OnEnable()
        {
            QuestionManager.MGR.Clear += onClear;
            QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
        }

        private void OnDisable()
        {
            QuestionManager.MGR.Clear -= onClear;
            QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
        }

        private void onClear()
        {
            Text.Reset();
            TextColor.Reset();
        }

        private void onCorrectAnswer()
        {
            Text.Set(_correctMessage);
            TextColor.Set(_answerColors.Correct);
            QuestionManager.MGR.Invoke("CloseWindow", 2f);
        }

        private void onIncorrectAnswer()
        {
            Text.Set(_incorrectMessage);
            TextColor.Set(_answerColors.Incorrect);
            QuestionManager.MGR.Invoke("CloseWindow", 2f);
        }
    }
}