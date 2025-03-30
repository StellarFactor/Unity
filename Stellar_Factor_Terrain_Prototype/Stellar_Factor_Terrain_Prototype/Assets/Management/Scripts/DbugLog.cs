using UnityEngine;

namespace StellarFactor
{
    [System.Serializable]
    public class DbugLog
    {
        public bool showDebug = true;

        public DbugLog() : this(true) { }

        public DbugLog(bool showDebug)
        {
            this.showDebug = showDebug;
        }

        public void Print(string msg)
        {
            if (!showDebug) { return; }
            Debug.Log(msg);
        }

        public void Print(string msg, Object context)
        {
            if (!showDebug) { return; }

            Debug.Log(msg, context);
        }

        public void Warn(string msg)
        {
            if (!showDebug) { return; }
            Debug.LogWarning(msg);
        }

        public void Warn(string msg, Object context)
        {
            if (!showDebug) { return; }

            Debug.LogWarning(msg, context);
        }

        public void Error(string msg)
        {
            if (!showDebug) { return; }
            Debug.LogError(msg);
        }

        public void Error(string msg, Object context)
        {
            if (!showDebug) { return; }

            Debug.LogError(msg, context);
        }
    }
}
