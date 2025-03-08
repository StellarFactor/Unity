using UnityEngine;

namespace StellarFactor
{
    public class QuestionPanel : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [Header("Question")]
        [SerializeField] private Textbox _questionBox;

        [Header("Answers")]
        [SerializeField] private AnswerField[] _answerFields;
        [SerializeField] private AnswerColorsSO _answerColors;


        private string _questionText;
        private Answer[] _answers;

        public bool Initialized { get; private set; }

        private void OnEnable()
        {
            GameManager.MGR.ArtifactInteractionStarted += HandleArtifactInteraction;
        }

        private void OnDisable()
        {
            GameManager.MGR.ArtifactInteractionStarted -= HandleArtifactInteraction;
        }


        private void HandleArtifactInteraction(Artifact artifact)
        {
            if (artifact == null)
            {
                Debug.LogWarning("artifact null");
                return;            
            }

            if (artifact.Question == null)
            {
                Debug.LogWarning($"{artifact.gameObject.name} question was null");
                return;
            }

            _questionBox.enabled = true;

            _questionText = artifact.Question.Text;
            _questionBox.Text.Set(_questionText);

            _answers = artifact.Question.Answers;

            for(int i = 0; i < _answers.Length; i++)
            {
                if(i >= _answerFields.Length) { continue; }

                _answerFields[i].FillWith(_answers[i], new AnswerColors(_answerColors), i);
            }

            Initialized = true;
        }

        public void Open()
        {
            _canvas.enabled = true;
        }

        public void Close()
        {
            _canvas.enabled = false;
        }

        public void ResetPanel()
        {
            ShowQuestion();

            for (int i = 0; i < _answers?.Length; i++)
            {
                if (i >= _answerFields.Length) { continue; }

                _answerFields[i].Clear();
            }
        }

        public void ShowQuestion()
        {
            _questionBox.enabled = true;
        }

        public void HideQuestion()
        {
            _questionBox.enabled = false;
        }
    }
}
