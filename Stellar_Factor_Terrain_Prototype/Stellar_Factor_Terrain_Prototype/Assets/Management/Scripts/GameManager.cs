using System;
using UnityEngine;

namespace StellarFactor
{
    public class GameManager : Singleton<GameManager>
    {
        private int _currentLevel;

        public int CurrentLevel { get { return _currentLevel; } }

        public Action<int> LevelLoaded;
        public Action Pause;
        public Action Resume;
        public Action PlayerDeath;
        public Action<Artifact> ArtifactFound;

        private void Start()
        {
            LevelLoaded.Invoke(CurrentLevel);
        }
    }

}
