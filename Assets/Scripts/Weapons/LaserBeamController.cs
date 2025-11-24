using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class LaserBeamController : MonoBehaviour
{
    [Header("Laser Difficulty Settings")]
    [SerializeField] private float baseSpeed = 5f;
    //[SerializeField] private float speedMultiplierLevel2 = 1.5f;
    [SerializeField] private float horizontalSpreadLevel1 = 0.5f;
    [SerializeField] private float horizontalSpreadLevel2 = 1.5f;

    [Space]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip beamContactClip;

    [Header("Visual Settings")]
    [SerializeField] private Transform spriteTransform;

    public float DifficultyFactor { get; set; } = 1.0f;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private HealthManager playerHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
        audioSource.playOnAwake = false;

        GameObject player = GameObject.Find("PlayerPaddle");
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthManager>();
        }
        else
        {
            Debug.LogError("HealthManager component not found on PlayerPaddle. Cannot subscribe to death event.");
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            DifficultyFactor = 3f;

        if (playerHealth != null)
            playerHealth.OnDeath += Die;

        LaunchLaser();
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnDeath -= Die;
    }

    private void Die(GameObject obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        UpdateSpriteRotation();
    }

    private void LaunchLaser()
    {
        float currentSpeed = baseSpeed;
        float currentSpread = horizontalSpreadLevel1;

        if (DifficultyFactor > 1.0f)
        {
            currentSpeed = baseSpeed * 1.5f;
            currentSpread = horizontalSpreadLevel2;
        }

        float randX = Random.Range(-currentSpread, currentSpread);
        Vector2 spawnDir = new Vector2(randX, -1f).normalized;

        rb.velocity = spawnDir * currentSpeed;

        UpdateSpriteRotation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 currentVelocity = rb.velocity;

        switch (other.tag)
        {
            case "BottomBoundary":
                if (playerHealth != null)
                    playerHealth.TakeDamage(10f);
                DestroyLaserWithExplosion();
                break;

            case "TopBoundary":
                Destroy(gameObject);
                break;

            case "Player":
                PlayBeamSound();
                rb.velocity = new Vector2(currentVelocity.x, Mathf.Abs(currentVelocity.y)).normalized * rb.velocity.magnitude;
                break;

            case "BounceBoundary":
                rb.velocity = new Vector2(-currentVelocity.x, currentVelocity.y).normalized * rb.velocity.magnitude;
                break;
        }
    }

    private void UpdateSpriteRotation()
    {
        if (spriteTransform == null) return;

        Vector2 velocity = rb.velocity;
        if (velocity.sqrMagnitude < 0.001f) return;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }

    private void DestroyLaserWithExplosion()
    {
        if (explosionPrefab != null)
        {
            var explosionClone = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (explosionClone.TryGetComponent(out ParticleSystemRenderer renderer))
            {
                renderer.sortingOrder = 100;
            }
        }

        Destroy(gameObject);
    }

    private void PlayBeamSound()
    {
        if (beamContactClip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(beamContactClip);
        }
    }
}
