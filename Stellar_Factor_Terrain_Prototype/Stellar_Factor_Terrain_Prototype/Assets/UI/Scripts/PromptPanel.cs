using System;
using UnityEngine;

namespace StellarFactor
{
    public class PromptPanel : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Spritebox _background;
        [SerializeField] private Textbox _textbox;

        [Header("Settings")]
        [SerializeField] private string _interactionPrompt;

        [Space(10)]
        [SerializeField] private string _acquiredArtifactMessasge;
        [SerializeField] private string _failedArtifactMessage;
        [SerializeField] private AnswerColorsSO _answerColors;


        private void OnEnable()
        {
            GameManager.MGR.LevelLoaded += onLevelLoaded;
            GameManager.MGR.ArtifactInteraction += onArtifactInteraction;
            QuestionManager.MGR.Clear += onClear;
            QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
        }

        private void OnDisable()
        {
            GameManager.MGR.LevelLoaded -= onLevelLoaded;
            GameManager.MGR.ArtifactInteraction -= onArtifactInteraction;
            QuestionManager.MGR.Clear -= onClear;
            QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
        }

        private void onLevelLoaded(int buildIndex)
        {
            ClosePrompt();
        }

        public void onArtifactInteraction(Artifact artifact)
        {
            hide();
        }

        private void onClear()
        {
            _textbox.Text.Reset();
            _textbox.TextColor.Reset();
        }

        private void onCorrectAnswer()
        {
            show();

            _textbox.Text.Set(_acquiredArtifactMessasge);
            _textbox.TextColor.Set(_answerColors.Highlight);

            Invoke("hide", 2f);
        }

        private void onIncorrectAnswer()
        {
            show();

            _textbox.Text.Set(_failedArtifactMessage);
            _textbox.TextColor.Set(_answerColors.Highlight);

            Invoke("hide", 2f);
        }

        private void hide()
        {
            _background.enabled = false;
            _textbox.enabled = false;
        }

        private void show()
        {
            _background.enabled = true;
            _textbox.enabled = true;
        }

        public void InteractionPrompt()
        {
            show();
            onClear();

            string message = _interactionPrompt;
            message = message.Replace("<>", $"{GameManager.MGR.InteractKey}");

            _textbox.Text.Set(message);
        }

        public void ClosePrompt()
        {
            onClear();
            hide();
        }
    }
}