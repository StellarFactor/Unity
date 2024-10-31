using UnityEngine;

namespace StellarFactor
{
    public class AnswerField : LabeledProperty
    {
        private Answer _answer;
        private AnswerColors _colors;
        private int _index;

        bool _ready;

        private void OnEnable()
        {
            QuestionManager.MGR.Clear += onClear;
            QuestionManager.MGR.SelectAnswer += onAnswerSelected;
            _ready = true;
        }

        private void OnDisable()
        {
            _ready = false;
            QuestionManager.MGR.Clear -= onClear;
            QuestionManager.MGR.SelectAnswer -= onAnswerSelected;
        }

        private void onClear()
        {
            Label.TextColor.Reset();
            Value.TextColor.Reset();
            _ready = true;
        }

        private void onAnswerSelected(int selectedIndex)
        {
            _ready = false;
            Color color = _answer.Correct ? _colors.Correct : _colors.Incorrect;

            if (selectedIndex == _index)
            {                
                if (_answer.Correct)
                {
                    QuestionManager.MGR.CorrectAnswer.Invoke();
                }
                else
                {
                    color = _colors.Selection;
                    QuestionManager.MGR.IncorrectAnswer.Invoke();
                }
            }

            Value.TextColor.Set(color);
        }

        public void FillWith(Answer answer, AnswerColors colors, int index)
        {
            _answer = answer;
            _colors = colors;
            _index = index;

            Label.Text.Set(((char)(index + 65)).ToString());
            Value.Text.Set(_answer.AnswerText);
        }

        public void OnHighlight()
        {
            if (!_ready) { return; }

            Value.TextColor.Set(_colors.Highlight);
        }

        public void OnUnhighlight()
        {
            if (!_ready) { return; }

            Value.TextColor.Reset();
        }

        public void OnClick()
        {
            if (!_ready) { return; }
            _ready = false;

            QuestionManager.MGR.SelectAnswer.Invoke(_index);
        }
    }
}
