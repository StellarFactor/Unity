#if UNITY_EDITOR
using UnityEditor;
#endif

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
            _quitText.TextColor.Reset();
        }

        private void onQuit()
        {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;

        #else
            Application.Quit();

        #endif
        }
    }
}