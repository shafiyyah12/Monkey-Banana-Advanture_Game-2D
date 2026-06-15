using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    private Transform targetPoint;

    void Start()
    {
        targetPoint = pointB;
    }

    void Update()
    {
        // Gerak ke target
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Cek arah gerak buat muter badan
        HandleFlip();

        // Kalau sampai, ganti target
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = (targetPoint == pointB) ? pointA : pointB;
        }
    }

    void HandleFlip()
    {
        // Cek selisih posisi target dengan posisi sekarang
        float direction = targetPoint.position.x - transform.position.x;

        Vector3 scale = transform.localScale;

        // KARENA SPRITE ASLI KAMU HADAP KIRI:
        if (direction > 0.1f) // Mau ke KANAN
        {
            scale.x = -Mathf.Abs(scale.x); // Jadi negatif biar nengok kanan
        }
        else if (direction < -0.1f) // Mau ke KIRI
        {
            scale.x = Mathf.Abs(scale.x); // Jadi positif biar nengok kiri
        }

        transform.localScale = scale;
    }
}