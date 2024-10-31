using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace StellarFactor
{
    public class Textbox : MonoBehaviour
    {
        Text _text;

        public ClassSwitcher<string> Text;
        public StructSwitcher<Color> TextColor;

        private void OnEnable()
        {
            findComponents();
            setDefaults();
        }

        private void findComponents()
        {
            _text = GetComponentInChildren<Text>();

            if (_text == null) { Debug.LogWarning($"No text found on {name}"); }
        }

        private void setDefaults()
        {
            if (Text.Get() == "")
            {
                Text = new ClassSwitcher<string>("No Value Set");
            }

            if (TextColor.Get().a == 0)
            {
                TextColor = new StructSwitcher<Color>(_text.color);
            }
        }

        private void Start()
        {

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
