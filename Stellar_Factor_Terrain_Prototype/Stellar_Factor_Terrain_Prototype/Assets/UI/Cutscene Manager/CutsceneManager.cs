using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

namespace StellarFactor
{
    public class CutsceneManager : Singleton<CutsceneManager>
    {
        //References the videoPlayer
        public VideoPlayer videoPlayer;

        //References the videoCanvas
        public Canvas videoCanvas;

        private bool isBusy = false;
        // change to param
        private float delayAfterVideo;
        private string sceneToTransitionTo;


        private void Start()
        {
            DisableCanvas();
            videoPlayer.gameObject.SetActive(false);
        }

        public void RequestCutscene(VideoClip clipToPlay)
        {
            if (isBusy) { return; }

            videoPlayer.clip = clipToPlay;
            EnableCanvas();
            PlayVideo();
            isBusy = true;
        }

        public void RequestCutscene(VideoClip clipToPlay, string sceneToTransitionTo, float delayAfterVideo)
        {
            if (isBusy) { return; }

            this.sceneToTransitionTo = sceneToTransitionTo;
            this.delayAfterVideo = delayAfterVideo;

            videoPlayer.clip = clipToPlay;

            EnableCanvas();
            PlayVideo();
            isBusy = true;
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
            }
        }

        private void OnVideoFinished(VideoPlayer vp)
        {
            Debug.Log("Video finished playing.");


            vp.loopPointReached -= OnVideoFinished;
            StartCoroutine(LoadSceneAfterDelay(sceneToTransitionTo, delayAfterVideo));

            sceneToTransitionTo = "";
            delayAfterVideo = 0f;
            isBusy = false;
        }

        private IEnumerator LoadSceneAfterDelay(string sceneName, float delayAfterVideo)
        {
            yield return new WaitForSeconds(delayAfterVideo);
            SceneManager.LoadScene(sceneName);

            // Wait a frame after loading the next scene,
            // then disable the video canvas
            // This way we don't see the player on the
            // other side of the portal in level 1
            yield return null;
            DisableCanvas();
        }
    }
}