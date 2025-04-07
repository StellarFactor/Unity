using UnityEngine;

namespace StellarFactor
{
    public interface IToggleable
    {
        bool IsOn { get; }
        void TurnOn();
        void TurnOff();
        void Toggle();
    }
}