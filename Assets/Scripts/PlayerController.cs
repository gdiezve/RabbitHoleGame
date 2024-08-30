using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 originalScale;
    private SpriteRenderer playerSpriteRenderer, bulletSpawnerSpriteRenderer;
    private Transform bulletSpawnerTransform;
    public GameObject winnerMenuUI;
    public GameObject keepTryingMenuUI;
    public GameObject loserMenuUI;
    public GameObject playerHealthUI;
    public GameObject heartPrefab;
    public int life = 100;
    public Sprite triangleSprite;
    private readonly int maxHealth = 5;
    private readonly List<GameObject> hearts = new();
    private readonly Color orangeColor = new(0.96f, 0.49f, 0f, 1); 

    void Start()
    {
        originalScale = transform.localScale;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        bulletSpawnerTransform = playerSpriteRenderer.gameObject.transform.Find("BulletSpawner");
        bulletSpawnerSpriteRenderer = bulletSpawnerTransform.GetComponent<SpriteRenderer>();
        SetInitialHealth();
    }
    
    void Update()
    {
        // Get input from arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the movement vector
        Vector3 movement = new(moveHorizontal, moveVertical, 0.0f);

        // Move the character
        transform.Translate(GlobalGameManager.Instance.playerSpeed * Time.deltaTime * movement, Space.World);

        // Rotate the player to face the mouse position
        RotateTowardsMouse();
    }

    public void ChangeScale(int scaleValue)
    {
        // Add the scale value to the player's current scale
        if (scaleValue < 0 && transform.localScale.x == 1) {
            Debug.Log("You're already too small!");
        } else {
            transform.localScale += new Vector3(scaleValue, scaleValue, 0);
        }
    }
    public void ChangeSprite(Sprite newSprite)
    {
        // Change the player's sprite to the new sprite
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sprite = newSprite;
            playerSpriteRenderer.color = orangeColor;
            bulletSpawnerSpriteRenderer.color = orangeColor;
            //bulletSpawnerTransform.position = new Vector3(0.3f, 0.1f, 0); 
        }
    }

    void RotateTowardsMouse()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Keep the Z position consistent (assuming 2D)

        // Calculate the direction from the player to the mouse
        Vector3 direction = mousePosition - transform.position;

        // Calculate the angle between the player's forward direction and the direction to the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the player to face the mouse
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void SetInitialHealth() {
        float spaceBetweenHearts = 50f;
        float xPosition = -60f;
        for (int i=0; i<maxHealth; i++) {
            GameObject newHeart = Instantiate(heartPrefab);
            hearts.Add(newHeart);

            newHeart.transform.SetParent(playerHealthUI.transform);
            newHeart.layer = 5;

            newHeart.transform.localPosition = new Vector3(xPosition, 0f, 0f);
            newHeart.transform.localScale = new Vector3(16f, 16f, 1f);
            xPosition += spaceBetweenHearts;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            CheckEnemy(collision);
        } else if (collision.gameObject.CompareTag("Hole")) {
            CheckWin();
        }
    }

    void CheckWin() {
        GameObject hole = GameObject.FindGameObjectWithTag("Hole");
        if (hole.transform.localScale == transform.localScale) {
            Debug.Log("Congrats, you win!");
            Time.timeScale = 0;
            winnerMenuUI.SetActive(true); 
        } else {
            Debug.Log("Not the right size, keep trying!");
            Time.timeScale = 0;
            keepTryingMenuUI.SetActive(true);
        }
    }

    void CheckEnemy(Collider2D collision) {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy.type == Enemy.EnemyType.Triangle) {
            ChangeSprite(triangleSprite);
        }

        // Reduce health by destroying a heart
        life -= 20;
        GameObject heartToDestroy = hearts.Last();
        Destroy(heartToDestroy);
        hearts.Remove(heartToDestroy);

        if (life <= 0) {
            Debug.Log("Game Over!");
            // TODO: Display dead animation
            Time.timeScale = 0;
            loserMenuUI.SetActive(true); 
        }
        // TODO: Display hurt animation and inmune during 2 seconds to allow scaping

        Destroy(collision.gameObject);
    }
}
