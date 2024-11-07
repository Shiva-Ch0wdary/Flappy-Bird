using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 5f; // Speed at which the object moves left
    private float leftEdge;

    private void Start()
    {
        // Calculate the left edge of the screen for destroying objects that move out of view
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        // Move the object left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Destroy the object if it moves past the left edge of the screen
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
