using StellarFactor;
using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerControl : MonoBehaviour
{
    private FirstPersonController _controller;

    private void OnEnable()
    {
        _controller = GetComponent<FirstPersonController>();

        GameManager.MGR.ArtifactInteraction += onArtifactInteraction;
        GameManager.MGR.CancelArtifactInteraction += onCancelArtifactInteraction;
        QuestionManager.MGR.CorrectAnswer += onCorrectAnswer;
        QuestionManager.MGR.IncorrectAnswer += onIncorrectAnswer;
    }


    private void OnDisable()
    {
        GameManager.MGR.ArtifactInteraction -= onArtifactInteraction;
        GameManager.MGR.CancelArtifactInteraction -= onCancelArtifactInteraction;
        QuestionManager.MGR.CorrectAnswer -= onCorrectAnswer;
        QuestionManager.MGR.IncorrectAnswer -= onIncorrectAnswer;
    }

    private void onArtifactInteraction(Artifact artifact)
    {
        lockControls();
    }

    private void onCancelArtifactInteraction()
    {
        unlockControls();
    }

    private void onCorrectAnswer()
    {
        unlockControls();

        // TODO:
        // Add "artifact" to "inventory" (probably
        // just flip a bool? lol)
    }

    private void onIncorrectAnswer()
    {
        unlockControls();
        
        // TODO:
        // Lose health? Anything else?
    }

    private void lockControls()
    {
        _controller.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void unlockControls()
    {
        _controller.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
