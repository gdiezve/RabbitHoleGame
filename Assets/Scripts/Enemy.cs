using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    

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
