using StellarFactor.Minimap;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace StellarFactor
{
    public class PlayerControl : MonoBehaviour
    {
        private FirstPersonController _controller;

        [SerializeField] private Node lastDeathMinimapNode;
        [SerializeField] private StaticNodeColorsSO deathNodeColors;
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
            GameManager.MGR.ArtifactInteraction += HandleArtifactInteraction;
            GameManager.MGR.CancelArtifactInteraction += HandleCancelArtifactInteraction;
            GameManager.MGR.PanelCyclerInteraction += HandlePanelCyclerInteraction;
            QuestionManager.MGR.CorrectAnswer += HandleCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += HandleIncorrectAnswer;
        }


        private void OnDisable()
        {
            GameManager.MGR.ArtifactInteraction -= HandleArtifactInteraction;
            GameManager.MGR.CancelArtifactInteraction -= HandleCancelArtifactInteraction;
            GameManager.MGR.PanelCyclerInteraction -= HandlePanelCyclerInteraction;
            QuestionManager.MGR.CorrectAnswer -= HandleCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= HandleIncorrectAnswer;
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
            lockInteractionStack++;
            lockControls();
        }

        private void HandleResume()
        {
            if (--lockInteractionStack > 0) { return; }

            unlockControls();
        }

        private void HandleArtifactInteraction(Artifact artifact)
        {
            lockInteractionStack++;

            lockControls();
        }

        private void HandleCancelArtifactInteraction()
        {
            if (--lockInteractionStack > 0) { return; }

            unlockControls();
        }

        private void HandleCorrectAnswer()
        {
            if (--lockInteractionStack > 0) { return; }

            unlockControls();

            // TODO:
            // Add "artifact" to "inventory" (probably
            // just flip a bool? lol)
        }

        private void HandleIncorrectAnswer()
        {
            // TODO:
            // Lose health? Anything else?
        }

        private void HandlePanelCyclerInteraction(PanelCycler cycler)
        {
            lockInteractionStack++;

            lockControls();

            void tryDecrementedUnlock() {
                if (--lockInteractionStack > 0) { return; }
                unlockControls();
            }

            WaitThenDo waitForFinish = new(
                this,
                () => !cycler.IsRunning,
                tryDecrementedUnlock);

            waitForFinish.Start();
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