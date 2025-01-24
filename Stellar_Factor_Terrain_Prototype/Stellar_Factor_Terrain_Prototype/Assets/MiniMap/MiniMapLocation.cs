using UnityEngine;

namespace Overtown
{
    public class MiniMapLocation : MonoBehaviour
    {
        [SerializeField] private bool triggerVisit;

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
            MiniMap.MGR.DisplayNodeFor(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            print ("colliding");
            if (other.CompareTag("Minimap"))
            {
                if (IsActive) {  return; }
                MiniMap.MGR.DisplayNodeFor(this);
                IsActive = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Minimap"))
            {
                if (!IsActive) { return; }
                MiniMap.MGR.HideNodeFor(this);
                IsActive = false;
            }
        }
    }
}
