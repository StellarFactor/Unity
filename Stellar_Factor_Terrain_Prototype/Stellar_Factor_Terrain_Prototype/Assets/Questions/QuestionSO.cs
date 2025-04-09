using System;
using UnityEngine;

namespace StellarFactor
{
    public enum QuestionGivenBy { ARTIFACT, PEDESTAL, BOSS };

    [CreateAssetMenu(fileName = "New Question", menuName = "Question")]
    public class QuestionSO : ScriptableObject
    {
        [SerializeField, TextArea(minLines:3, maxLines:20)] private string _text;

        [SerializeField] private Answer[] _answers;

        public string Text { get { return _text; } }
        public Answer[] Answers { get { return _answers; } }

        /// <summary>
        /// To be set by whatever <see cref="IInteractable"/> object owns
        /// the question at the time that it is about to try to open the
        /// <see cref="QuestionPanel"/>.
        /// This way, <see cref="QuestionManager"/> can decide what to send to the
        /// <see cref="ResponsePanel"/>. It's convoluted and awful but we are
        /// out of time to restructure this. Godspeed, future devs.
        /// We salute you.
        /// </summary>
        public QuestionGivenBy QuestionGivenBy { get; set; } = QuestionGivenBy.ARTIFACT;

        /// <summary>
        /// To be called when the question is about to be displayed in the
        /// <c><see cref="ResponsePanel"/></c>
        /// </summary>
        public void OnLoadIntoWindow()
        {
            QuestionManager.MGR.WindowClosed += HandleWindowClosed;
        }

        private void HandleWindowClosed()
        {
            QuestionManager.MGR.WindowClosed -= HandleWindowClosed;

            // Reset to default.
            QuestionGivenBy = QuestionGivenBy.ARTIFACT;
        }
    }
}
