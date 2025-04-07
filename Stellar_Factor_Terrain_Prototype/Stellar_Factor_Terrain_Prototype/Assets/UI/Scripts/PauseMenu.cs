using UnityEngine;

namespace StellarFactor
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private void OnEnable()
        {
            GameManager.MGR.GamePaused += onPause;
            GameManager.MGR.GameResumed += onResume;
        }

        private void OnDisable()
        {
            GameManager.MGR.GamePaused -= onPause;
            GameManager.MGR.GameResumed -= onResume;
        }

        private void Start()
        {
            canvas.enabled = false;
        }

        private void onPause()
        {
            canvas.enabled = true;
        }

        private void onResume()
        {
            canvas.enabled = false;
        }
    }
}