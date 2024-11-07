using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Text powerUpTimerText; // Text element to display the power-up timer

    public int score { get; private set; } = 0;

    private Coroutine powerUpTimerCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Pause();
        powerUpTimerText.text = ""; // Clear the timer text at the start
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        // Reset the player to normal state
        player.ResetPowerUps();

        // Stop the timer and clear the timer text
        StopPowerUpTimer();

        // Destroy existing pipes and power-ups when the game restarts
        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

        // Destroy all speed power-ups
        GameObject[] speedPowerUps = GameObject.FindGameObjectsWithTag("SpeedPowerUp");
        for (int i = 0; i < speedPowerUps.Length; i++)
        {
            Destroy(speedPowerUps[i]);
        }

        // Destroy all invisible power-ups
        GameObject[] invisiblePowerUps = GameObject.FindGameObjectsWithTag("InvisiblePowerUp");
        for (int i = 0; i < invisiblePowerUps.Length; i++)
        {
            Destroy(invisiblePowerUps[i]);
        }

        powerUpTimerText.text = ""; // Clear any active timer text when restarting the game
    }

    public void GameOver()
    {
        playButton.SetActive(true);
        gameOver.SetActive(true);

        Pause();

        // Stop the timer and clear the timer text
        StopPowerUpTimer();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    // Method to activate speed boost on the player
    public void ActivateSpeedBoost(float duration)
    {
        player.ActivateSpeedBoost(duration);
        StartPowerUpTimer(duration); // Start the power-up timer
    }

    // Method to activate invisibility on the player, with the shield prefab
    public void ActivateInvisibility(float duration, GameObject shieldPrefab)
    {
        player.ActivateInvisibility(duration, shieldPrefab);
        StartPowerUpTimer(duration); // Start the power-up timer
    }

    private void StartPowerUpTimer(float duration)
    {
        // Stop any ongoing timer to avoid overlapping
        StopPowerUpTimer();

        // Start a new timer coroutine
        powerUpTimerCoroutine = StartCoroutine(PowerUpTimerCoroutine(duration));
    }

    private IEnumerator PowerUpTimerCoroutine(float duration)
    {
        float remainingTime = duration;

        // Update the timer text until the duration ends
        while (remainingTime > 0f)
        {
            powerUpTimerText.text = $"Power-Up: {remainingTime:F1}s";
            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        // Clear the timer text when the power-up expires
        powerUpTimerText.text = "";
    }

    private void StopPowerUpTimer()
    {
        if (powerUpTimerCoroutine != null)
        {
            StopCoroutine(powerUpTimerCoroutine);
            powerUpTimerCoroutine = null;
        }
        powerUpTimerText.text = ""; // Clear the timer text
    }
}
