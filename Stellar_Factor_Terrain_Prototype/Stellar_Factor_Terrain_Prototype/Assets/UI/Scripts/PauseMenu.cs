using UnityEngine;

namespace StellarFactor
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        public bool Initialized { get; private set; }

        private void OnEnable()
        {
            GameManager.MGR.Pause += onPause;
            GameManager.MGR.Resume += onResume;
        }

        private void OnDisable()
        {
            GameManager.MGR.Pause -= onPause;
            GameManager.MGR.Resume -= onResume;
        }

        private void Start()
        {
            _canvas.enabled = false;
        }

        private void onPause()
        {
            _canvas.enabled = true;
        }

        private void onResume()
        {
            _canvas.enabled = false;
        }
    }
}