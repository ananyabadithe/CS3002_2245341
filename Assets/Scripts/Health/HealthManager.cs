using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth;
    public float healthAmount;

    public event Action<GameObject> OnDeath;
    public event Action<float, float> OnHealthChanged;

    public bool powerballSpawned = false;

    private void Start()
    {
        healthAmount = maxHealth;
    }

    private void Update()
    {
        // Intentionally left empty
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
        UpdateHealthBar();

        if (healthAmount <= 0)
        {
            OnDeath?.Invoke(gameObject);
        }
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / maxHealth;
        }

        OnHealthChanged?.Invoke(healthAmount, maxHealth);
    }
}
