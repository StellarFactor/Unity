#if UNITY_EDITOR
#endif

using UnityEngine;

namespace StellarFactor
{
    public class PausePrompt : MonoBehaviour
    {
        [SerializeField] Textbox _pauseText;
        [SerializeField] Spritebox _background;

        private bool _isPaused;
        private bool _canPause;

        private void OnEnable()
        {
            GameManager.MGR.LevelLoaded += onLevelLoaded;
            GameManager.MGR.Pause += onPause;
            GameManager.MGR.Resume += onResume;
        }

        private void OnDisable()
        {
            GameManager.MGR.LevelLoaded -= onLevelLoaded;
            GameManager.MGR.Pause -= onPause;
            GameManager.MGR.Resume -= onResume;
        }

        private void Start()
        {
            _canPause = true;
        }

        private void Update()
        {
            if (!_canPause) { return; }

            if (Input.GetKeyDown(GameManager.MGR.PauseKey))
            {
                if (_isPaused)
                {
                    GameManager.MGR.OnResume();
                }
                else
                {
                    GameManager.MGR.OnPause();
                }
            }
        }

        private void onLevelLoaded(int obj)
        {
            string message = _pauseText.Text.Get();

            message = message.Replace("<>", $"{GameManager.MGR.PauseKey}");

            _pauseText.Text.Set(message);
            _pauseText.TextColor.Reset();
        }

        private void onPause()
        {
            _canPause = false;
            _pauseText.enabled = false;
            _background.enabled = false;
        }

        private void onResume()
        {
            _canPause = true;
            _pauseText.enabled = true;
            _background.enabled = true;
        }
    }
}