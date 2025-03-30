#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

namespace StellarFactor
{
public class GameManager : Singleton<GameManager>
    {
        [Header("= Spawning ====")]
        [SerializeField] private Transform startingSpawnPos;
        [SerializeField] private bool overrideStartingSpawnPos;

        [Header("= Prompts ====")]
        [SerializeField] private GameObject promptCanvasPrefab;
        [SerializeField] private PromptsCanvas promptsCanvas;
        [Space(5)]
        [SerializeField] private KeyCode interactKey;
        [Space(5)]
        [SerializeField] private KeyCode pauseKey;

        [Header("= Inventory ====")]
        [SerializeField] private GameObject inventoryPrefab;


        public KeyCode InteractKey { get { return interactKey; } }
        public KeyCode PauseKey { get { return pauseKey; } }
        public bool IsPaused { get; private set; }

        public event Action GamePaused;
        public event Action GameResumed;
        public event Action PlayerDied;
        public event Action Quit;
        public event Action<PanelCycler> PanelCyclerInteractionStarted;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            if (promptsCanvas == null)
            {
                promptsCanvas = Instantiate(promptCanvasPrefab).GetComponent<PromptsCanvas>();
            }

            if (ArtifactInventoryUI.MGR == null)
            {
                Instantiate(inventoryPrefab);
            }

            promptsCanvas.InteractionPromptWindow.ClosePrompt();
            promptsCanvas.PausePromptWindow.OpenPrompt(pauseKey, "Pause");
            if (!overrideStartingSpawnPos)
            {
                FindObjectOfType<PlayerControl>().transform.position = startingSpawnPos.position;
            }
        }

        public void PauseGame()
        {
            if (IsPaused) { return; }

            promptsCanvas.PausePromptWindow.ClosePrompt();
            IsPaused = true;

            GamePaused?.Invoke();
        }
        public void ResumeGame()
        {
            if (!IsPaused) { return; }

            promptsCanvas.PausePromptWindow.OpenPrompt(pauseKey, "Pause");
            IsPaused = false;

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

        public void StartPanelCyclerInteraction(PanelCycler cycler)
        {
            PanelCyclerInteractionStarted?.Invoke(cycler);
        }

        public bool RequestInteractionPrompt(string actionToPrompt)
        {
            if (promptsCanvas.InteractionPromptWindow.IsOpen)
            {
                Debug.LogWarning(
                    $"Couldn't complete request to open an interaction prompt; " +
                    $"There is already an active interaction prompt.");

                return false;
            }

            promptsCanvas.InteractionPromptWindow.OpenPrompt(interactKey, actionToPrompt);
            return true;
        }

        public bool RequestCloseInteractionPrompt()
        {
            if (!promptsCanvas.InteractionPromptWindow.IsOpen)
            {
                Debug.LogWarning(
                    $"Couldn't complete request to close interaction prompt; " +
                    $"Interaction prompt is not active.");

                return false;
            }

            promptsCanvas.InteractionPromptWindow.ClosePrompt();
            return true;
        }
    }
}
