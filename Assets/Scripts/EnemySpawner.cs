using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform player; // Reference to the player's transform
    private float spawnTimer;

    void Start()
    {
        spawnTimer = GlobalGameManager.Instance.enemySpawnInterval; // Initialize the timer
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = GlobalGameManager.Instance.enemySpawnInterval; // Reset the timer
        }
    }

    void SpawnEnemy()
    {
        // Generate a random position within the field size around the player
        Vector3 spawnPosition = GetRandomSpawnPosition();
        spawnPosition.z = 0f; // Keep the enemy on the same plane as the player

        // Spawn the enemy and set its target to the player
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.player = player;
    }

    Vector2 GetRandomSpawnPosition()
    {
        Vector2 playerPosition = player.position;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float spawnDistance = Random.Range(GlobalGameManager.Instance.enemyMinSpawnRadius, GlobalGameManager.Instance.enemySpawnRadius);

        Vector2 potentialSpawnPosition = playerPosition + randomDirection * spawnDistance;

        // Ensure the enemy spawns within the field size limits
        int halfFieldSize = GlobalGameManager.Instance.fieldSize / 2;
        float xMin = -halfFieldSize + 3;
        float xMax = halfFieldSize - 3;
        float yMin = -halfFieldSize + 3;
        float yMax = halfFieldSize - 3;

        // Clamp the spawn position to be within the field limits
        float clampedX = Mathf.Clamp(potentialSpawnPosition.x, xMin, xMax);
        float clampedY = Mathf.Clamp(potentialSpawnPosition.y, yMin, yMax);

        return new Vector2(clampedX, clampedY);
    }
}
