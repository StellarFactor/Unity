using System;
using UnityEngine;

namespace StellarFactor.Minimap
{
    public class DeathLocation : IMapLocation
    {
        private Logger log = new();

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

            GameManager.MGR.PlayerDeath += HandlePlayerDeath;
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
            throw new NotImplementedException();
        }

        public void CreateNode()
        {
            throw new NotImplementedException();
        }

        public void DestroyNode()
        {
            throw new NotImplementedException();
        }

        ~DeathLocation()
        {
            log.Print($"{this} is being deconstructed");

            /// Unsubscribe
            GameManager.MGR.PlayerDeath -= HandlePlayerDeath;
            MiniMap.MGR.NodeUpdates -= HandleNodeUpdates;
        }
    }
}
