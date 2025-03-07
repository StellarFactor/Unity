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

        [SerializeField] private GameObject particleEffect;

        private bool _playerHere;
        private QuestionSO _question;

        public QuestionSO Question { get { return _question; } }

        private void OnEnable()
        {
            QuestionManager.MGR.AnsweredCorrectly += HandleCorrectAnswer;
            QuestionManager.MGR.AnsweredIncorrectly += HandleIncorrectAnswer;
        }


        private void OnDisable()
        {
            QuestionManager.MGR.AnsweredCorrectly -= HandleCorrectAnswer;
            QuestionManager.MGR.AnsweredIncorrectly -= HandleIncorrectAnswer;
        }

        //private void Start()
        //{
        //    // Fill this with a question when it's loaded
        //    _question = getQuestion();
        //}

        private void HandleCorrectAnswer()
        {
            if (!_playerHere) { return; }

            // When a particle system gets assigned to the artifact, itll stop and destroy itself
            // once the question is answered correctly.
            if (particleEffect != null)
            {
                ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
                if (ps!= null)
                {
                    ps.Stop();
                }

                //Instead of the particle instantly destroying itself, itll take a second to look less awkward.
                Destroy(particleEffect, 1f);
            }
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

        private void HandleIncorrectAnswer()
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
            OnPlayerEnter?.Invoke();

            // If question is null get a question, else don't
            _question = (_question != null) ? _question : getQuestion();
        }

        public void Interact()
        {
            OnInteract?.Invoke();
            GameManager.MGR.OnArtifactInteraction(this);
        }

        public void PlayerExitRange()
        {
            OnPlayerExit?.Invoke();
            _playerHere = false;
        }

        private QuestionSO getQuestion()
        {
            switch (QuestionManager.MGR.QuestionLoadOrder)
            {
                default:
                case QuestionLoadOrder.RANDOM:
                    return QuestionManager.MGR.GetQuestion(_difficulty);

                case QuestionLoadOrder.INSPECTOR_INDEX:
                    return QuestionManager.MGR.GetQuestion(_difficulty, _index);

                case QuestionLoadOrder.INTERACTION_ORDER:
                    return QuestionManager.MGR.GetNextQuestion(_difficulty);
            }
        }
    }
}
