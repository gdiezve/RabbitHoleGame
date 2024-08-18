using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType
    {
        AddScale,       // Adds a value to the player's scale
        MultiplyScale,  // Multiplies the player's scale by a factor
        DivideScale,    // Divides the player's scale by a factor
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
            case CollectableType.AddScale:
                player.ChangeScale(value);
                break;
            case CollectableType.MultiplyScale:
                player.MultiplyScale(value);
                break;
            case CollectableType.DivideScale:
                player.DivideScale(value);
                break;
            case CollectableType.ChangeSprite:
                player.ChangeSprite(newSprite);
                break;
        }
    }
}
