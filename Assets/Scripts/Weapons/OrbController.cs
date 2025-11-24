using UnityEngine;

public class OrbController : MonoBehaviour
{
    [Header("Laser Settings")]
    public float speed = 8f;
    public float horizontalSpread = 1.5f;

    [Header("Visual Settings")]
    public Transform spriteTransform;
    private EnemyShipSpawner EnemyShipSpawner;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    public GameObject explosionPrefab;
    public AudioClip enemyDieClip;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        EnemyShipSpawner = FindObjectOfType<EnemyShipSpawner>();

        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
            circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;

        rb.gravityScale = 0;

        float randX = Random.Range(-horizontalSpread, horizontalSpread);
        float randY = 1f;
        Vector2 spawnDir = new Vector2(randX, randY).normalized;

        rb.velocity = spawnDir * speed;

        UpdateSpriteRotation();
    }

    private void Update()
    {
        UpdateSpriteRotation();
    }

    private void UpdateSpriteRotation()
    {
        if (spriteTransform == null) return;

        Vector2 velocity = rb.velocity;
        if (velocity.sqrMagnitude < 0.001f) return;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Die(GameObject obj)
    {
        Debug.Log(obj.transform.position.ToString());
        if (obj == null) return;

        HealthManager hm = obj.GetComponent<HealthManager>();

        if (obj.CompareTag("EnemyShip"))
        {
            if (enemyDieClip != null)
                AudioSource.PlayClipAtPoint(enemyDieClip, transform.position, 1.5f);

            EnemyShipSpawner.SpawnedShips.Remove(obj);
        }

        if (hm != null && !hm.powerballSpawned)
        {
            hm.powerballSpawned = true;

            int chance = Random.Range(0, 3);
            if (chance == 0 || chance == 2)
            {
                if (CollectiblesController.Instance != null)
                    CollectiblesController.Instance.GeneratePowerball(obj.transform.position);
            }
        }

        Destroy(obj);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 currentVelocity = rb.velocity;

        if (other.CompareTag("BottomBoundary"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("TopBoundary"))
        {
            rb.velocity = new Vector2(currentVelocity.x, -currentVelocity.y).normalized * speed;
        }
        else if (other.CompareTag("Player"))
        {
            rb.velocity = new Vector2(currentVelocity.x, Mathf.Abs(currentVelocity.y)).normalized * speed;
        }
        else if (other.CompareTag("BounceBoundary"))
        {
            rb.velocity = new Vector2(-currentVelocity.x, currentVelocity.y).normalized * speed;
        }
        else if (other.CompareTag("EnemyShip"))
        {
            HealthManager hm = other.GetComponent<HealthManager>();
            if (hm != null)
            {
                hm.OnDeath -= Die;
                hm.OnDeath += Die;

                hm.TakeDamage(50f);
            }

            if (explosionPrefab != null)
            {
                GameObject explosionClone = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                ParticleSystemRenderer renderer = explosionClone.GetComponent<ParticleSystemRenderer>();
                if (renderer != null)
                    renderer.sortingOrder = 100;
            }

            rb.velocity = new Vector2(-currentVelocity.x, currentVelocity.y).normalized * speed;
        }
    }
}
