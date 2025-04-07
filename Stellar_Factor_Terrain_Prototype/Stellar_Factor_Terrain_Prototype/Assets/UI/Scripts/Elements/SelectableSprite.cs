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
        
        public bool IsHighlighted { get; private set; }
        public bool IsSelected { get; private set; }

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

            IsSelected = true;
        }

        public void Deselect()
        {
            _additionalOnDeselct?.Invoke();

            IsSelected = false;
        }

        public void Toggle()
        {
            if (IsSelected)
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