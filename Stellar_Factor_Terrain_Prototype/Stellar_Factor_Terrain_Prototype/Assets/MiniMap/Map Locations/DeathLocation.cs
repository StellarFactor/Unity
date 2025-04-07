using System;
using UnityEngine;

namespace StellarFactor.Minimap
{
    public class DeathLocation : IMapLocation
    {
        private DbugLog log = new();

        private readonly Node nodePrefab;
        private readonly Vector3 position;
        private readonly Color color;

        private Node instantiatedNode;

        Node IMapLocation.InstantiatedNode {
            get { return instantiatedNode; }
            set { instantiatedNode = value; }
        }

        public DeathLocation(Node node, Vector3 position, Color color)
        {
            this.nodePrefab = node;
            this.position = position;
            this.color = color;

            GameManager.MGR.PlayerDied += HandlePlayerDeath;
            MiniMap.MGR.NodeUpdates += HandleNodeUpdates;
        }

        public Node GetNodePrefab()
        {
            return nodePrefab;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public Color GetNodeColor()
        {
            return color;
        }

        public void HandleNodeUpdates()
        {
            if (instantiatedNode == null) { return; }
            MiniMap.MGR.UpdateNode(this);
        }

        public void HandlePlayerDeath()
        {
            // TODO: Do these really belong on IMapLocation?
        }

        public void CreateNode()
        {
            // TODO: Do these really belong on IMapLocation?
        }

        public void DestroyNode()
        {
            // TODO: Do these really belong on IMapLocation?
        }

        ~DeathLocation()
        {
            log.Print($"{this} is being deconstructed");

            /// Unsubscribe
            GameManager.MGR.PlayerDied -= HandlePlayerDeath;
            MiniMap.MGR.NodeUpdates -= HandleNodeUpdates;
        }
    }
}
