using UnityEngine;

namespace StellarFactor
{
    public class SelectableText : Textbox
    {
        [Space(20)]
        [SerializeField] private string _highlightMessage;
        [SerializeField] private string _selectedMessage;

        [SerializeField] private Color _highlightColor;
        [SerializeField] private Color _selectedColor;

        public void Highlight()
        {
            if (_highlightMessage != "")
            {
                Text.Set(_highlightMessage);
            }

            TextColor.Set(_highlightColor);
        }

        public void Unhighlight()
        {
            Text.Reset();
            TextColor.Reset();
        }

        public void Select()
        {
            if (_selectedMessage != null)
            {
                Text.Set(_selectedMessage);
            }

            TextColor.Set(_selectedColor);
        }
    }
}