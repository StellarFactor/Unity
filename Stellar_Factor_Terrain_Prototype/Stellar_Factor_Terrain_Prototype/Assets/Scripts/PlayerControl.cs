using StellarFactor.Minimap;
using System.Collections.Generic;
using System.Linq;
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

        private bool wasRecentAnswerCorrect;
        private IAcquirable answeringToAcquire;
        private QuestionSO currentlyAnswering;

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
            QuestionManager.MGR.QuestionStarted += HandleQuestionStarted;
            QuestionManager.MGR.QuestionAnswered += HandleQuestionAnswered;
            QuestionManager.MGR.WindowOpened += HandleQuestionWindowOpened;
            QuestionManager.MGR.WindowClosed += HandleQuestionWindowClosed;
            if (PedestalManager.MGR != null)
            {
                PedestalManager.MGR.PedestalCompleted += HandlePedestalCompleted;
            }
        }


        private void OnDisable()
        {
            GameManager.MGR.GamePaused -= HandlePause;
            GameManager.MGR.GameResumed -= HandleResume;
            GameManager.MGR.PanelCyclerInteractionStarted -= HandlePanelCyclerInteraction;
            QuestionManager.MGR.QuestionAnswered -= HandleQuestionAnswered;
            QuestionManager.MGR.WindowOpened -= HandleQuestionWindowOpened;
            QuestionManager.MGR.WindowClosed -= HandleQuestionWindowClosed;
            if (PedestalManager.MGR != null)
            {
                PedestalManager.MGR.PedestalCompleted -= HandlePedestalCompleted;
            }

            LockControls();
            lockInteractionStack = 0;
        }

        private void Update()
        {
            if (PauseKeyPressed)
            {
                GameManager.MGR.PauseGame();
            }
// This can be turned on in Project Settings > Player > Other > Scripting Define Symbols
#if DEBUG_KEYS
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.B))
                {
                    Boss boss = FindObjectOfType<Boss>();
                    transform.position = boss.transform.position + new Vector3(5f, 5f, 0f);
                }
#endif
        }

        protected void HandleQuestionStarted(QuestionSO question)
        {
            currentlyAnswering = question;
        }

        protected void HandleQuestionAnswered(bool correctly, IAcquirable toAcquire)
        {
            wasRecentAnswerCorrect = correctly;
            answeringToAcquire = toAcquire;
        }

        protected void HandlePedestalCompleted(Pedestal pedestal)
        {
            List<Artifact> currentArtifacts =
                Inventory
                .GetCurrentItemsOfType(typeof(Artifact))
                .Cast<Artifact>()
                .ToList();

            if (currentArtifacts.Count <= 0)
            {
                Debug.LogWarning(
                    $"<color=red>No artifacts remaining to place. " +
                    $"Access should not have been granted to this room " +
                    $"with less than 8 Artifacts in player inventory.</color>");
                return;
            }

            pedestal.Place(currentArtifacts[0]);
        }

        public void Acquire(IAcquirable item)
        {
            if (item == null) { return; }
            if (Inventory.AcquireItem(item))
            {
                Debug.Log($"{name} acquired {item}.");
            }
            else
            {
                Debug.LogWarning($"{name} couldn't acquire {item}.");
            }
        }

        public void Drop(IAcquirable item)
        {
            if (item == null) { return; }
            if (Inventory.RemoveItem(item))
            {
                Debug.Log($"{name} dropped {item}.");
            }
            else
            {
                Debug.LogWarning($"{name} couldn't drop {item}.");
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

            if (currentlyAnswering?.QuestionGivenBy == QuestionGivenBy.ARTIFACT
                && wasRecentAnswerCorrect)
            {
                Acquire(answeringToAcquire);
            }
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
            Debug.Log($"Requesting Lock Controls.  Lock Stack : {lockInteractionStack}");
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
