using UnityEngine;

namespace StellarFactor
{
    public class ResponsePanel : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

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

        private void Start()
        {
            Close();
        }

        public void Open()
        {
            _canvas.enabled = true;
        }

        public void Close()
        {
            _canvas.enabled = false;
        }

        public void SetCorrect()
        {
            _questionResponseBox.Text.Set(_correctMessage);
            _questionResponseBox.TextColor.Set(_answerColors.Correct);

            _artifactResponseBox.Text.Set(_acquiredArtifactMessasge);
            _artifactResponseBox.TextColor.Set(_answerColors.Highlight);
        }

        public void SetIncorrect()
        {
            _questionResponseBox.Text.Set(_incorrectMessage);
            _questionResponseBox.TextColor.Set(_answerColors.Incorrect);

            _artifactResponseBox.Text.Set(_failedArtifactMessage);
            _artifactResponseBox.TextColor.Set(_answerColors.Highlight);
        }

        public void ResetPanel()
        {
            _questionResponseBox.ResetAll();
            _artifactResponseBox.ResetAll();
        }
    }
}