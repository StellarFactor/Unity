using StellarFactor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelCycler : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new();
    [SerializeField] private bool wrapStart = false;
    [SerializeField] private bool wrapEnd = false;
    [SerializeField] private bool openOnStart = false;

    private int currentIndex = 0;
    private GameObject currentPanel;

    public bool IsRunning { get; private set; }

    private List<GameObject> Panels
    {
        get { return panels ?? new(); }
    }

    private bool NeedsUpdate
    {
        get
        {
            if (!Panels.Any()) { return false; }

            return currentPanel != Panels[currentIndex];
        }
    }

    private bool IsGreater  => currentIndex >= Panels.Count;
    private bool IsLess     => currentIndex < 0;

    private void OnEnable()
    {
        GameManager.MGR.GamePaused += HandlePause;
        GameManager.MGR.GameResumed += HandleResume;
    }

    private void OnDisable()
    {
        GameManager.MGR.GamePaused -= HandlePause;
        GameManager.MGR.GameResumed -= HandleResume;
    }

    private void Start()
    {
        if (openOnStart)
        {
            BeginCycle();
        }
    }

    private void Update()
    {
        if (!IsRunning) { return; }

        if (NeedsUpdate)
        {
            UpdateCurrentPanel();
            ActivateCurrentPanel();
        }
    }

    public void NextPanel()
    {
        currentIndex++;

        if (IsGreater)
        {
            if (wrapEnd)
            {
                WrapIndex();
            }
            else
            {
                EndCycle();
            }
        }
    }

    public void PrevPanel()
    {
        currentIndex--;

        if (IsLess)
        {
            if (wrapStart)
            {
                WrapIndex();
            }
            else
            {
                currentIndex = 0;
            }
        }
    }

    public void BeginCycle()
    {
        IsRunning = true;
        currentIndex = 0;
        GameManager.MGR.StartPanelCyclerInteraction(this);
    }

    public void EndCycle()
    {
        currentIndex = 0;
        currentPanel = null;
        Panels.ForEach(panel => panel.SetActive(false));
        IsRunning = false;
    }

    private void UpdateCurrentPanel()
    {
        if (!Panels.Any()) { return; }

        currentPanel = Panels[currentIndex];
    }

    private void ActivateCurrentPanel()
    {
        foreach (GameObject panel in Panels)
        {
            panel.SetActive(panel == currentPanel);
        }
    }

    private void WrapIndex()
    {
        if (IsLess)
        {
            currentIndex = Panels.Count - 1;
        }
        else if (IsGreater)
        {
            currentIndex = 0;
        }
    }

    private void HandlePause()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons.ToList().ForEach(button => button.interactable = false);
    }

    private void HandleResume()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons.ToList().ForEach(button => button.interactable = true);
    }
}
