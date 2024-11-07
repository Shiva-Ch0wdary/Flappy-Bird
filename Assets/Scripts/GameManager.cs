using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;

    public int score { get; private set; } = 0;

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
    }

    public void GameOver()
    {
        playButton.SetActive(true);
        gameOver.SetActive(true);

        Pause();
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
    }

    // Method to activate invisibility on the player, with the shield prefab
    public void ActivateInvisibility(float duration, GameObject shieldPrefab)
    {
        player.ActivateInvisibility(duration, shieldPrefab);
    }
}
