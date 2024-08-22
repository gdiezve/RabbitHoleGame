using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly string[] tags = new string[] {"Add1Modifier", "Substract1Modifier", "Obstacle"};

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the enemy
            Destroy(collision.gameObject);

            // Destroy the bullet
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (tags.Contains(collision.gameObject.tag))
        {
            // Destroy the bullet if it crashes into any other obstacle or collectable
            Destroy(gameObject);
        }
    }
}
