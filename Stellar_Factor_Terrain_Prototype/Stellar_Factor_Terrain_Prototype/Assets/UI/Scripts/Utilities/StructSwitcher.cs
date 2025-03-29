using UnityEngine;

namespace StellarFactor
{
    [System.Serializable]
    public class StructSwitcher<T> where T : struct
    {
        DbugLog log = new();
        [SerializeField] private T _default;
        private T _current;
        private T _buffer;

        public T Default { get { return _default; } }

        public StructSwitcher()
        {
            _default = default;
            _current = _default;
            _buffer = _default;
        }

        public StructSwitcher(T defaultT)
        {
            _default = defaultT;
            _current = _default;
            _buffer = _default;
        }

        public void Reset()
        {
            _buffer = _current;
            _current = _default;
        }

        public T Get()
        {
            return _current;
        }

        public void Set(T newVal)
        {
            _buffer = _current;
            _current = newVal;
        }

        public void Revert()
        {
            T temp = _current;
            _current = _buffer;
            _buffer = temp;
        }
    }
}
