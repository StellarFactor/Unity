using StellarFactor.Minimap;
using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace StellarFactor
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private Node lastDeathMinimapNode;
        [SerializeField] private StaticNodeColorsSO deathNodeColors;

        private FirstPersonController _controller;
        private DeathLocation lastDeathLocation;

        private int lockInteractionStack = 0;

        public bool PauseKeyPressed =>
            Input.GetKeyDown(GameManager.MGR.PauseKey);


        private void Awake()
        {
            _controller = GetComponent<FirstPersonController>();
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
            requestLockControls();
        }

        private void HandleResume()
        {
            requestUnlockControls();
        }

        private void HandleQuestionWindowClosed()
        {
            requestUnlockControls();
        }

        private void HandleQuestionWindowOpened()
        {
            requestLockControls();
        }

        //private void HandleArtifactInteraction(Artifact artifact)
        //{
        //}

        //private void HandleCancelArtifactInteraction()
        //{
        //}

        // TODO:
        // Probably should add an Artifact param to the correctAnswer event
        // and add "artifact" to "inventory"
        // but I think this script doesn't need to concern itself with
        // whether the player got the question right or not, just whether any UI
        // windows are open.
        //private void HandleCorrectAnswer()
        //{
        //}

        //private void HandleIncorrectAnswer()
        //{
        //    // TODO:
        //    // Lose health? Anything else?
        //}

        private void HandlePanelCyclerInteraction(PanelCycler cycler)
        {
            requestLockControls();

            WaitThenDo waitForPanelCycler = new WaitThenDo(
                this,
                () => !cycler.IsRunning,
                () => false,
                () => requestUnlockControls(),
                () => { });

            waitForPanelCycler.Start();
        }

        private void requestLockControls()
        {
            lockInteractionStack++;
            Debug.LogWarning($"Requesting Lock Controls.  Lock Stack : {lockInteractionStack}");
            lockControls();
        }

        private bool requestUnlockControls()
        {
            lockInteractionStack = (int)Mathf.Clamp(
                --lockInteractionStack,
                0,
                Mathf.Infinity
            );

            if (lockInteractionStack == 0)
            {
                unlockControls();
                return true;
            }

            return false;
        }

        private void lockControls()
        {
            _controller.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void unlockControls()
        {
            _controller.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}