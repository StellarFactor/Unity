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

        private void Awake()
        {
            _controller = GetComponent<FirstPersonController>();
        }

        private void OnEnable()
        {
            GameManager.MGR.Pause += HandlePause;
            GameManager.MGR.Resume += HandleResume;
            GameManager.MGR.PanelCyclerInteraction += HandlePanelCyclerInteraction;
            QuestionManager.MGR.WindowOpened += HandleQuestionWindowOpened;
            QuestionManager.MGR.WindowClosed += HandleQuestionWindowClosed;
        }


        private void OnDisable()
        {
            GameManager.MGR.Pause -= HandlePause;
            GameManager.MGR.Resume -= HandleResume;
            GameManager.MGR.PanelCyclerInteraction -= HandlePanelCyclerInteraction;
            QuestionManager.MGR.WindowOpened -= HandleQuestionWindowOpened;
            QuestionManager.MGR.WindowClosed -= HandleQuestionWindowClosed;
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
            GameManager.MGR.OnPlayerDeath();
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
            lockControls();
        }

        private bool requestUnlockControls()
        {
            if (--lockInteractionStack <= 0)
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