using StellarFactor;
using UnityEngine;

public interface IAcquirable
{
    Inventory StoredIn { get; }
    bool CanStack { get; }
    void OnAcquired(Inventory acquiredBy);
    void OnRemoved(Inventory removedFrom, Vector3 dropPos, Vector3 dropEuler);
}
