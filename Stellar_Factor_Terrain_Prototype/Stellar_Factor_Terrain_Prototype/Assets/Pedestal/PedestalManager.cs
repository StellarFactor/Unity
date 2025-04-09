using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace StellarFactor
{
    public class PedestalManager : Singleton<PedestalManager>
    {
        [SerializeField] private List<Pedestal> allPedestals = new();
        private List<Pedestal> emptyPedestals => allPedestals.Where(ped => !ped.ArtifactPlaced).ToList();
        private List<Pedestal> fullPedestals => allPedestals.Where(ped => ped.ArtifactPlaced).ToList();

        public bool ReadyForBoss => emptyPedestals.Count == 0;

        public UnityEvent OnAllPedestalsCompletedInspector;
        public event Action AllPedestalsCompleted;

        private void Update()
        {
            if (fullPedestals.Count == allPedestals.Count)
            {
                OnAllPedestalsComplete();
            }
        }

        public void CompletePedestal(Pedestal pedestal)
        {
            if (emptyPedestals.Remove(pedestal))
            {
                fullPedestals.Add(pedestal);
            }
        }

        protected virtual void OnAllPedestalsComplete()
        {
            Debug.Log("<color=green>All Pedestals completed</color>");
            OnAllPedestalsCompletedInspector.Invoke();
            AllPedestalsCompleted.Invoke();
        }
    }
}
