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
            QuestionCanvas.MGR.SelectAnswer += onAnswerSelected;
            QuestionCanvas.MGR.UnselectAll += onUnselectAll;
            _ready = true;
        }

        private void OnDisable()
        {
            _ready = false;
            QuestionCanvas.MGR.SelectAnswer -= onAnswerSelected;
            QuestionCanvas.MGR.UnselectAll -= onUnselectAll;
        }

        private void onAnswerSelected(int selectedIndex)
        {
            _ready = false;
            if (_answer.Correct)
            {
                Value.TextColor.Set(_colors.Correct);
            }
            else if (selectedIndex == _index)
            {
                Value.TextColor.Set(_colors.Selection);
            }
            else
            {
                Value.TextColor.Set(_colors.Incorrect);
            }
        }

        private void onUnselectAll()
        {
            Label.TextColor.Reset();
            Value.TextColor.Reset();
            _ready = true;
        }

        public void SetWith(Answer answer, AnswerColors colors, int index)
        {
            _answer = answer;
            _colors = colors;
            _index = index;

            Label.Text.Set(index.ToString());
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

            QuestionCanvas.MGR.SelectAnswer.Invoke(_index);
        }

    }
}
