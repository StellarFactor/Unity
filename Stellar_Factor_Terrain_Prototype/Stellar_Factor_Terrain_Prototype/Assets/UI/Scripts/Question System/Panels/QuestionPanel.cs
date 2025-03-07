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
            GameManager.MGR.ArtifactInteraction += HandleArtifactInteraction;
            GameManager.MGR.CancelArtifactInteraction += HandleCancelArtifactInteraction;
            QuestionManager.MGR.WindowOpened += HandleQuestionWindowOpened;
            QuestionManager.MGR.WindowClosed += HandleQuestionWindowClosed;
            QuestionManager.MGR.WindowReset += HandleQuestionWindowReset;
            QuestionManager.MGR.AnsweredCorrectly += HandleAnsweredCorrectly;
            QuestionManager.MGR.AnsweredIncorrectly += HandleAnsweredIncorrectly;
        }

        private void OnDisable()
        {
            GameManager.MGR.ArtifactInteraction -= HandleArtifactInteraction;
            GameManager.MGR.CancelArtifactInteraction -= HandleCancelArtifactInteraction;
            QuestionManager.MGR.WindowOpened -= HandleQuestionWindowOpened;
            QuestionManager.MGR.WindowClosed -= HandleQuestionWindowClosed;
            QuestionManager.MGR.WindowReset -= HandleQuestionWindowReset;
            QuestionManager.MGR.AnsweredCorrectly -= HandleAnsweredCorrectly;
            QuestionManager.MGR.AnsweredIncorrectly -= HandleAnsweredIncorrectly;
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

            QuestionManager.MGR.ResetWindow();

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

        private void HandleCancelArtifactInteraction()
        {
            QuestionManager.MGR.CloseWindow();
        }

        private void HandleQuestionWindowOpened()
        {
            _canvas.enabled = true;
        }

        private void HandleQuestionWindowClosed()
        {
            _canvas.enabled = false;
        }
        private void HandleQuestionWindowReset()
        {
            _questionBox.enabled = true;
        }

        private void HandleAnsweredCorrectly()
        {
            _questionBox.enabled = false;

            CountdownTimer timer = new(this, 2f);
            timer.Start();

            WaitThenDo waitAndClose = new(
                this,
                () => timer.IsFinished,
                () => timer.BeenCanceled,
                () => QuestionManager.MGR.CloseWindow(),
                () => { });

            waitAndClose.Start();
        }

        private void HandleAnsweredIncorrectly()
        {
            _questionBox.enabled = false;

            CountdownTimer timer = new(this, 2f);
            timer.Start();

            WaitThenDo waitAndReset = new(
                this,
                () => timer.IsFinished,
                () => timer.BeenCanceled,
                () => QuestionManager.MGR.ResetWindow(),
                () => { });

            waitAndReset.Start();
        }
    }
}
