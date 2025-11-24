using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [Header("References")]
    public Transform hValueBar;                 // The child bar object
    public PlayerPaddleController player;       // Reference to your player

    private Vector3 initialScale;

    void Start()
    {
        if (hValueBar == null)
        {
            Debug.LogError("HealthBarController: hValueBar not assigned!");
            enabled = false;
            return;
        }

        if (player == null)
        {
            Debug.LogError("HealthBarController: player not assigned!");
            enabled = false;
            return;
        }

        // Store the original scale to calculate percentage
        initialScale = hValueBar.localScale;
    }

    void Update()
    {

    }
}
