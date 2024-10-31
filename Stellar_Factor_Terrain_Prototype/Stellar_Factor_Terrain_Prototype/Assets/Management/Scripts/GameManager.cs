using System;
using UnityEngine;

namespace StellarFactor
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private KeyCode _interactKey;

        private int _currentLevel;

        public int CurrentLevel { get { return _currentLevel; } }
        public KeyCode InteractKey { get { return _interactKey; } }

        public Action<int> LevelLoaded;
        public Action Pause;
        public Action Resume;
        public Action PlayerDeath;
        public Action<Artifact> ArtifactInteraction;

        private void Start()
        {
            LevelLoaded.Invoke(CurrentLevel);
        }
    }

}
