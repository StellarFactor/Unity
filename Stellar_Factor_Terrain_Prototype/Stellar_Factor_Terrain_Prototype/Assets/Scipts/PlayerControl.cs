using StellarFactor.Minimap;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace StellarFactor
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private Node lastDeathMinimapNode;
        [SerializeField] private StaticNodeColorsSO deathNodeColors;

        private FirstPersonController _controller;
        private DeathLocation lastDeathLocation;

        private int lockInteractionStack = 0;

        public Inventory Inventory { get; private set; }

        public bool PauseKeyPressed =>
            Input.GetKeyDown(GameManager.MGR.PauseKey);


        private void Awake()
        {
            _controller = GetComponent<FirstPersonController>();
            Inventory = GetComponent<Inventory>();
        }

        private void OnEnable()
        {
            GameManager.MGR.GamePaused += HandlePause;
            GameManager.MGR.GameResumed += HandleResume;
            GameManager.MGR.PanelCyclerInteractionStarted += HandlePanelCyclerInteraction;
            QuestionManager.MGR.WindowOpened += HandleQuestionWindowOpened;
            QuestionManager.MGR.WindowClosed += HandleQuestionWindowClosed;
        }


        private void OnDisable()
        {
            GameManager.MGR.GamePaused -= HandlePause;
            GameManager.MGR.GameResumed -= HandleResume;
            GameManager.MGR.PanelCyclerInteractionStarted -= HandlePanelCyclerInteraction;
            QuestionManager.MGR.WindowOpened -= HandleQuestionWindowOpened;
            QuestionManager.MGR.WindowClosed -= HandleQuestionWindowClosed;

            LockControls();
            lockInteractionStack = 0;
        }

        private void Update()
        {
            if (PauseKeyPressed)
            {
                GameManager.MGR.PauseGame();
            }
        }

        public void Die(Vector3 respawnPoint)
        {
            if (lastDeathLocation != null)
            {
                Node node = (lastDeathLocation as IMapLocation).InstantiatedNode;
                Destroy(node.gameObject);
            }

            lastDeathLocation = new DeathLocation(
                lastDeathMinimapNode,
                transform.position,
                deathNodeColors.StaticColor
                );
            MiniMap.MGR.InstantiateNodeAt(lastDeathLocation);

            Teleport(respawnPoint);
        }

        protected virtual void OnDeath()
        {
            GameManager.MGR.PlayerDeath();
        }

        public void Teleport(Vector3 position)
        {
            transform.position = position;
            Physics.SyncTransforms();
        }

        private void HandlePause()
        {
            RequestLockControls();
        }

        private void HandleResume()
        {
            RequestUnlockControls();
        }

        private void HandleQuestionWindowClosed()
        {
            RequestUnlockControls();
        }

        private void HandleQuestionWindowOpened()
        {
            RequestLockControls();
        }

        private void HandlePanelCyclerInteraction(PanelCycler cycler)
        {
            RequestLockControls();

            WaitThenDo waitForPanelCycler = new WaitThenDo(
                this,
                () => !cycler.IsRunning,
                () => false,
                () => RequestUnlockControls(),
                () => { });

            waitForPanelCycler.Start();
        }

        private void RequestLockControls()
        {
            lockInteractionStack++;
            Debug.LogWarning($"Requesting Lock Controls.  Lock Stack : {lockInteractionStack}");
            LockControls();
        }

        private bool RequestUnlockControls()
        {
            lockInteractionStack = (int)Mathf.Clamp(
                --lockInteractionStack,
                0,
                Mathf.Infinity
            );

            if (lockInteractionStack == 0)
            {
                UnlockControls();
                return true;
            }

            return false;
        }

        private void LockControls()
        {
            _controller.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void UnlockControls()
        {
            _controller.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
