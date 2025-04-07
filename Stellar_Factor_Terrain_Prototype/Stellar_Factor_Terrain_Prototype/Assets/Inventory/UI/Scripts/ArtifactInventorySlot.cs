using StellarFactor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArtifactInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private IconMask iconMask;

    private HighlightableColorSet empty;
    private HighlightableColorSet full;
    private HighlightableColorSet fullSelected;

    private bool isMouseHere;
    private bool isMouseDown;

    [field: SerializeField] public Image Background { get; private set; }
    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public Image Mask { get; private set; }

    [field: SerializeField] public string MatchingArtifactName { get; private set; }

    public Artifact Artifact { get; private set; }

    private void Start()
    {
        InitColors();
        Icon.sprite = iconMask.iconSprite;

        Mask.sprite = iconMask.iconSprite;
        Mask.material = iconMask.maskMat;
        Mask.color = Color.white;
    }

    private void Update()
    {
        Icon.color = Artifact == null
            ? isMouseHere                   // Empty
                ? isMouseDown
                    ? empty.Highlight
                    : empty.Highlight
                : empty.Normal
            : isMouseHere                   // Full
                ? isMouseDown
                    ? fullSelected.Highlight
                    : full.Highlight
                : full.Normal;
    }

    public void InitColors()
    {
        empty = ArtifactInventoryUI.MGR.EmptyColorSet;
        full = ArtifactInventoryUI.MGR.FullColorSet;
        fullSelected = ArtifactInventoryUI.MGR.FullColorSelectedSet;
    }

    public void FillWith(Artifact artifact)
    {
        Artifact = artifact;
    }

    public Artifact Empty()
    {
        Artifact toEmpty = Artifact;
        Artifact = null;
        return toEmpty;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseHere = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseHere = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isMouseHere) { return; }

        isMouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMouseDown = false;
    }
}
