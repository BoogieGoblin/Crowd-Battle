using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUp : MonoBehaviour
{
    int currentSceneIndex;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int nextSceneIndex = currentSceneIndex + 1;

            // 2. Wrap around if you reach the last scene
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0; // Or load a Win/Main Menu scene
            }

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}