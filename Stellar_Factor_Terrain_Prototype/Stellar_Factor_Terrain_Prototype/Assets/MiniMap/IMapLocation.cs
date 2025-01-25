using UnityEngine;

namespace StellarFactor.Minimap
{
    public interface IMapLocation
    {
        Vector3 GetPosition();
        RectTransform GetNodeToDisplay();
    }

    public class DeathLocation : IMapLocation
    {
        RectTransform node;
        Vector3 position;

        public DeathLocation(RectTransform node, Vector3 position)
        {
            this.node = node;
            this.position = position;
        }

        public RectTransform GetNodeToDisplay()
        {
            return node;
        }

        public Vector3 GetPosition()
        {
            return position;
        }
    }
}
