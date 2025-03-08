#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

namespace StellarFactor
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Prompts")]
        [SerializeField] private KeyCode interactKey;
        [SerializeField] private PromptWindow interactionPrompt;
        [SerializeField] private KeyCode pauseKey;
        [SerializeField] private PausePrompt pausePrompt;

        public KeyCode InteractKey { get { return interactKey; } }
        public KeyCode PauseKey { get { return pauseKey; } }
        public bool IsPaused { get; private set; }

        public event Action GamePaused;
        public event Action GameResumed;
        public event Action PlayerDied;
        public event Action Quit;
        public event Action<Artifact> ArtifactInteractionStarted;
        public event Action ArtifactInteractionCanceled;
        public event Action<PanelCycler> PanelCyclerInteractionStarted;


        private void Start()
        {
            interactionPrompt.ClosePrompt();
            pausePrompt.OpenPrompt();
        }

        public void PauseGame()
        {
            if (IsPaused) { return; }

            pausePrompt.ClosePrompt();

            IsPaused = true;
            pausePrompt.OpenPrompt();

            GamePaused?.Invoke();
        }
        public void ResumeGame()
        {
            if (!IsPaused) { return; }

            pausePrompt.OpenPrompt();

            IsPaused = false;
            pausePrompt.OpenPrompt();

            GameResumed?.Invoke();
        }
        public void QuitGame()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;

            #else
            Application.Quit();
            #endif
        }

        public void PlayerDeath()
        {
            PlayerDied?.Invoke();
        }
        public void StartArtifactInteraction(Artifact artifact)
        {
            ArtifactInteractionStarted?.Invoke(artifact);
        }
        public void CancelArtifactInteraction()
        {
            ArtifactInteractionCanceled?.Invoke();
        }

        public void StartPanelCyclerInteraction(PanelCycler cycler)
        {
            PanelCyclerInteractionStarted?.Invoke(cycler);
        }

        public bool RequestInteractionPrompt(string actionToPrompt)
        {
            if (interactionPrompt.IsOpen)
            {
                Debug.LogWarning(
                    $"Couldn't complete request to open an interaction prompt; " +
                    $"There is already an active interaction prompt.");

                return false;
            }

            interactionPrompt.OpenPrompt(actionToPrompt);
            return true;
        }

        public bool RequestCloseInteractionPrompt()
        {
            if (!interactionPrompt.IsOpen)
            {
                Debug.LogWarning(
                    $"Couldn't complete request to close interaction prompt; " +
                    $"Interaction prompt is not active.");

                return false;
            }

            interactionPrompt.ClosePrompt();
            return true;
        }
    }
}
