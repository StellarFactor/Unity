using UnityEngine;

[CreateAssetMenu(menuName = "MiniMap/Node Colors/Static Node Color")]
public class StaticNodeColorsSO : NodeColorsSO
{
    public Color StaticColor { get { return defaultColor; } }
}
