using System.Collections;
using UnityEngine;


namespace StellarFactor
{
    public class SpriteButton : Spritebox
    {
        [SerializeField] private Color _enabled;
        [SerializeField] private Color _highlight;

        bool _ready;

        private void ResetButton()
        {
            SpriteColor.Reset();
            _ready = false;
        }

        public void OnHighlight()
        {
            if (!_ready) { return; }

            SpriteColor.Set(_highlight);
        }

        public void OnUnhighlight()
        {
            if (!_ready) { return; }

            SpriteColor.Revert();
        }
    }
}
