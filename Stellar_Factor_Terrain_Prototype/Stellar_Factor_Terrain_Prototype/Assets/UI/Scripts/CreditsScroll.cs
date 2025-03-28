using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 50f;

    private void Update()
    {
       
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }
}
