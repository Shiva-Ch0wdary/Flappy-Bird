using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject speedPowerUpPrefab;
    public GameObject invisiblePowerUpPrefab;
    public float spawnRate = 5f;
    public float minHeight = -1f;
    public float maxHeight = 2f;
    public float spawnDistanceInFront = 5f; // Distance to spawn in front of the current position

    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
        InvokeRepeating(nameof(SpawnPowerUp), spawnRate, spawnRate);
    }

    private void SpawnPowerUp()
    {
        // Randomly select a power-up to spawn
        GameObject powerUpPrefab = Random.value > 0.5f ? speedPowerUpPrefab : invisiblePowerUpPrefab;

        // Set spawn position in front of the spawner with a random vertical offset
        Vector3 spawnPosition = transform.position + Vector3.right * spawnDistanceInFront;
        spawnPosition.y += Random.Range(minHeight, maxHeight);

        // Instantiate the power-up at the adjusted position
        GameObject powerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}

public class MovingPowerUp : MonoBehaviour
{
    public float speed = 5f; // Same speed as pipes

    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Destroy power-up when it goes off screen
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
