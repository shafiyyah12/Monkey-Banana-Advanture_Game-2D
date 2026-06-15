using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform pointA;
    public Transform pointB;
    
    [Header("Settings")]
    public float speed = 2f;
    
    private Vector3 worldA;
    private Vector3 worldB;
    private Vector3 nextTarget;

    void Start()
    {
        // Ambil koordinat murni dari dunia Unity saat game dimulai
        if (pointA != null && pointB != null)
        {
            worldA = pointA.position;
            worldB = pointB.position;
            nextTarget = worldB;
        }
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        // Gerakkan secara murni berdasarkan posisi dunia (World Space)
        transform.position = Vector3.MoveTowards(transform.position, nextTarget, speed * Time.deltaTime);

        // Cek jarak
        if (Vector2.Distance(transform.position, nextTarget) < 0.02f)
        {
            nextTarget = (nextTarget == worldB) ? worldA : worldB;
        }
    }

    // Supaya monyet nempel dan gak slip
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}