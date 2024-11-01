using System;
using UnityEngine;

namespace StellarFactor
{
    [Serializable]
    public class ClassSwitcher<T> where T : class
    {
        [SerializeField] private T _default;
        private T _current;
        private T _buffer;

        public T Default { get { return _default; } }

        public ClassSwitcher(T defaultT)
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

        public void Set(T obj)
        {
            _buffer = _current;
            _current = obj;
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
