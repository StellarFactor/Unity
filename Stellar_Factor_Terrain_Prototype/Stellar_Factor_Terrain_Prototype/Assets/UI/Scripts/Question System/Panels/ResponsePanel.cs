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

        [Space(5)]
        [SerializeField] private string _pedistalArtifactSuccessMessage;
        [SerializeField] private string _pedistalArtifactFailMessage;

        [Space(5)]
        [SerializeField] private string _bossDefeatedMessage;
        [SerializeField] private string _bossNotDefeatedMessage;

        public QuestionGivenBy QuestionGivenBy { get; private set; }

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

        public void SetCorrect(QuestionGivenBy givenBy)
        {
            QuestionGivenBy = givenBy;

            _questionResponseBox.Text.Set(_correctMessage);
            _questionResponseBox.TextColor.Set(_answerColors.Correct);

            _artifactResponseBox.Text.Set(GetCorrectDetailMessage(givenBy));
            _artifactResponseBox.TextColor.Set(_answerColors.Highlight);
        }

        public void SetIncorrect(QuestionGivenBy givenBy)
        {
            _questionResponseBox.Text.Set(_incorrectMessage);
            _questionResponseBox.TextColor.Set(_answerColors.Incorrect);

            _artifactResponseBox.Text.Set(GetIncorrectDetailMessage(givenBy));
            _artifactResponseBox.TextColor.Set(_answerColors.Highlight);
        }

        public void ResetPanel()
        {
            _questionResponseBox.ResetAll();
            _artifactResponseBox.ResetAll();
        }

        private string GetCorrectDetailMessage(QuestionGivenBy givenBy)
        {
            return givenBy switch {
                QuestionGivenBy.ARTIFACT => _acquiredArtifactMessasge,
                QuestionGivenBy.BOSS => _bossDefeatedMessage,
                QuestionGivenBy.PEDISTAL => _pedistalArtifactSuccessMessage,
                _ => _acquiredArtifactMessasge
            };
        }

        private string GetIncorrectDetailMessage(QuestionGivenBy givenBy)
        {
            return givenBy switch
            {
                QuestionGivenBy.ARTIFACT => _failedArtifactMessage,
                QuestionGivenBy.BOSS => _bossNotDefeatedMessage,
                QuestionGivenBy.PEDISTAL => _pedistalArtifactFailMessage,
                _ => _failedArtifactMessage
            };
        }
    }
}