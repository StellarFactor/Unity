using UnityEngine;

namespace StellarFactor
{
    public class LabeledProperty : MonoBehaviour
    {
        [SerializeField] Textbox _label;
        [SerializeField] Textbox _value;

        public Textbox Label { get { return _label; } }
        public Textbox Value { get { return _value; } }
    }
}
