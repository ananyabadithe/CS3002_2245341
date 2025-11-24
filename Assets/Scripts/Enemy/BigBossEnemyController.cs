using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBossEnemyController : MonoBehaviour
{
    public AsteroidController asteroid;

    private Rigidbody2D rb;
    private float speed = 2.5f;
    private HealthManager hm;

    [SerializeField] private float dodgeTriggerDistanceY = 5f;
    [SerializeField] private float dodgeTriggerAlignmentX = 2f;

    private float randomAsteroidThrowTime;
    private float lastThrowTime = 0f;

    private float nextDodgeCheck = 0f;
    private float dodgeInterval = 0.25f; 
    private int dodgeChance = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hm = GetComponent<HealthManager>();

        rb.transform.position = new Vector3(-8f, 12f, 0f);
        rb.velocity = (Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right) * speed;

        randomAsteroidThrowTime = Random.Range(0f, 1.5f);
    }

    void Update()
    {
        if (Time.time >= nextDodgeCheck)
        {
            nextDodgeCheck = Time.time + dodgeInterval;
            dodgeChance = Random.Range(1, 4);
        }

        HandleDodging();

        if (CanThrow())
        {
            Instantiate(asteroid, transform.position, Quaternion.identity);

            lastThrowTime = Time.time;
            randomAsteroidThrowTime = Random.Range(5f, 7f);
        }
    }

    private bool CanThrow()
    {
        return Time.time - lastThrowTime >= randomAsteroidThrowTime;
    }

    private void HandleDodging()
    {
        if (hm == null || hm.healthAmount > 50) return;

        int requiredRoll = hm.healthAmount > 25 ? 2 : 1;

        if (dodgeChance != requiredRoll) return;

        if (BulletController.activeBullets != null && BulletController.activeBullets.Count > 0)
        {
            Transform bullet = BulletController.activeBullets[0].transform;

            float verticalDiff = transform.position.y - bullet.position.y;
            float horizontalDiff = Mathf.Abs(transform.position.x - bullet.position.x);

            bool inDodgeRange =
                verticalDiff <= dodgeTriggerDistanceY &&
                verticalDiff > 0f &&
                horizontalDiff <= dodgeTriggerAlignmentX;

            if (inDodgeRange)
            {
                float randX = Random.Range(-16f, -1f);
                rb.transform.position = new Vector3(randX, transform.position.y, transform.position.z);

                rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BounceBoundary"))
            rb.velocity *= -1;
    }
}
