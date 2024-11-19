using System;
using UnityEngine;

namespace StellarFactor
{
    public class InteractionPrompt : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Spritebox _background;
        [SerializeField] private Textbox _textbox;

        [Header("Settings")]
        [SerializeField] private string _interactionPrompt;

        private void OnEnable()
        {
            GameManager.MGR.LevelLoaded += onLevelLoaded;
            GameManager.MGR.ArtifactInteraction += onArtifactInteraction;
            QuestionManager.MGR.Reset += onClear;
        }

        private void OnDisable()
        {
            GameManager.MGR.LevelLoaded -= onLevelLoaded;
            GameManager.MGR.ArtifactInteraction -= onArtifactInteraction;
            QuestionManager.MGR.Reset -= onClear;
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
            _textbox.ResetAll();
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

        public void DisplayInteractionPrompt()
        {
            show();
            onClear();

            string message = _interactionPrompt;
            message = message.Replace("<>", $"{GameManager.MGR.InteractKey}");

            _textbox.Text.Set(message);
        }

        public void ClosePrompt()
        {
            _textbox.ResetAll();
            hide();
        }
    }
}