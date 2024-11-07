using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUps; // Array to hold the power-up prefabs
    public float spawnInterval = 2f; // Time between spawns
    private float timer = 0f;

    [SerializeField] private Transform playerTransform; // Reference to player's position
    [SerializeField] private float spawnDistance = 5f; // Distance in front of the player to spawn power-ups

    private void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnPowerUp();
            timer = spawnInterval;
        }
    }

    private void SpawnPowerUp()
    {
        // Randomly choose a power-up prefab
        int randomIndex = Random.Range(0, powerUps.Length);
        GameObject selectedPowerUp = powerUps[randomIndex];

        // Ensure the power-up has a Collider2D set to trigger
        if (selectedPowerUp.GetComponent<Collider2D>() == null)
        {
            selectedPowerUp.AddComponent<BoxCollider2D>().isTrigger = true;
        }

        // Position the power-up in front of the player and align it between pipes
        float spawnY = GetRandomYPositionBetweenPipes();
        Vector3 spawnPosition = new Vector3(playerTransform.position.x + spawnDistance, spawnY, 0f);

        Instantiate(selectedPowerUp, spawnPosition, Quaternion.identity);
    }

    private float GetRandomYPositionBetweenPipes()
    {
        // Adjust this value according to the gap between your pipe prefabs
        float pipeGapMinY = -1f; // Replace with actual min Y of the pipe gap
        float pipeGapMaxY = 1f; // Replace with actual max Y of the pipe gap

        return Random.Range(pipeGapMinY, pipeGapMaxY);
    }
}
