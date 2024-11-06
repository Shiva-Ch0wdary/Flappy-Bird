using UnityEngine;

public class InvisiblePowerUp : MovingPowerUp
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.ActivateInvisibility();
            }
            Destroy(gameObject); // Destroy the power-up after it's collected
        }
    }
}
