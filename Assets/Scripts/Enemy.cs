using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Circle,       // Adds a value to the player's scale
        Triangle    // Changes the player's sprite
    }
    
    public Transform player; // Reference to the player's transform
    public EnemyType type;

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = player.position - transform.position;
            direction.Normalize(); // Normalize the direction vector to get the direction only

            // Move the enemy towards the player
            transform.position += GlobalGameManager.Instance.enemySpeed * Time.deltaTime * direction;
        }
    }
}
