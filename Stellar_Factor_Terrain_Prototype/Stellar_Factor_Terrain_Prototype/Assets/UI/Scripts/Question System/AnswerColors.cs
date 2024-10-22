using UnityEngine;

namespace StellarFactor
{
    public struct AnswerColors
    {
        public Color Correct { get; private set; }
        public Color Incorrect { get; private set; }
        public Color Highlight { get; private set; }
        public Color Selection { get; private set; }

        public AnswerColors(AnswerColorsSO scriptable)
        {
            Correct = scriptable.Correct;
            Incorrect = scriptable.Incorrect;
            Highlight = scriptable.Highlight;
            Selection = scriptable.Hint;
        }
    }
}
