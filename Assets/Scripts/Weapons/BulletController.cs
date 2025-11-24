using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public static List<BulletController> activeBullets = new List<BulletController>();
    private EnemyAlienSpawner EnemyAlienSpawner;
    private EnemyShipSpawner EnemyShipSpawner;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip enemyDieClip;

    public float speed = 10f;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        activeBullets.Add(this);
    }

    private void OnDisable()
    {
        activeBullets.Remove(this);
    }

    private void Start()
    {
        EnemyAlienSpawner = FindObjectOfType<EnemyAlienSpawner>();
        EnemyShipSpawner = FindObjectOfType<EnemyShipSpawner>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        rb.velocity = Vector2.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TopBoundary"))
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("EnemyShip"))
            HandleHit(other, 50);

        if (other.CompareTag("Alien"))
            HandleHit(other, 10);

        if (other.CompareTag("BigBoss"))
            HandleHit(other, 10);
    }

    private void HandleHit(Collider2D other, int damage)
    {
        HealthManager hm = other.GetComponent<HealthManager>();
        if (hm != null)
        {
            hm.OnDeath -= Die;
            hm.OnDeath += Die;

            hm.TakeDamage(damage);
        }
        DestroyBulletWithExplosion();
    }


    private void Die(GameObject obj)
    {
        Vector3 safePos = obj.transform.position;

        HealthManager hm = obj.GetComponent<HealthManager>();

        if (obj.CompareTag("Alien") || obj.CompareTag("EnemyShip"))
        {
            if (enemyDieClip != null)
            {
                AudioSource.PlayClipAtPoint(enemyDieClip, obj.transform.position, 1.5f);
            }
        }

        if (obj.CompareTag("Alien"))
        {
            EnemyAlienSpawner.SpawnedAliens.Remove(obj);
        }
        if (obj.CompareTag("EnemyShip"))
        {
            EnemyShipSpawner.SpawnedShips.Remove(obj);
        }
        if (hm != null && !hm.powerballSpawned)
        {
            hm.powerballSpawned = true;

            int chance = Random.Range(0, 3);
            if (chance == 0 || chance == 2)
            {
                if (CollectiblesController.Instance != null)
                    CollectiblesController.Instance.GeneratePowerball(safePos);
            }
        }

        Destroy(obj);
    }


     
    private void DestroyBulletWithExplosion()
    {
        if (explosionPrefab != null)
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (explosion.TryGetComponent(out ParticleSystemRenderer renderer))
                renderer.sortingOrder = 100;
        }

        Destroy(gameObject);
    }
}
