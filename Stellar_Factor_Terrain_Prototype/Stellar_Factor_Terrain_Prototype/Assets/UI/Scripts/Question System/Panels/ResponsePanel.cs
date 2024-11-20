using System;
using UnityEngine;

namespace StellarFactor
{
    public class ResponsePanel : MonoBehaviour
    {
        [SerializeField] private AnswerColorsSO _answerColors;

        [Header("Question Response Settings")]
        [SerializeField] private Textbox _questionResponseBox;

        [Space(5)]
        [SerializeField] private string _correctMessage;
        [SerializeField] private string _incorrectMessage;

        [Space(10)]
        [Header("Artifact Response Settings")]
        [SerializeField] private Spritebox _artifactResponseBKG;
        [SerializeField] private Textbox _artifactResponseBox;

        [Space(5)]
        [SerializeField] private string _acquiredArtifactMessasge;
        [SerializeField] private string _failedArtifactMessage;

        private void OnEnable()
        {
            QuestionManager.MGR.Open += onOpen;
            QuestionManager.MGR.Close += onClose;
            QuestionManager.MGR.Reset += onClear;
            QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
        }


        private void OnDisable()
        {
            QuestionManager.MGR.Open -= onOpen;
            QuestionManager.MGR.Close -= onClose;
            QuestionManager.MGR.Reset -= onClear;
            QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
        }

        private void Start()
        {
            onClear();
        }

        private void onOpen()
        {
            showAll();
        }

        private void onClose()
        {
            _questionResponseBox.ResetAll();
            _artifactResponseBox.ResetAll();

            hideAll();
        }

        private void onClear()
        {
            _questionResponseBox.ResetAll();
            _artifactResponseBox.ResetAll();

            hideAll();
        }

        private void onCorrectAnswer()
        {
            showAll();

            _questionResponseBox.Text.Set(_correctMessage);
            _questionResponseBox.TextColor.Set(_answerColors.Correct);

            _artifactResponseBox.Text.Set(_acquiredArtifactMessasge);
            _artifactResponseBox.TextColor.Set(_answerColors.Highlight);

            Invoke("hideAll", 2f);
        }

        private void onIncorrectAnswer()
        {
            showAll();

            _questionResponseBox.Text.Set(_incorrectMessage);
            _questionResponseBox.TextColor.Set(_answerColors.Incorrect);

            _artifactResponseBox.Text.Set(_failedArtifactMessage);
            _artifactResponseBox.TextColor.Set(_answerColors.Highlight);

            Invoke("hideAll", 2f);
        }

        private void hideAll()
        {
            _questionResponseBox.enabled = false;

            _artifactResponseBKG.enabled = false;
            _artifactResponseBox.enabled = false;
        }

        private void showAll()
        {
            _questionResponseBox.enabled = true;

            _artifactResponseBKG.enabled = true;
            _artifactResponseBox.enabled = true;
        }
    }
}