using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    int currentSceneIndex;
    void Awake()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
