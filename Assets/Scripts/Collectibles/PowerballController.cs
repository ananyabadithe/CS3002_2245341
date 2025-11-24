using UnityEngine;

public class PowerballController : MonoBehaviour
{
    private float speed = 2f;
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.down;
    //[SerializeField] private AudioClip powerballClip;

    private PlayerPaddleController playerPaddle;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerPaddle = player.GetComponent<PlayerPaddleController>();

        if (playerPaddle == null)
            Debug.LogError("PlayerPaddleController NOT found on Player!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        //if (collision.CompareTag("Player") && powerballClip != null)
        //{
        //    AudioSource.PlayClipAtPoint(powerballClip, transform.position, 1f);
        //}

        switch (tag)
        {
            case "Expand": playerPaddle.ApplyTemporaryEffect(null, 1.5f, 5f); break;
            case "Shrink": playerPaddle.ApplyTemporaryEffect(null, 0.5f, 5f); break;
            case "IncreaseSpeed": playerPaddle.ApplyTemporaryEffect(20f, null, 5f); break;
            case "DecreaseSpeed": playerPaddle.ApplyTemporaryEffect(5f, null, 5f); break;
            case "Heart": playerPaddle.GetComponent<HealthManager>().Heal(20f); break;
            case "Skull": playerPaddle.GetComponent<HealthManager>().TakeDamage(20f); break;
        }

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    //private void PlayBeamSound()
    //{
    //    if (audioSource != null)
    //    {
    //        audioSource.PlayOneShot(powerballClip);
    //    }
    //}
}
