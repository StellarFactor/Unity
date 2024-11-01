using UnityEditor;
using UnityEngine;

namespace StellarFactor
{
    public class QuitPrompt : MonoBehaviour
    {
        [SerializeField] Textbox _quitText;

        private void OnEnable()
        {
            GameManager.MGR.LevelLoaded += onLevelLoaded;
            GameManager.MGR.Quit += onQuit;
        }

        private void OnDisable()
        {
            GameManager.MGR.LevelLoaded -= onLevelLoaded;
            GameManager.MGR.Quit -= onQuit;
        }

        private void Update()
        {
            if (Input.GetKeyDown(GameManager.MGR.QuitKey))
            {
                GameManager.MGR.Quit.Invoke();
            }
        }

        private void onLevelLoaded(int obj)
        {
            string message = _quitText.Text.Get();

            message = message.Replace("<>", $"{GameManager.MGR.QuitKey}");

            _quitText.Text.Set(message);
        }

        private void onQuit()
        {
            if (Application.isEditor)
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }
        }
    }
}