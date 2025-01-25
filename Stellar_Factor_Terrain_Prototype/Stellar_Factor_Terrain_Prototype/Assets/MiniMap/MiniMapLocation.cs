using UnityEngine;

namespace StellarFactor.Minimap
{
    public class MiniMapLocation : MonoBehaviour, IMapLocation
    {
        [SerializeField] private bool triggerVisit;

        [SerializeField] private RectTransform unvisitedNode;
        [SerializeField] private RectTransform visitedNode;

        public bool BeenVisited {get; private set; }
        public bool IsActive { get; private set; }


        private void Update()
        {
            if (triggerVisit)
            {
                triggerVisit = false;
                Visit();
            }
        }

        public void Visit()
        {
            BeenVisited = true;
            IsActive = true;
            MiniMap.MGR.AddNodeFor(this);
        }

        public void Enter()
        {
            IsActive = true;
            MiniMap.MGR.AddNodeFor(this);
        }

        public void Leave()
        {
            IsActive = false;
            MiniMap.MGR.RemoveNodeFor(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            print ("colliding");
            if (other.CompareTag("Minimap"))
            {
                if (IsActive) { return; }

                Enter();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Minimap"))
            {
                if (!IsActive) { return; }

                Leave();
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public RectTransform GetNodeToDisplay()
        {
            return BeenVisited ? visitedNode : unvisitedNode;
        }
    }
}
