using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB; 
    [SerializeField] private float speed = 2f; 

    private Transform target;

    private void Start()
    {
        //Platforms start moving towards PointA by default when the game starts
        target = pointA; 
    }

    private void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        //Whenever the platform reaches one of the checkpoints, it'll start moving towards the other one
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }
}
