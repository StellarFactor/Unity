using UnityEngine;
using UnityEngine.UI;

namespace StellarFactor
{
    public class Spritebox : MonoBehaviour
    {
        Image _image;

        public ClassSwitcher<Sprite> Sprite;
        public StructSwitcher<Color> SpriteColor;

        protected virtual void Awake()
        {
            findComponents();
            setDefaults();
        }

        private void OnEnable()
        {
            _image.enabled = true;
        }

        private void OnDisable()
        {
            _image.enabled = false;
        }

        private void findComponents()
        {
            _image = GetComponentInChildren<Image>();

            if (_image == null) { Debug.LogWarning($"No img found on {name}"); }
        }

        private void setDefaults()
        {
            if (Sprite.Get() == null)
            {
                Sprite = new ClassSwitcher<Sprite>(_image.sprite);
            }

            if (SpriteColor.Get().a == 0)
            {
                SpriteColor = new StructSwitcher<Color>(_image.color);
            }
        }

        private void Start()
        {

        }

        private void Update()
        {
            _image.sprite = Sprite.Get();
            _image.color = SpriteColor.Get();
        }

        public void ResetAll()
        {
            Sprite.Reset();
            SpriteColor.Reset();
        }

        public void RevertAll()
        {
            Sprite.Revert();
            SpriteColor.Revert();
        }
    }
}
