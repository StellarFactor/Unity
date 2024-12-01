using UnityEngine;
using UnityEngine.Events;

namespace StellarFactor
{
    public class SelectableSprite : Spritebox
    {
        [SerializeField] private UnityEvent _additionalOnSelect;
        [SerializeField] private UnityEvent _additionalOnDeselct;

        [Space(20)]
        [SerializeField] private Sprite _highlightSprite;
        [SerializeField] private Sprite _selectedSprite;

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
            if (_highlightSprite != null)
            {                
                Sprite.Set(_highlightSprite);
            }

            SpriteColor.Set(_highlightColor);
        }

        public void Unhighlight()
        {
            Sprite.Reset();
            SpriteColor.Reset();
        }

        public void Select()
        {
            if (_selectedSprite != null)
            {
                Sprite.Set(_selectedSprite);
            }

            SpriteColor.Set(_selectedColor);

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