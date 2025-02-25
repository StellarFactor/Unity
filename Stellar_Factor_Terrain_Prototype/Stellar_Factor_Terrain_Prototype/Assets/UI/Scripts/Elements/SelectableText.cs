using UnityEngine;
using UnityEngine.Events;

namespace StellarFactor
{
    public class SelectableText : Textbox
    {
        [SerializeField] private UnityEvent _additionalOnSelect;
        [SerializeField] private UnityEvent _additionalOnDeselct;

        [Space(20)]
        [SerializeField] private string _highlightMessage;
        [SerializeField] private string _selectedMessage;

        [SerializeField] private Color _highlightColor;
        [SerializeField] private Color _selectedColor;

        [SerializeField] private bool _startsSelected;
        private bool _selected = false;

        private void Start()
        {
            setDefaultState();
            Unhighlight();
        }

        private void setDefaultState()
        {
            if (_startsSelected)
            {
                Select();
            }
            else
            {
                Deselect();
            }
        }

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

            _additionalOnSelect?.Invoke();

            _selected = true;
        }

        public void Deselect()
        {
            _additionalOnDeselct?.Invoke();

            _selected = false;
        }

        public void Toggle()
        {
            if (_selected)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }
    }
}