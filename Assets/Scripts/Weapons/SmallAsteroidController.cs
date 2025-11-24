using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroidController : MonoBehaviour
{
    private HealthManager playerHealth;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 currentVelocity = rb.velocity;
        switch (collision.tag)
        {
            case "Bullet":
                Destroy(gameObject);
                break;
            case "BottomBoundary":
                if (playerHealth != null) playerHealth.TakeDamage(10f);
                Destroy(gameObject);
                break;

            case "TopBoundary":
                rb.velocity = new Vector2(currentVelocity.x, -currentVelocity.y).normalized * rb.velocity.magnitude;
                break;

            case "Player":
                rb.velocity = new Vector2(currentVelocity.x, Mathf.Abs(currentVelocity.y)).normalized * rb.velocity.magnitude;
                break;

            case "BounceBoundary":
                rb.velocity = new Vector2(-currentVelocity.x, currentVelocity.y).normalized * rb.velocity.magnitude;
                break;
        }
    }
}
