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
            GameManager.MGR.LevelLoaded += HandleLevelLoaded;
            GameManager.MGR.ArtifactInteraction += HandleArtifactInteraction;
            QuestionManager.MGR.WindowReset += Clear;
        }

        private void OnDisable()
        {
            GameManager.MGR.LevelLoaded -= HandleLevelLoaded;
            GameManager.MGR.ArtifactInteraction -= HandleArtifactInteraction;
            QuestionManager.MGR.WindowReset -= Clear;
        }

        public void DisplayInteractionPrompt()
        {
            Show();
            Clear();

            string message = _interactionPrompt;
            message = message.Replace("<>", $"{GameManager.MGR.InteractKey}");

            _textbox.Text.Set(message);
        }

        public void ClosePrompt()
        {
            Clear();
            Hide();
        }

        private void HandleLevelLoaded(int buildIndex)
        {
            ClosePrompt();
        }

        private void HandleArtifactInteraction(Artifact artifact)
        {
            Hide();
        }

        private void Clear()
        {
            _textbox.ResetAll();
        }

        private void Hide()
        {
            _background.enabled = false;
            _textbox.enabled = false;
        }

        private void Show()
        {
            _background.enabled = true;
            _textbox.enabled = true;
        }
    }
}