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
            GameManager.MGR.ArtifactInteraction += onArtifactInteraction;
            GameManager.MGR.CancelArtifactInteraction += onCancelArtifactInteraction;
            QuestionManager.MGR.Open += onOpen;
            QuestionManager.MGR.Close += onClose;
            QuestionManager.MGR.Reset += onReset;
            QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
        }

        private void OnDisable()
        {
            GameManager.MGR.ArtifactInteraction -= onArtifactInteraction;
            GameManager.MGR.CancelArtifactInteraction -= onCancelArtifactInteraction;
            QuestionManager.MGR.Open -= onOpen;
            QuestionManager.MGR.Close -= onClose;
            QuestionManager.MGR.Reset -= onReset;
            QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
        }


        private void onArtifactInteraction(Artifact artifact)
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

            QuestionManager.MGR.Reset.Invoke();

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

        private void onCancelArtifactInteraction()
        {
            QuestionManager.MGR.CloseWindow();
        }

        private void onOpen()
        {
            _canvas.enabled = true;
        }

        private void onClose()
        {
            _canvas.enabled = false;
        }
        private void onReset()
        {
            _questionBox.enabled = true;
        }

        private void onCorrectAnswer()
        {
            _questionBox.enabled = false;

            QuestionManager.MGR.Invoke("CloseWindow", 2f);
        }

        private void onIncorrectAnswer()
        {
            _questionBox.enabled = false;

            //QuestionManager.MGR.Invoke("CloseWindow", 2f);
            QuestionManager.MGR.Invoke("ResetWindow", 2f);
        }
    }
}
