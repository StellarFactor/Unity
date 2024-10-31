using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaRespawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // If player falls into lava, itll reload the scene.
        if (other.CompareTag("Lava"))
        {
            //SceneManager.LoadScene("TerrainPrototype");

            // (layla) changed this because we're using a new scene now.
            // This way will load whatever scene we're in.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Debug.Log("Player fell in lava.");
        }

        if (other.CompareTag("Stargate"))
        {
            //SceneManager.LoadScene("TerrainPrototype");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Debug.Log("Player Entered the Stargate!");
        }

    }

}
