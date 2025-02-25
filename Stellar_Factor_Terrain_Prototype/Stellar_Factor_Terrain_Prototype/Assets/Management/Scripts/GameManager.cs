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

        public event Action<int> LevelLoaded;
        public event Action Pause;
        public event Action Resume;
        public event Action PlayerDeath;
        public event Action Quit;
        public event Action<Artifact> ArtifactInteraction;
        public event Action CancelArtifactInteraction;
        public event Action<PanelCycler> PanelCyclerInteraction;

        private void Start()
        {
            LevelLoaded?.Invoke(CurrentLevel);
        }

        public void OnPauseGame()
        {
            Pause?.Invoke();
        }
        public void OnResumeGame()
        {
            Resume?.Invoke();
        }
        public void OnPlayerDeath()
        {
            PlayerDeath?.Invoke();
        }
        public void OnArtifactInteraction(Artifact artifact)
        {
            ArtifactInteraction?.Invoke(artifact);
        }
        public void OnCancelArtifactInteraction()
        {
            CancelArtifactInteraction?.Invoke();
        }
        public void OnLevelLoaded(int level)
        {
            LevelLoaded?.Invoke(level);
        }
        public void OnPause()
        {
            Pause?.Invoke();
        }
        public void OnResume()
        {
            Resume?.Invoke();
        }
        //public void OnQuit()
        //{
        //    Quit?.Invoke();
        //}
        public void OnPanelCyclerInteraction(PanelCycler cycler)
        {
            PanelCyclerInteraction?.Invoke(cycler);
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
