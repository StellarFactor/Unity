using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaRespawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If player falls into lava, itll reload the scene.
        if (other.CompareTag("Lava"))
        {
            SceneManager.LoadScene("TerrainPrototype");

            Debug.Log("Player fell in lava.");
        }

    }

}
