using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;  // First position
    [SerializeField] private Transform pointB;  // Second position
    [SerializeField] private float speed = 2f;  // Movement speed

    private Transform target;  // Current target

    private void Start()
    {
        target = pointA; // Start moving towards Point A
    }

    private void Update()
    {
        // Move platform towards the target position
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // If the platform reaches the target, switch to the other one
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }
}
