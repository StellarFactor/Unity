using System.Runtime.CompilerServices;
using UnityEngine;

namespace StellarFactor
{
    public class AnswerField : LabeledProperty
    {
        private Answer _answer;
        private AnswerColors _colors;
        private int _index;

        private bool _showCorrect;

        private bool _ready;
        private bool _mouseHere;

        private Color CorrectColor => _colors.Correct;
        private Color IncorrectColor => _showCorrect
            ? _colors.Incorrect
            : _colors.Hint;

        private void OnEnable()
        {
            _ready = true;
        }

        private void OnDisable()
        {
            _ready = false;
        }

        public void FillWith(Answer answer, AnswerColors colors, int index)
        {
            _answer = answer;
            _colors = colors;
            _index = index;

            Label.Text.Set(((char)(index + 65)).ToString());
            Value.Text.Set(_answer.AnswerText);
        }

        public void Clear()
        {
            Label.TextColor.Reset();
            Value.TextColor.Reset();

            _ready = true;

            if (_mouseHere)
            {
                OnHighlight();
            }
        }

        public void OnHighlight()
        {
            _mouseHere = true;
            if (!_ready) { return; }

            Value.TextColor.Set(_colors.Highlight);
        }

        public void OnUnhighlight()
        {
            _mouseHere = false;
            if (!_ready) { return; }

            Value.TextColor.Reset();
        }

        public void OnClick()
        {
            if (!_ready) { return; }

            _ready = false;

            Color color = _answer.Correct
                ? CorrectColor
                : IncorrectColor;

            QuestionManager.MGR.AnswerQuestion(_answer.Correct);
            
            Value.TextColor.Set(color);
        }
    }
}
