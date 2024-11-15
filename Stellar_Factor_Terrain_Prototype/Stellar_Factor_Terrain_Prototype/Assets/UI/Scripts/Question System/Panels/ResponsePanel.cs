using System;
using UnityEngine;

namespace StellarFactor
{
    public class ResponsePanel : MonoBehaviour
    {
        [SerializeField] private Textbox _textbox;

        [Header("Settings")]
        [SerializeField] private string _correctMessage;
        [SerializeField] private string _incorrectMessage;
        [SerializeField] private AnswerColorsSO _answerColors;

        private void OnEnable()
        {
            QuestionManager.MGR.Open += onOpen;
            QuestionManager.MGR.Close += onClose;
            QuestionManager.MGR.Clear += onClear;
            QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
        }


        private void OnDisable()
        {
            QuestionManager.MGR.Open -= onOpen;
            QuestionManager.MGR.Close -= onClose;
            QuestionManager.MGR.Clear -= onClear;
            QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
        }

        private void Start()
        {
            onClear();
        }

        private void onOpen()
        {
            _textbox.enabled = true;
        }

        private void onClose()
        {
            reset();
            _textbox.enabled = false;
        }

        private void onClear()
        {
            reset();
            _textbox.enabled = false;
        }

        private void onCorrectAnswer()
        {
            _textbox.enabled = true;
            _textbox.Text.Set(_correctMessage);
            _textbox.TextColor.Set(_answerColors.Correct);
        }

        private void onIncorrectAnswer()
        {
            _textbox.enabled = true;
            _textbox.Text.Set(_incorrectMessage);
            _textbox.TextColor.Set(_answerColors.Incorrect);
        }

        private void reset()
        {
            _textbox.Text.Reset();
            _textbox.TextColor.Reset();
        }
    }
}