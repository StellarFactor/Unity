using UnityEngine;

namespace StellarFactor
{
    public class SelectableSprite : Spritebox
    {
        [Space(20)]
        [SerializeField] private Sprite _highlightSprite;
        [SerializeField] private Sprite _selectedSprite;

        [SerializeField] private Color _highlightColor;
        [SerializeField] private Color _selectedColor;

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
        }
    }
}