using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidController : MonoBehaviour
{
    [SerializeField] private AudioClip asteroidSplitClip;

    [Header("Movement Settings")]
    [SerializeField] private float minLaunchSpeed = 1.5f;
    [SerializeField] private float maxLaunchSpeed = 2.5f;
    [SerializeField] private float rotationalSpeed = 50f;

    [Header("Splitting Logic (REQUIRED)")]
    public GameObject smallerAsteroidPrefab;
    [SerializeField] private int splitCount = 2;
    //[SerializeField] private float splitForce = 150f;

    private HealthManager playerHealth;
    private Rigidbody2D rb;

    public void LaunchFromBoss(Rigidbody2D rb, bool small)
    {
        if (rb == null) return;

        float randX = Random.Range(-0.8f, 0.8f);
        Vector2 spawnDir = new Vector2(randX, -1f).normalized;
        float currentSpeed = Random.Range(minLaunchSpeed, maxLaunchSpeed);

        if (!small)
        {
            rb.velocity = spawnDir * currentSpeed;
            rb.angularVelocity = Random.Range(-rotationalSpeed, rotationalSpeed);
        }
        else
        {
            rb.velocity = spawnDir * currentSpeed * -1;
            rb.angularVelocity = Random.Range(-rotationalSpeed, rotationalSpeed);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            LaunchFromBoss(rb, false);

        GameObject player = GameObject.Find("PlayerPaddle");
        if (player != null)
            playerHealth = player.GetComponent<HealthManager>();
        else
            Debug.LogError("HealthManager component not found on PlayerPaddle. Cannot subscribe to death event.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 currentVelocity = rb.velocity;

        switch (other.tag)
        {
            case "BottomBoundary":
                if (playerHealth != null)
                    playerHealth.TakeDamage(10f);
                Destroy(gameObject);
                break;

            case "TopBoundary":
                rb.velocity = new Vector2(currentVelocity.x, -currentVelocity.y).normalized * rb.velocity.magnitude;
                break;

            case "Player":
                rb.velocity = new Vector2(currentVelocity.x, Mathf.Abs(currentVelocity.y)).normalized * rb.velocity.magnitude;
                break;

            case "Bullet":
                Destroy(other.gameObject);
                HandleShotByGun();
                break;

            case "BounceBoundary":
                rb.velocity = new Vector2(-currentVelocity.x, currentVelocity.y).normalized * rb.velocity.magnitude;
                break;
        }
    }

    private void HandleShotByGun()
    {
        if (smallerAsteroidPrefab == null)
        {
            Debug.Log("Smallest asteroid destroyed! Deal damage to Boss.");
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < splitCount; i++)
        {
            if (asteroidSplitClip != null)
                AudioSource.PlayClipAtPoint(asteroidSplitClip, transform.position, 1.5f);

            GameObject newAsteroid = Instantiate(smallerAsteroidPrefab, transform.position, Quaternion.identity);
            Rigidbody2D newRb = newAsteroid.GetComponent<Rigidbody2D>();

            if (newRb != null)
                LaunchFromBoss(newRb, true);
        }

        if (CollectiblesController.Instance != null)
            CollectiblesController.Instance.GeneratePowerball(transform.position);

        Destroy(gameObject);
    }
}
