using UnityEngine;

[CreateAssetMenu(fileName = "New Highlightable Color Set", menuName = "Colors/Highlightable Set")]
public class HighlightableColorSet : ScriptableObject
{
    [field: SerializeField] public Color Normal { get; private set; } = Color.white;
    [field: SerializeField] public Color Highlight { get; private set; } = Color.white;
}
