using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PortalTransition : MonoBehaviour
{
    //This is what references the name of the scene that we are transitioning to
    public string nextSceneName = "Level 2 Inside";

    //References the videoPlayer
    public VideoPlayer videoPlayer;

    //References the videoCanvas
    public GameObject videoCanvas;

    //I added a small delay after the video transition ends just so it doesn't feel awkward when transitioning
    public float delayAfterVideo = 0.5f;

    // Prevent multiple triggers
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Portal triggered by player!");

            //This'll enable the Video Canvas in order to showcase the transition Video
            if (videoCanvas != null)
            {
                Debug.Log("Enabling video canvas...");
                videoCanvas.SetActive(true);
                Debug.Log("Canvas active: " + videoCanvas.activeInHierarchy);
            }
            else
            {
                Debug.LogWarning("Video canvas is not assigned in the PortalTransition script!");
            }

            //This enables the video player to start playing
            if (videoPlayer != null)
            {
                videoPlayer.gameObject.SetActive(true);
                Debug.Log("Starting video playback...");
                videoPlayer.loopPointReached += OnVideoFinished;
                videoPlayer.Play();
            }
            else
            {
                Debug.LogWarning("VideoPlayer is not assigned in the PortalTransition script!");
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished playing.");
        vp.loopPointReached -= OnVideoFinished;
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayAfterVideo);
        SceneManager.LoadScene(nextSceneName);
    }
}
