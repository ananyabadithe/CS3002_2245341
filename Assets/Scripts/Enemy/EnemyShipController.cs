using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    // Health
    private float maxHealth = 100f;
    public float currentEnemyHealth = 100f;
    public float hitPenalty = 25f;

    private HealthManager healthManager;

    private void Awake()
    {
        if (healthManager == null)
        {
            healthManager = FindObjectOfType<HealthManager>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentEnemyHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Orb")){
            currentEnemyHealth =- hitPenalty;

            int healthprint = (int)currentEnemyHealth;
            Debug.Log(healthprint);
        }
    }
}
