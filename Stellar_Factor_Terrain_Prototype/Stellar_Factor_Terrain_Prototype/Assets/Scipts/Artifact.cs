using StellarFactor.Global;
using UnityEngine;
using UnityEngine.Events;

namespace StellarFactor
{
    public class Artifact : MonoBehaviour, IInteractable
    {
        [SerializeField] private Difficulty _difficulty;
        [SerializeField] private int _index;

        [SerializeField] private UnityEvent OnPlayerEnter;
        [SerializeField] private UnityEvent OnInteract;
        [SerializeField] private UnityEvent OnPlayerExit;

        private bool _playerHere;
        private QuestionSO _question;

        public QuestionSO Question { get { return _question; } }

        private void OnEnable()
        {
            QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
        }


        private void OnDisable()
        {
            QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
        }

        private void Start()
        {
            // Fill this with a question when it's loaded
            _question = getQuestion();
        }

        private void onCorrectAnswer()
        {
            if (!_playerHere) { return; }

            // TODO:
            // animate?
            // anything else?

            // Temporarily setting this to get destroyed after
            // one second, but we could/should tie the time to
            // something else.
            // Waiting for the player to have it in their inventory?
            // Waiting for the end of an anim?
            // idk.
            Destroy(gameObject, 1f);
        }

        private void onIncorrectAnswer()
        {
            if (!_playerHere) { return; }

            // Anything we might want in here?
            // A cooldown? (i.e. gotta go try a different one first)

            // Maybe load a new question? ==============================
            // Dr T. said this was too complicated,
            // but I'll leave it here as a comment just in case.

            //_question = QuestionManager.MGR.GetQuestion(_difficulty);
            //GameManager.MGR.PlayerDeath.Invoke();
            // =========================================================
        }

        public void PlayerEnterRange()
        {
            _playerHere = true;
            OnPlayerEnter.Invoke();
        }

        public void Interact()
        {
            OnInteract.Invoke();
            GameManager.MGR.ArtifactInteraction.Invoke(this);
        }

        public void PlayerExitRange()
        {
            OnPlayerExit.Invoke();
            _playerHere = false;
        }

        private QuestionSO getQuestion()
        {
            if (QuestionManager.MGR.RandomMode)
            {
                return QuestionManager.MGR.GetQuestion(_difficulty);
            }
            else
            {
                return QuestionManager.MGR.GetQuestion(_difficulty, _index);
            }
        }
    }
}
