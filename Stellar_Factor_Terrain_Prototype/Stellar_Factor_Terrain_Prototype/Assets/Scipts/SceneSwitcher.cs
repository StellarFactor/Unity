using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToGame()
    {
        SceneManager.LoadScene("Interaction Test");
    }
    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void SwitchToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
