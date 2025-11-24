using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPaddleController : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody2D rb;

    private Vector2 moveInput;
   
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float x_scale = 1f;

    [SerializeField] private float shootCooldown;
    private float lastShootTime = 0f;

    private Coroutine activeEffect;

    [SerializeField] private AudioClip bulletSound;

    private AudioSource audioSource;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public float XScale
    {
        get => x_scale;
        set
        {
            x_scale = Mathf.Clamp(value, 0.5f, 5f);
            Vector3 newScale = transform.localScale;
            newScale.x = x_scale;
            transform.localScale = newScale;
        }
    }

    public void ApplyTemporaryEffect(float? speed, float? xScale, float duration)
    {
        if (activeEffect != null) StopCoroutine(activeEffect);
        activeEffect = StartCoroutine(TemporaryEffectCoroutine(speed, xScale, duration));
    }

    private IEnumerator TemporaryEffectCoroutine(float? speed, float? xScale, float duration)
    {
        float orginalSpeed = 10f;
        float orginalXscale = 1.0f;

        if (speed.HasValue) MoveSpeed = speed.Value;
        if (xScale.HasValue) XScale = xScale.Value;

        yield return new WaitForSeconds(duration);

        MoveSpeed = orginalSpeed;
        XScale = orginalXscale;
        activeEffect = null;

    }

    private void Update()
    {
        //Debug.Log(moveSpeed);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            shootCooldown = 1f;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            shootCooldown = 0.5f;
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
            return;
        }
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        moveInput.y = 0;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && CanShoot())
        {
            if (bulletSound != null)
                audioSource.PlayOneShot(bulletSound);
            Instantiate(bullet, transform.position, transform.rotation);
            lastShootTime = Time.time;
        }
    }

    private bool CanShoot()
    {
        return Time.time - lastShootTime >= shootCooldown && BulletController.activeBullets.Count < 2 ;
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.velocity = new Vector2(moveInput.x * MoveSpeed, 0f);

        float clampedX = Mathf.Clamp(transform.position.x, -16.3f, -0.5f);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
