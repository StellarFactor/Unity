#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

namespace StellarFactor
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private KeyCode _interactKey;
        [SerializeField] private KeyCode _pauseKey;

        private int _currentLevel;

        public int CurrentLevel { get { return _currentLevel; } }
        public KeyCode InteractKey { get { return _interactKey; } }
        public KeyCode PauseKey { get { return _pauseKey; } }

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

        public void PauseGame()
        {
            Pause?.Invoke();
        }

        public void ResumeGame()
        {
            Resume?.Invoke();
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;

            #else
            Application.Quit();

            #endif
        }
    }

}
