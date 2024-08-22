using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LevelGenerator : MonoBehaviour
{
    public GameObject obstaclePrefab;       // Prefab for the obstacles
    public GameObject finishPrefab;         // Prefab for the finish point
    public GameObject wallPrefab;           // Prefab for the wall
    public GameObject playerPrefab;         // Prefab for the player
    public float minDistanceFromFinish = 3f; // Minimum distance from finish point to avoid obstacles
    public int level = 1;                   // Current level number
    public GameObject winnerMenuUI;
    public GameObject keepTryingMenuUI;
    public GameObject loserMenuUI;
    public GameObject playerHealthUI;
    public TextMeshProUGUI playerScoreUIText;
    public TextMeshProUGUI winnerScoreUIText;
    public TextMeshProUGUI loserScoreUIText;

    private Vector2 finishPoint;
    private Vector2 playerSpawnPoint;
    private readonly HashSet<Vector2> occupiedPositions = new();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        int halfFieldSize = GlobalGameManager.Instance.fieldSize / 2;

        // Generate walls around the field and mark their positions as occupied
        GenerateWalls(halfFieldSize);

        // Generate random finish point
        finishPoint = GenerateRandomPosition(halfFieldSize);
        occupiedPositions.Add(finishPoint);

        // Instantiate the finish point with random scale
        GameObject finishObject = Instantiate(finishPrefab, new Vector3(finishPoint.x, finishPoint.y, 0), Quaternion.identity);
        finishObject.transform.localScale = new Vector3(GlobalGameManager.Instance.randomFinishScale, GlobalGameManager.Instance.randomFinishScale, 1f);

        // Generate and instantiate obstacles
        for (int i = 0; i < GlobalGameManager.Instance.obstacleCount; i++)
        {
            Vector2 obstaclePosition = GenerateRandomPosition(halfFieldSize);

            // Ensure the obstacle is not too close to the finish point or on top of a wall
            if (Vector2.Distance(obstaclePosition, finishPoint) < minDistanceFromFinish || occupiedPositions.Contains(obstaclePosition))
            {
                i--; // Retry this iteration
                continue;
            }

            // Mark the obstacle's position as occupied and instantiate it
            occupiedPositions.Add(obstaclePosition);
            Instantiate(obstaclePrefab, new Vector3(obstaclePosition.x, obstaclePosition.y, 0), Quaternion.identity);
        }

        // Generate player spawn point
        GeneratePlayerSpawnPoint(halfFieldSize);

        // Instantiate the player at the spawn point with random scale
        GameObject player = Instantiate(playerPrefab, new Vector3(playerSpawnPoint.x, playerSpawnPoint.y, 0), Quaternion.identity);
        do {
           GlobalGameManager.Instance.randomPlayerScale = Random.Range(1, 3);
        } while (GlobalGameManager.Instance.randomPlayerScale == GlobalGameManager.Instance.randomFinishScale);
        player.transform.localScale = new Vector3(GlobalGameManager.Instance.randomPlayerScale, GlobalGameManager.Instance.randomPlayerScale, 1f);
        player.GetComponent<PlayerController>().winnerMenuUI = winnerMenuUI;
        player.GetComponent<PlayerController>().keepTryingMenuUI = keepTryingMenuUI;
        player.GetComponent<PlayerController>().loserMenuUI = loserMenuUI;
        player.GetComponent<PlayerController>().playerHealthUI = playerHealthUI;
        player.GetComponent<ScoreManager>().playerScoreUIText = playerScoreUIText;
        player.GetComponent<ScoreManager>().winnerScoreUIText = winnerScoreUIText;
        player.GetComponent<ScoreManager>().loserScoreUIText = loserScoreUIText; 

        // Set the player Transform for the enemy spawner and the camera
        EnemySpawner enemySpawnerScript = FindObjectOfType<EnemySpawner>();
        enemySpawnerScript.player = player.transform;
        TopDownCamera topDownCameraScript = FindObjectOfType<TopDownCamera>();
        topDownCameraScript.player = player.transform;
        MinimapCamera minimapCamera = FindObjectOfType<MinimapCamera>();
        minimapCamera.player = player;
    }

    void GenerateWalls(int halfFieldSize)
    {
        for (int x = -halfFieldSize; x <= halfFieldSize; x++)
        {
            // Top wall (y = halfFieldSize)
            Vector2 topWallPosition = new(x, halfFieldSize);
            occupiedPositions.Add(topWallPosition);
            Instantiate(wallPrefab, new Vector3(topWallPosition.x, topWallPosition.y, 0), Quaternion.identity);

            // Bottom wall (y = -halfFieldSize)
            Vector2 bottomWallPosition = new Vector2(x, -halfFieldSize);
            occupiedPositions.Add(bottomWallPosition);
            Instantiate(wallPrefab, new Vector3(bottomWallPosition.x, bottomWallPosition.y, 0), Quaternion.identity);
        }

        for (int y = -halfFieldSize; y <= halfFieldSize; y++)
        {
            // Right wall (x = halfFieldSize)
            Vector2 rightWallPosition = new Vector2(halfFieldSize, y);
            occupiedPositions.Add(rightWallPosition);
            Instantiate(wallPrefab, new Vector3(rightWallPosition.x, rightWallPosition.y, 0), Quaternion.identity);

            // Left wall (x = -halfFieldSize)
            Vector2 leftWallPosition = new Vector2(-halfFieldSize, y);
            occupiedPositions.Add(leftWallPosition);
            Instantiate(wallPrefab, new Vector3(leftWallPosition.x, leftWallPosition.y, 0), Quaternion.identity);
        }
    }

    void GeneratePlayerSpawnPoint(int halfFieldSize)
    {
        // Try finding a valid spawn point for the player
        do
        {
            playerSpawnPoint = GenerateRandomPosition(halfFieldSize);
        } while (occupiedPositions.Contains(playerSpawnPoint) || Vector2.Distance(playerSpawnPoint, finishPoint) < 12f);

        // Mark the player's position as occupied
        occupiedPositions.Add(playerSpawnPoint);
    }

    Vector2 GenerateRandomPosition(int halfFieldSize)
    {
        int x = Random.Range(-halfFieldSize + 3, halfFieldSize - 3);
        int y = Random.Range(-halfFieldSize + 3, halfFieldSize - 3);
        return new Vector2(x, y);
    }

    // Call this function to load the next level with different parameters
    public void LoadLevel(int obstacleCountIncrease = 0, int fieldSizeIncrease = 0, int numberOfCollectablesPerTypeIncrease = 0)
    {
        level++;

        // Adjust level parameters as needed for each level
        GlobalGameManager.Instance.obstacleCount += obstacleCountIncrease;  
        GlobalGameManager.Instance.fieldSize += fieldSizeIncrease;      
        GlobalGameManager.Instance.numberOfCollectablesPerType = numberOfCollectablesPerTypeIncrease; 

        // Destroy the existing player object
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayer != null)
        {
            Destroy(existingPlayer);
        }

        // Destroy all existing enemies
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            Destroy(enemy);
        }

        // Destroy all existing obstacles
        GameObject[] existingObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in existingObstacles)
        {
            Destroy(obstacle);
        }

        // Reset occupied positions
        occupiedPositions.Clear();

        // Generate the new level
        GenerateLevel();
    }
}
