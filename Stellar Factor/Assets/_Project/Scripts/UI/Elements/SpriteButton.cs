using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace StellarFactor
{
    public class SpriteButton : Spritebox
    {
        [SerializeField] private Color _enabled;
        [SerializeField] private Color _highlight;

        bool _ready;

        protected override void OnEnable()
        {
            base.OnEnable();
            QuestionCanvas.MGR.SelectAnswer += onAnswerSelected;
            QuestionCanvas.MGR.UnselectAll += onUnselectAll;
        }

        private void OnDisable()
        {
            QuestionCanvas.MGR.SelectAnswer -= onAnswerSelected;
            QuestionCanvas.MGR.UnselectAll -= onUnselectAll;
        }

        private IEnumerator waitThenEnable()
        {
            yield return new WaitForSeconds(1f);

            SpriteColor.Set(_enabled);
            yield return null;
            _ready = true;
        }

        private void onAnswerSelected(int selectedIndex)
        {
            _ready = false;
            Debug.LogWarning($"hi");
            StartCoroutine(waitThenEnable());
        }

        private void onUnselectAll()
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

        public void OnClick()
        {
            if (!_ready) { return; }

            QuestionCanvas.MGR.UnselectAll.Invoke();
        }
    }
}
