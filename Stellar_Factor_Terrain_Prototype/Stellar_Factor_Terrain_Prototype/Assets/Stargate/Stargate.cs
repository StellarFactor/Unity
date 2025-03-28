using System.Collections.Generic;
using UnityEngine;

namespace StellarFactor
{
    public class Stargate : MonoBehaviour, IToggleable
    {
        [SerializeField] private List<GameObject> portalGameObjects;

        private void Start()
        {
            TurnOff();
        }

        public bool IsOn { get; private set; }
        public void TurnOn()
        {
            IsOn = true;

            portalGameObjects.ForEach(obj => obj.gameObject.SetActive(true));
            // whatever you know needs to get set active / enabled, etc
        }
        public void TurnOff()
        {
            IsOn = false;

            // opposite of above
            portalGameObjects.ForEach(obj => obj.gameObject.SetActive(false));
        }
        public void Toggle()
        {
            if (IsOn)
                TurnOff();
            else
                TurnOn();
        }
    }
}