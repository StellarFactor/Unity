#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StellarFactor
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("= Scenes ====")]
        [SerializeField] private int levelOneBuildIndex;
        [SerializeField] private int levelTwoBuildIndex;

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

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
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

        public bool RequestSimplePrompt(string message)
        {
            if (promptsCanvas.InteractionPromptWindow.IsOpen)
            {
                Debug.LogWarning(
                    $"Couldn't complete request to open an interaction prompt; " +
                    $"There is already an active interaction prompt.");

                return false;
            }

            promptsCanvas.InteractionPromptWindow.OpenPrompt(message);
            return true;
        }

        public bool RequestClosePrompt()
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

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            int ind = scene.buildIndex;
            if (ind != levelOneBuildIndex && ind != levelTwoBuildIndex) { return; }

            promptsCanvas.PausePromptWindow.OpenPrompt(pauseKey, "Pause");
        }
    }
}
