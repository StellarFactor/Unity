using StellarFactor;
using UnityEngine;

public interface IAcquirable
{
    void AquireBy(Inventory inventory);
    void RemoveFrom(Inventory inventory);
}
