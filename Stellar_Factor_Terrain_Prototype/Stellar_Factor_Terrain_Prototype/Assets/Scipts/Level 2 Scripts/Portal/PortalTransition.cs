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
    public Canvas videoCanvas;

    //I added a small delay after the video transition ends just so it doesn't feel awkward when transitioning
    public float delayAfterVideo = 0.5f;

    // Prevent multiple triggers
    private bool hasTriggered = false;

    private void Start()
    {
        DisableCanvas();
        videoPlayer.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Portal triggered by player!");

            EnableCanvas();
            PlayVideo();
        }
    }

    //This'll enable the Video Canvas in order to showcase the transition Video
    private void EnableCanvas()
    {
        if (videoCanvas != null)
        {
            Debug.Log("Enabling video canvas...");
            videoCanvas.gameObject.SetActive(true);
            videoCanvas.enabled = true;
            Debug.Log("Canvas active: " + videoCanvas.isActiveAndEnabled);
        }
        else
        {
            Debug.LogWarning("Video canvas is not assigned in the PortalTransition script!");
        }
    }

    private void DisableCanvas()
    {
        if (videoCanvas != null)
        {
            Debug.Log("Disabling video canvas...");
            videoCanvas.gameObject.SetActive(false);
            videoCanvas.enabled = false;
        }
    }

    //This enables the video player to start playing
    private void PlayVideo()
    {
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

        // Wait a frame after loading the next scene,
        // then disable the video canvas
        // This way we don't see the player on the
        // other side of the portal in level 1
        yield return null;
        DisableCanvas();
    }
}
