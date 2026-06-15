using UnityEngine;

public class BananaProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 4f;

    // seberapa kuat tracking
    public float trackingSpeed = 2f;

    // berapa lama tracking aktif
    public float homingTime = 0.7f;

    [Header("Lifetime")]
    public float destroyTime = 5f;

    [Header("Damage")]
    public int damage = 1;

    // TRUE = projectile beruang
    // FALSE = projectile monyet
    public bool isEnemyProjectile = false;

    private Transform target;

    private bool sudahHit = false;

    private Rigidbody2D rb;

    private Vector2 moveDirection;

    private float homingTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Transform enemyTarget)
    {
        target = enemyTarget;

        if (target != null)
        {
            moveDirection =
                (target.position - transform.position)
                .normalized;
        }
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);

        homingTimer = homingTime;
    }

    void FixedUpdate()
    {
        if (sudahHit) return;

        // ====================================
        // HOMING HANYA BEBERAPA DETIK
        // ====================================

        if (target != null && homingTimer > 0)
        {
            homingTimer -= Time.fixedDeltaTime;

            Vector2 targetDirection =
                (
                    target.position -
                    transform.position
                ).normalized;

            moveDirection = Vector2.Lerp(
                moveDirection,
                targetDirection,
                trackingSpeed * Time.fixedDeltaTime
            );
        }

        // GERAK
        rb.linearVelocity =
            moveDirection.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sudahHit) return;

        // ====================================
        // PROJECTILE MONYET
        // DAMAGE BERUANG
        // ====================================

        if (!isEnemyProjectile)
        {
            BearHealth bear =
                collision.GetComponent<BearHealth>();

            if (bear != null)
            {
                sudahHit = true;

                bear.TakeDamage(damage);

                Destroy(gameObject);

                return;
            }
        }

        // ====================================
        // PROJECTILE BERUANG
        // DAMAGE PLAYER
        // ====================================

        if (isEnemyProjectile)
        {
            PlayerMovement player =
                collision.GetComponent<PlayerMovement>();

            if (player != null)
            {
                sudahHit = true;

                player.TakeDamage(50);

                Destroy(gameObject);

                return;
            }
        }

        // ====================================
        // KENA GROUND
        // ====================================

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}