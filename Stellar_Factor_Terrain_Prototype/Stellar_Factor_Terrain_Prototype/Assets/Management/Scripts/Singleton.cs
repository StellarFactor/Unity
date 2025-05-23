using UnityEngine;

namespace StellarFactor
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T MGR { get { return _instance; } }

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
            }
        }
    }
}