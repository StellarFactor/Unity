using System;
using UnityEngine;

namespace StellarFactor
{
    public class PromptPanel : MonoBehaviour
    {
        [SerializeField] private string _interactionPrompt;

        [SerializeField] private string _acquiredArtifactMessasge;
        [SerializeField] private string _failedArtifactMessage;
        [SerializeField] private AnswerColorsSO _answerColors;

        [SerializeField] private Spritebox _background;
        [SerializeField] private Textbox _text;

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
            _text.Text.Reset();
            _text.TextColor.Reset();
        }

        private void onCorrectAnswer()
        {
            show();

            _text.Text.Set(_acquiredArtifactMessasge);
            _text.TextColor.Set(_answerColors.Highlight);

            Invoke("hide", 2f);
        }

        private void onIncorrectAnswer()
        {
            show();

            _text.Text.Set(_failedArtifactMessage);
            _text.TextColor.Set(_answerColors.Highlight);

            Invoke("hide", 2f);
        }

        private void hide()
        {
            _background.enabled = false;
            _text.enabled = false;
        }

        private void show()
        {
            _background.enabled = true;
            _text.enabled = true;
        }

        public void InteractionPrompt()
        {
            show();
            onClear();

            string message = _interactionPrompt;
            message = message.Replace("<>", $"{GameManager.MGR.InteractKey}");

            _text.Text.Set(message);
        }

        public void ClosePrompt()
        {
            onClear();
            hide();
        }
    }
}