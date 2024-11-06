using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] sprites;
    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;
    public float speedBoostMultiplier = 2f;
    public float invisibilityDuration = 5f;
    public GameObject invisibilityShield;

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private int spriteIndex;
    private bool isSpeedBoosted = false;
    private bool isInvisible = false;
    private float speedBoostEndTime;
    private float invisibilityEndTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        invisibilityShield.SetActive(false);
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
        // Handling player input for jumping
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
        }

        // Apply gravity and update the position
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime * (isSpeedBoosted ? speedBoostMultiplier : 1f);

        // Tilt the bird based on the direction
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;

        // Check for speed boost expiration
        if (isSpeedBoosted && Time.time > speedBoostEndTime)
        {
            isSpeedBoosted = false;
        }

        // Check for invisibility expiration
        if (isInvisible && Time.time > invisibilityEndTime)
        {
            isInvisible = false;
            invisibilityShield.SetActive(false);
        }
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

    public void ActivateSpeedBoost()
    {
        isSpeedBoosted = true;
        speedBoostEndTime = Time.time + 5f; // 5 seconds speed boost duration
    }

    public void ActivateInvisibility()
    {
        isInvisible = true;
        invisibilityEndTime = Time.time + invisibilityDuration;
        invisibilityShield.SetActive(true);
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
    }
}
