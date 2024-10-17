using UnityEngine;

namespace StellarFactor
{
    [CreateAssetMenu(fileName = "New Answer Colors", menuName = "Answer Color Set")]
    public class AnswerColorsSO : ScriptableObject
    {
        public Color Correct;
        public Color Incorrect;
        public Color Highlight;
        public Color Hint;
    }
}
