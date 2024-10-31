using StellarFactor;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerControl : MonoBehaviour
{
    private FirstPersonController _controller;

    private void OnEnable()
    {
        _controller = GetComponent<FirstPersonController>();

        GameManager.MGR.ArtifactInteraction += onArtifactFound;
        QuestionManager.MGR.CorrectAnswer += onAnswerGiven;
        QuestionManager.MGR.IncorrectAnswer += onAnswerGiven;
    }

    private void OnDisable()
    {
        GameManager.MGR.ArtifactInteraction -= onArtifactFound;
        QuestionManager.MGR.CorrectAnswer -= onAnswerGiven;
        QuestionManager.MGR.IncorrectAnswer -= onAnswerGiven;
    }

    private void onArtifactFound(Artifact artifact)
    {
        _controller.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void onAnswerGiven()
    {
        _controller.enabled = true;
        Cursor.visible = false;
        Cursor.lockState= CursorLockMode.Locked;
    }
}
