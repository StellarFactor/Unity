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
            lockControls();
        }

        private void HandleResume()
        {
            unlockControls();
        }

        private void HandleArtifactInteraction(Artifact artifact)
        {
            lockControls();
        }

        private void HandleCancelArtifactInteraction()
        {
            unlockControls();
        }

        private void HandleCorrectAnswer()
        {
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
            lockControls();
            WaitThenDo waitForFinish = new(this, () => !cycler.IsRunning, unlockControls);
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