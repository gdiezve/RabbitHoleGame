using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform bulletSpawn;   // Spawn point for the bullet
    public float bulletSpeed = 15f; // Speed of the bullet
    public float fireRate = 0.5f;   // Time between shots
    private float fireTimer;
    AudioSource audioSoundEffect;

    void Awake() {
        audioSoundEffect = GetComponent<AudioSource>();
    }
    void Update()
    {
        fireTimer -= Time.deltaTime;

        // Check if the player presses the fire button (left mouse button)
        if (Input.GetButtonDown("Fire1") && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate; // Reset the fire timer
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the bulletSpawn position
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        
        // Apply a 90-degree rotation around the z-axis (for 2D)
        bullet.transform.Rotate(Vector3.forward * 90f);

        // Get the Rigidbody2D component from the bullet and set its velocity
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bulletSpawn.right * bulletSpeed; // Ensure the bullet moves in the right direction

        // Destroy the bullet after 2 seconds to prevent memory leaks
        Destroy(bullet, 2f);
        
        // Play bullet sound effect
        audioSoundEffect.Play();
    }
}
