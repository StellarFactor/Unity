using System;
using UnityEditor;
using UnityEngine;

namespace StellarFactor
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private KeyCode _interactKey;
        [SerializeField] private KeyCode _quitKey;

        private int _currentLevel;

        public int CurrentLevel { get { return _currentLevel; } }
        public KeyCode InteractKey { get { return _interactKey; } }
        public KeyCode QuitKey { get { return _quitKey; } }

        public Action<int> LevelLoaded;
        public Action Pause;
        public Action Resume;
        public Action PlayerDeath;
        public Action Quit;
        public Action<Artifact> ArtifactInteraction;
        public Action CancelArtifactInteraction;

        private void Start()
        {
            LevelLoaded.Invoke(CurrentLevel);
        }
    }

}
