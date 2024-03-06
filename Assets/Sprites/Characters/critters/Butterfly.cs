using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // Speed of the butterfly
    [SerializeField] private float maxDistance = 5f; // Maximum distance from original position
    private Vector3 originalPosition; // Original position of the butterfly
    private Vector3 targetPosition; // Target position for the butterfly to move towards
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float changeDirectionTimer = 0f;
    private float changeDirectionTime = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        SetRandomTargetPosition();
    }

    void Update()
    {
        // Move towards the target position
        rb.MovePosition(Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime));

        // Check if the butterfly has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Set a new random target position
            SetRandomTargetPosition();
        }

        // Increment the timer
        changeDirectionTimer += Time.deltaTime;

        // Check if it's time to change direction
        if (changeDirectionTimer >= changeDirectionTime)
        {
            // Set a new random move direction
            SetRandomMoveDirection();
            changeDirectionTimer = 0f;
        }
    }

    // Set a new random target position within the maximum distance
    private void SetRandomTargetPosition()
    {
        targetPosition = originalPosition + Random.insideUnitSphere * maxDistance;
        targetPosition.z = originalPosition.z; // Keep the z position the same as the original
    }

    // Set a new random move direction
    private void SetRandomMoveDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate()
    {
        // Move in the random direction
        rb.velocity = moveDirection * speed;
    }
}
