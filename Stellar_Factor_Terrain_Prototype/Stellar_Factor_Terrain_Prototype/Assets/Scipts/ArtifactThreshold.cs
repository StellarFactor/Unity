using StellarFactor;
using UnityEngine;
using UnityEngine.Assertions;

public class ArtifactThreshold : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject physicalCollider;
    [SerializeField] private TriggerBox triggerBox;

    [SerializeField] private int requiredArtifactCount = 8;
    [SerializeField] private string notMetMessage;
    [SerializeField] private string metMessage;

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
        Assert.IsNotNull(triggerBox,
            $"{name}'s Trigger box is null. Cannot check for artifacts.");
        Assert.IsTrue(notMetMessage != "",
            $"{name}'s Not Met Message is empty. Cannot display threshold message.");
    }

    private void Update()
    {
        physicalCollider.SetActive(ThresholdIsActive);
        triggerBox.gameObject.SetActive(ThresholdIsActive);
    }

    public void Interact()
    {
    }

    public void PlayerEnterRange(PlayerControl playerControl)
    {
        Assert.IsNotNull(playerControl, "PlayerControl is null. Cannot check for artifacts.");

        playerInventory = playerControl.GetComponent<Inventory>();

        if (ThresholdIsActive && IsConditionMet())
        {
            if (metMessage == "")
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
        MessageIsActive = GameManager.MGR.RequestSimplePrompt(notMetMessage);
    }

    public void ClearMessage()
    {
        MessageIsActive = !GameManager.MGR.RequestClosePrompt();
    }

    protected virtual bool IsConditionMet()
    {
        return playerInventory.ArtifactsAcquired.Count >= requiredArtifactCount;
    }
}
