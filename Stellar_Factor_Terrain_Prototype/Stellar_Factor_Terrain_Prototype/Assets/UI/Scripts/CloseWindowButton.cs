using StellarFactor;
using UnityEngine;

public class CloseWindowButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.MGR.CancelArtifactInteraction();
    }
}
