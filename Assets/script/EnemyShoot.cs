using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Projectiles")]
    public GameObject fishProjectile;
    public GameObject woodProjectile;

    [Header("Shoot Point")]
    public Transform shootPoint;

    [Header("Target")]
    public Transform target;

    [Header("Settings")]
    public float shootDelay = 1.5f;

    public bool canShoot = true;

    private float timer;

    void Update()
    {
        if (!canShoot) return;

        timer += Time.deltaTime;

        if (timer >= shootDelay)
        {
            Shoot();

            timer = 0f;
        }
    }

    void Shoot()
    {
        if (shootPoint == null) return;

        Animator anim = GetComponent<Animator>();

        if (anim != null)
        {
            anim.SetTrigger("attack");
        }

        // RANDOM PROJECTILE
        GameObject selectedProjectile;

        int randomAmmo = Random.Range(0, 2);

        if (randomAmmo == 0)
        {
            selectedProjectile = fishProjectile;
        }
        else
        {
            selectedProjectile = woodProjectile;
        }

        if (selectedProjectile == null) return;

        GameObject obj = Instantiate(
            selectedProjectile,
            shootPoint.position,
            Quaternion.identity
        );

        BananaProjectile bp =
            obj.GetComponent<BananaProjectile>();

        // PROJECTILE MILIK BERUANG
        if (bp != null)
        {
            bp.isEnemyProjectile = true;
        }

        // TARGET PLAYER
        if (bp != null && target != null)
        {
            bp.SetTarget(target);
        }
    }
}