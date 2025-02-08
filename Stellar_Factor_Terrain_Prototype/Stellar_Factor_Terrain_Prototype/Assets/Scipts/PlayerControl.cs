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
            GameManager.MGR.Pause += onPause;
            GameManager.MGR.Resume += onResume;
            GameManager.MGR.ArtifactInteraction += onArtifactInteraction;
            GameManager.MGR.CancelArtifactInteraction += onCancelArtifactInteraction;
            QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
        }


        private void OnDisable()
        {
            GameManager.MGR.ArtifactInteraction -= onArtifactInteraction;
            GameManager.MGR.CancelArtifactInteraction -= onCancelArtifactInteraction;
            QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
            QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
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

        public void Teleport(Vector3 position)
        {
            transform.position = position;
            Physics.SyncTransforms();
        }

        private void onPause()
        {
            lockControls();
        }

        private void onResume()
        {
            unlockControls();
        }

        private void onArtifactInteraction(Artifact artifact)
        {
            lockControls();
        }

        private void onCancelArtifactInteraction()
        {
            unlockControls();
        }

        private void onCorrectAnswer()
        {
            unlockControls();

            // TODO:
            // Add "artifact" to "inventory" (probably
            // just flip a bool? lol)
        }

        private void onIncorrectAnswer()
        {
            // TODO:
            // Lose health? Anything else?
        }

        protected virtual void OnDeath()
        {
            GameManager.MGR.OnPlayerDeath();
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