using UnityEngine;

namespace StellarFactor
{
    [System.Serializable]
    public class Logger
    {
        public bool showDebug = true;

        public Logger() : this(true) { }

        public Logger(bool showDebug)
        {
            this.showDebug = showDebug;
        }

        public void Print(string msg)
        {
            if (!showDebug) { return; }
            Debug.Log(msg);
        }

        public void Warn(string msg)
        {
            if (!showDebug) { return; }
            Debug.LogWarning(msg);
        }

        public void Throw(string msg)
        {
            if (!showDebug) { return; }
            Debug.LogException(new System.Exception(msg));
        }
    }
}
