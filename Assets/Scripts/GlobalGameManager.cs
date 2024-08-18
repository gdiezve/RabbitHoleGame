using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    // Global values
    public int fieldSize = 50;              // Size of the square field (e.g., 10x10)
    public int obstacleCount = 20;          // Number of obstacles to spawn
    public int numberOfCollectablesPerType = 5; // Number of collectables of each type to spawn
    public float enemySpawnRadius = 15f; // Radius around the player where enemies will spawn
    public float enemySpawnInterval = 3f; // Time interval between spawns
    public float enemyMinSpawnRadius = 10f; // Minimum distance from the player to spawn enemies
    public float orthographicSize = 7f; // Size of the orthographic camera view
    public float enemySpeed = 3f; // Speed at which the enemy moves towards the player
    public float playerSpeed = 5f; // Speed of the character movement
    public int randomPlayerScale;
    public int randomFinishScale;

    void Awake()
    {
        randomPlayerScale = Random.Range(1, 4);
        randomFinishScale = Random.Range(1, 4);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
}
