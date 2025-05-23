using StellarFactor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ArtifactThreshold : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject physicalCollider;
    [SerializeField] private BoxCollider triggerCollider;

    [SerializeField] private int requiredArtifactCount = 8;
    [SerializeField] private bool requireGreater;
    [SerializeField, TextArea] private string notEnoughMessage = "You haven't collected all the Artifacts yet!";
    [SerializeField, TextArea] private string tooManyMessage = "You haven't placed all the Artifacts back on their pedestals yet!";
    [SerializeField, TextArea] private string metMessage;

    protected List<Renderer> rends = new();
    protected Inventory playerInventory;

    #region Properties
    //==========================================================================
    public bool MessageIsActive { get; private set; } = false;
    public bool ThresholdIsActive { get; private set; } = true;
    #endregion // Properties


    private void Awake()
    {
        Assert.IsNotNull(physicalCollider,
            $"{name}'s Physical collider is null. Cannot check for artifacts.");

        rends = transform.parent.GetComponentsInChildren<Renderer>().ToList();
        rends.ForEach(rend => rend.enabled = false);
    }

    private void Update()
    {
        physicalCollider.SetActive(ThresholdIsActive);
        triggerCollider.enabled = ThresholdIsActive;
    }


    public void PlayerEnterRange(PlayerControl playerControl)
    {
        Assert.IsNotNull(playerControl, "PlayerControl is null. Cannot check for artifacts.");

        playerInventory = playerControl.GetComponent<Inventory>();

        if (ThresholdIsActive && IsConditionMet())
        {
            if (metMessage != "")
            {
                ShowMetMessage();
            }
            ThresholdIsActive = false;
        }
        else if (!IsConditionMet())
        {
            ShowNotMetMessage();
        }
    }

    public void Interact()
    {
    }

    public void PlayerExitRange(PlayerControl playerControl)
    {
        if (MessageIsActive)
        {
            ClearMessage();
        }
    }

    public void ShowMetMessage()
    {
        MessageIsActive = GameManager.MGR.RequestSimplePrompt(metMessage);
    }

    public void ShowNotMetMessage()
    {
        string msg = requireGreater ? notEnoughMessage : tooManyMessage;
        MessageIsActive = GameManager.MGR.RequestSimplePrompt(msg);
    }

    public void ClearMessage()
    {
        MessageIsActive = !GameManager.MGR.RequestClosePrompt();
    }

    protected virtual bool IsConditionMet()
    {
        int artifactCount = playerInventory.GetCurrentItemsOfType(typeof(Artifact)).Count;
        return requireGreater
            ? artifactCount >= requiredArtifactCount
            : artifactCount <= requiredArtifactCount;
    }
}

