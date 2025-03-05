using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PortalTransition : MonoBehaviour
{
    [Tooltip("Name of the scene to load (must be added in Build Settings)")]
    public string nextSceneName = "Level 2 Inside";

    [Tooltip("Reference to the VideoPlayer component that will play the video")]
    public VideoPlayer videoPlayer;

    [Tooltip("Reference to the Canvas that contains the VideoPlayer (make sure it is disabled initially)")]
    public GameObject videoCanvas;

    [Tooltip("Optional delay (in seconds) after the video finishes before switching scenes")]
    public float delayAfterVideo = 0.5f;

    // Prevent multiple triggers
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Portal triggered by player!");

            // Enable the canvas that holds the video
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

            // Enable and start the VideoPlayer
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
