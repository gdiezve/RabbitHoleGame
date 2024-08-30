using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType
    {
        ChangeScale,       // Adds a value to the player's scale
        ChangeSprite    // Changes the player's sprite
    }

    public CollectableType type; // The type of the collectable
    public int value;          // The value to add/multiply/divide by
    public Sprite newSprite;     // The new sprite to change to (for ChangeSprite type)

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out var playerController))
            {
                ApplyEffect(playerController);
            }

            Destroy(gameObject); // Destroy the collectable after it's collected
        }
    }

    void ApplyEffect(PlayerController player)
    {
        switch (type)
        {
            case CollectableType.ChangeScale:
                player.ChangeScale(value);
                break;
            case CollectableType.ChangeSprite:
                player.ChangeSprite(newSprite);
                break;
        }
    }
}
