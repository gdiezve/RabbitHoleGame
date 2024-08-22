using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableManager : MonoBehaviour
{
    public GameObject[] collectablePrefabs; // Array of collectable prefabs
    private int[] initialCollectableCounts; // Stores initial counts for each type
    private bool[] respawnTimers; // To prevent multiple coroutines from running for the same type
    int halfFieldSize;
    readonly HashSet<Vector2> occupiedPositions = new(); // To avoid overlap with existing objects

    void Start()
    {
        halfFieldSize = GlobalGameManager.Instance.fieldSize / 2;
        // Initialize initial counts and timers based on the collectable prefabs array length
        initialCollectableCounts = new int[collectablePrefabs.Length];
        respawnTimers = new bool[collectablePrefabs.Length];

        for (int i = 0; i < collectablePrefabs.Length; i++)
        {
            initialCollectableCounts[i] = GlobalGameManager.Instance.numberOfCollectablesPerType;
            respawnTimers[i] = false;
        }

        // Spawn initial collectables
        int totalCollectableTypes = collectablePrefabs.Length;

        for (int i = 0; i < totalCollectableTypes; i++)
        {
            for (int j = 0; j < GlobalGameManager.Instance.numberOfCollectablesPerType; j++)
            {
                Vector2 collectablePosition = GenerateRandomPosition(halfFieldSize);

                // Ensure collectable is not on an obstacle, wall, or too close to the player/finish
                if (occupiedPositions.Contains(collectablePosition))
                {
                    j--; // Retry this iteration
                    continue;
                }

                // Instantiate the collectable of the current type
                Instantiate(collectablePrefabs[i], new Vector3(collectablePosition.x, collectablePosition.y, 0), Quaternion.identity);
                occupiedPositions.Add(collectablePosition);
            }
        }
    }

    void Update()
    {
        CheckAndRespawnCollectables();
    }

    void CheckAndRespawnCollectables()
    {
        for (int i = 0; i < collectablePrefabs.Length; i++)
        {
            // Find all active collectables of this type
            GameObject[] existingCollectables = GameObject.FindGameObjectsWithTag(collectablePrefabs[i].tag);

            // Check if the count is less than expected
            if (existingCollectables.Length < initialCollectableCounts[i] && !respawnTimers[i])
            {
                // Start the respawn coroutine for this collectable type
                StartCoroutine(RespawnCollectables(i, initialCollectableCounts[i] - existingCollectables.Length));
            }
        }
    }

    IEnumerator RespawnCollectables(int typeIndex, int amountToRespawn)
    {
        // Set the timer for this type to prevent multiple coroutines
        respawnTimers[typeIndex] = true;

        // Wait for 5 seconds before respawning
        yield return new WaitForSeconds(5f);

        // Find and store occupied positions to avoid respawning on them
        foreach (GameObject collectable in GameObject.FindGameObjectsWithTag(collectablePrefabs[typeIndex].tag))
        {
            occupiedPositions.Add(collectable.transform.position);
        }

        for (int i = 0; i < amountToRespawn; i++)
        {
            Vector2 spawnPosition;
            do
            {
                spawnPosition = new Vector2(
                    Random.Range(-halfFieldSize + 3, halfFieldSize - 3),
                    Random.Range(-halfFieldSize + 3, halfFieldSize - 3)
                );
            } while (occupiedPositions.Contains(spawnPosition));

            Instantiate(collectablePrefabs[typeIndex], new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
            occupiedPositions.Add(spawnPosition); // Mark the position as occupied
        }

        // Reset the timer so that the collectable can be respawned again if needed
        respawnTimers[typeIndex] = false;
    }

    Vector2 GenerateRandomPosition(int halfFieldSize)
    {
        int x = Random.Range(-halfFieldSize + 3, halfFieldSize - 3);
        int y = Random.Range(-halfFieldSize + 3, halfFieldSize - 3);
        return new Vector2(x, y);
    }
}
