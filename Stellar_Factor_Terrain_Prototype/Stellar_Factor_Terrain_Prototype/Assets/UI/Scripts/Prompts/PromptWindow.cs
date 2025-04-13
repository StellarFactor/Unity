using UnityEngine;

namespace StellarFactor
{
    public class PromptWindow : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Tooltip(
            "The token substring to replace (in the \"interaction prompt\" " +
            "below) that will be replaced with the text of the key to press.")]
        private string keyReplacementToken;

        [SerializeField, Tooltip(
            "The token substring to replace (in the \"interaction prompt\" " +
            "below) that will be replaced with the text of the action that " +
            "will be performed when the key is pressed.")]
        private string actionReplacementToken;

        [SerializeField] private string promptMessage;

        [Header("Internal References")]
        [SerializeField] private Spritebox _background;
        [SerializeField] private Textbox _textbox;

        public bool IsOpen { get; private set; }

        private void OnEnable()
        {
            QuestionManager.MGR.WindowReset += HandleWindowReset;
        }

        private void OnDisable()
        {
            QuestionManager.MGR.WindowReset -= HandleWindowReset;
        }

        public void OpenPrompt(string message)
        {
            IsOpen = true;

            Show();
            Clear();

            _textbox.Text.Set(message);
        }

        public void OpenPrompt(KeyCode key, string actionToPrompt)
        {
            IsOpen = true;

            Show();
            Clear();

            string message = promptMessage;
            message = message.Replace(keyReplacementToken, $"{key}");
            message = message.Replace(actionReplacementToken, actionToPrompt);

            _textbox.Text.Set(message);
        }

        public void ClosePrompt()
        {
            IsOpen = false;

            Clear();
            Hide();
        }

        protected virtual void HandleWindowReset()
        {
            Clear();
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

