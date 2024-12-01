using UnityEngine;

namespace StellarFactor
{
    [System.Serializable]
    public class StructSwitcher<T> where T : struct
    {
        [SerializeField] private T _default;
        private T _current;
        private T _buffer;

        public T Default { get { return _default; } }

        public StructSwitcher(T defaultT)
        {
            init(defaultT);
        }

        private void init(T defaultT)
        {
            _default = defaultT;
            _current = defaultT;
            _buffer = defaultT;
        }

        public void Reset()
        {
            _buffer = _current;
            _current = _default;
        }

        public void Set(T color)
        {
            _buffer = _current;
            _current = color;
        }
        public void Revert()
        {
            T temp = _current;
            _current = _buffer;
            _buffer = temp;
        }

        public T Get()
        {
            return _current;
        }
    }
}
