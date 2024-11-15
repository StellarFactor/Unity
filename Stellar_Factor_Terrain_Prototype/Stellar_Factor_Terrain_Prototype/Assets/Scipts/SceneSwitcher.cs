using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToGame(string sceneName)
    {
        SceneManager.LoadScene("Interaction Test");
    }
    public void SwitchToMainMenu(string sceneName)
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void SwitchToSettings(string sceneName)
    {
        SceneManager.LoadScene("Settings");
    }

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
