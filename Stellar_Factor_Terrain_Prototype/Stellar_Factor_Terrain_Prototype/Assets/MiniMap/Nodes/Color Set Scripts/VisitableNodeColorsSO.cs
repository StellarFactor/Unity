using UnityEngine;

[CreateAssetMenu(menuName = "MiniMap/Node Colors/Visitable Location")]
public class VisitableNodeColorsSO : NodeColorsSO
{
    [SerializeField] private Color unvisited = Color.white;
    [SerializeField] private Color visited = Color.white;

    public Color Unvisited { get { return unvisited; } }
    public Color Visited { get { return visited; } }
}
