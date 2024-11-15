using System.Collections;
using System.Collections.Generic;
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
}
