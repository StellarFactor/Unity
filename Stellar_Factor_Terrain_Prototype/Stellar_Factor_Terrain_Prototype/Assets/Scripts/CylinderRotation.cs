using UnityEngine;

public class CylinderRotator : MonoBehaviour
{
    [SerializeField] public float rotationSpeed = 50f; 

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
