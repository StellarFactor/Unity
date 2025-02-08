using UnityEngine;
using UnityEngine.UI;

namespace StellarFactor.Minimap
{
    [RequireComponent(
        typeof(RectTransform),
        typeof(Image))]
    public class Node : MonoBehaviour
    {
        public Logger log = new();

        private RectTransform rt;
        private Image image;

        private StructSwitcher<Color> color;

        public RectTransform RT { get { return rt; } }

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            this.color = new(image.color);
        }

        private void Update()
        {
            UpdateColor();
        }

        public void SetColor(Color color)
        {
            this.color.Set(color);
        }

        private void UpdateColor()
        {
            if (image.color == color.Get()) { return; }
            
            image.color = color.Get();
        }
    }
}