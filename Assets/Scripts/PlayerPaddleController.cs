using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPaddleController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField] private float moveSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found! Please add one to your paddle GameObject.");
            return;
        }

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        moveInput.y = 0; // Only allow horizontal movement
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, 0f);
        rb.velocity = targetVelocity;
    }
}
