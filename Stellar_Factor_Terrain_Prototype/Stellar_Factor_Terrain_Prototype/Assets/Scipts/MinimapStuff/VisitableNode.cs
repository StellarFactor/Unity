using UnityEngine;

namespace StellarFactor.Minimap
{
    public class VisitableNode : Node
    {
        [SerializeField] bool beenVisited;

        public bool BeenVisited { get { return beenVisited; } }
    }
}