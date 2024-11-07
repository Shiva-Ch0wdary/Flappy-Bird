using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Sprite[] sprites;
    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;
    public GameObject shieldPrefab; // Prefab for the hexagonal shield effect

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private int spriteIndex;
    private bool isInvisible = false;
    private float originalStrength; // Store the original strength for resetting
    public float maxHeight = 4f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalStrength = strength; // Store the initial strength value
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {
        // check maximum height
        if (transform.position.y > maxHeight)
        {
            Vector3 position = transform.position;
            position.y = maxHeight;
            transform.position = position;
        }


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
        }

        // Apply gravity and update the position
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // Tilt the bird based on the direction
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        if (spriteIndex < sprites.Length && spriteIndex >= 0)
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    public void ActivateSpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(duration));
    }

    private IEnumerator SpeedBoostCoroutine(float duration)
    {
        float originalStrength = strength;
        strength *= 1.5f; // Double the movement strength for speed boost

        yield return new WaitForSeconds(duration);

        strength = originalStrength; // Reset to original speed
    }

    public void ActivateInvisibility(float duration, GameObject shieldPrefab)
    {
        StartCoroutine(InvisibilityCoroutine(duration, shieldPrefab));
    }

    private IEnumerator InvisibilityCoroutine(float duration, GameObject shieldPrefab)
    {
        isInvisible = true;

        // Instantiate shield effect
        GameObject shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
        shield.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(duration);

        // Remove invisibility effect
        isInvisible = false;
        Destroy(shield);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle") && !isInvisible)
        {
            GameManager.Instance.GameOver();
        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            GameManager.Instance.IncreaseScore();
        }
        else if (other.gameObject.CompareTag("SpeedPowerUp"))
        {
            GameManager.Instance.ActivateSpeedBoost(5f); // Set duration as needed
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("InvisiblePowerUp"))
        {
            GameManager.Instance.ActivateInvisibility(5f, shieldPrefab); // Set duration as needed
            Destroy(other.gameObject);
        }
    }

    // Reset the player to the normal state (no power-ups)
    public void ResetPowerUps()
    {
        // Stop any active power-up coroutines
        StopAllCoroutines();

        // Reset power-up states
        isInvisible = false;
        strength = originalStrength; // Reset strength to original value
    }
}
