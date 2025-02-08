using UnityEngine;

namespace StellarFactor.Minimap
{
    public interface IMapLocation
    {
        Node InstantiatedNode { get; set; }
        Vector3 GetPosition();
        Node GetNodePrefab();
        Color GetNodeColor();
        void HandleNodeUpdates();
        void HandlePlayerDeath();
        void CreateNode();
        void DestroyNode();
    }
}
