using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Load(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
