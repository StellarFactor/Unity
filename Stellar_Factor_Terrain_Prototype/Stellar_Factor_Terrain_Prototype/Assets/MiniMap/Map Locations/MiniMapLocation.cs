using UnityEngine;

namespace StellarFactor.Minimap
{
    public class MiniMapLocation : MonoBehaviour, IMapLocation, IVisitable
    {
        #region Serialized Vars
        // ======================================================
        [Header("Debug")]
        [SerializeField] private Logger log;
        [SerializeField] private bool triggerVisit;

        [Header("Node")]
        [SerializeField] private Node nodePrefab;
        [SerializeField] private VisitableNodeColorsSO colors;

        [Header("Visitable")]
        [SerializeField] private bool hasExternalTrigger;
        #endregion // Serialized Vars


        # region Private Vars
        // ======================================================
        private Node instantiatedNode;
        #endregion // Private Vars


        #region Properties
        // ======================================================
        public bool IsActive => instantiatedNode != null;
        #endregion // Properties


        #region Unity Methods
        // ======================================================
        private void OnEnable()
        {
            GameManager.MGR.PlayerDied += HandlePlayerDeath;
            MiniMap.MGR.NodeUpdates += HandleNodeUpdates;
        }

        private void OnDisable()
        {
            GameManager.MGR.PlayerDied -= HandlePlayerDeath;
            MiniMap.MGR.NodeUpdates -= HandleNodeUpdates;

            DestroyNode();
        }

        private void Update()
        {
            if (triggerVisit)
            {
                triggerVisit = false;
                Visit();
            }

            if (instantiatedNode != null)
            {
                MiniMap.MGR.UpdateNode(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Minimap"))
            {
                log.Print(
                    $"Collided with {other.gameObject.name}");

                Enter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Minimap"))
            {
                log.Print(
                    $"{other.gameObject.name} left my" +
                    $"({gameObject.name}) trigger");

                Leave();
            }
        }
        #endregion // Unity Methods


        #region IVisitable
        // ======================================================
        bool IVisitable.BeenVisited { get; set; }

        public void Visit()
        {
            (this as IVisitable).BeenVisited = true;
        }

        public void Enter()
        {
            if (!(this as IVisitable).BeenVisited
                && !hasExternalTrigger)
            {
                Visit();
            }

            CreateNode();
        }

        public void Leave()
        {
            DestroyNode();
        }
        #endregion // IVisitable


        #region IMapLocation
        // ======================================================
        Node IMapLocation.InstantiatedNode
        {
            get { return instantiatedNode; }
            set { instantiatedNode = value; }
        }

        public void CreateNode()
        {
            if (IsActive) { return; }

            MiniMap.MGR.InstantiateNodeAt(this);
        }

        public void DestroyNode()
        {
            if (!IsActive) { return; }

            Destroy(instantiatedNode.gameObject);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Node GetNodePrefab()
        {
            return nodePrefab;
        }

        public Color GetNodeColor()
        {
            return (this as IVisitable).BeenVisited
                    ? colors.Visited
                    : colors.Unvisited;
        }

        public void HandleNodeUpdates()
        {
            if (instantiatedNode == null) { return; }
            CreateNode();
        }

        public void HandlePlayerDeath()
        {
            DestroyNode();
        }
        #endregion // IMapLocation
    }
}
