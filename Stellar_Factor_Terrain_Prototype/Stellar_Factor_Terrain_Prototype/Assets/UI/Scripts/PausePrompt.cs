#if UNITY_EDITOR
#endif

using UnityEditor.VersionControl;
using UnityEngine;

namespace StellarFactor
{
    public class PausePrompt : MonoBehaviour
    {
        [SerializeField] private Spritebox background;
        [SerializeField] private Textbox textbox;

        [SerializeField, Tooltip(
            "The token substring to replace (in the \"pausePrompt\" " +
            "below) that will be replaced with the text of the key to press.")]
        private string keyReplacementToken;

        [SerializeField] private string promptMessage;

        public void OpenPrompt()
        {
            Show();
            Clear();
            promptMessage = promptMessage.Replace(keyReplacementToken, $"{GameManager.MGR.PauseKey}");
            textbox.Text.Set(promptMessage);
        }

        public void ClosePrompt()
        {
            Clear();
            Hide();
        }

        private void Clear()
        {
            textbox.ResetAll();
        }

        private void Hide()
        {
            textbox.enabled = false;
            background.enabled = false;
        }

        private void Show()
        {
            textbox.enabled = true;
            background.enabled = true;
        }
    }
}