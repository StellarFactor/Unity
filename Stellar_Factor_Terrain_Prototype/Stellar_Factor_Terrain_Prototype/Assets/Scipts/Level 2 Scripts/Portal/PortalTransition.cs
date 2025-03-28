using StellarFactor;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PortalTransition : MonoBehaviour
{
    public VideoClip cutsceneClip;

    //This is what references the name of the scene that we are transitioning to
    public string nextSceneName = "Level 2 Inside";

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

            CutsceneManager.MGR.RequestCutscene(cutsceneClip, nextSceneName, delayAfterVideo);
        }
    }
}
