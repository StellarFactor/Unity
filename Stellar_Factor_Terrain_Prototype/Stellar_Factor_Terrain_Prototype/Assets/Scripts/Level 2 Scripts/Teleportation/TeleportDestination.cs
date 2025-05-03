using UnityEngine;

public class TeleportDestination : MonoBehaviour
{
    public Transform teleportPoint; 

    public Transform GetDestination()
    {
        return teleportPoint;
    }
}
