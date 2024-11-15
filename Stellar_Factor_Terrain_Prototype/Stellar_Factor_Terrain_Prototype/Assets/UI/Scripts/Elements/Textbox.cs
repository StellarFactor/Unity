using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace StellarFactor
{
    public class Textbox : MonoBehaviour
    {
        TMP_Text _text;

        public ClassSwitcher<string> Text;
        public StructSwitcher<Color> TextColor;

        private void Awake()
        {
            findComponents();
            setDefaults();
        }

        private void OnEnable()
        {
            _text.enabled = true;
        }

        private void OnDisable()
        {
            _text.enabled = false;
        }

        private void findComponents()
        {
            _text = GetComponentInChildren<TMP_Text>();

            if (_text == null) { Debug.LogWarning($"No text found on {name}"); }
        }

        private void setDefaults()
        {
            if (Text.Default == "")
            {
                Text = new ClassSwitcher<string>(_text.text);
            }

            if (TextColor.Default.a == 0)
            {
                TextColor = new StructSwitcher<Color>(_text.color);
            }
        }

        private void Update()
        {
            _text.text = Text.Get();
            _text.color = TextColor.Get();
        }

        public void ResetAll()
        {
            Text.Reset();
            TextColor.Reset();
        }

        public void RevertAll()
        {
            Text.Revert();
            TextColor.Revert();
        }
    }
}
