using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Kecepatan")]
    public float naikSpeed = 10f;    // Cepat pas naik
    public float turunSpeed = 2f;   // Pelan pas turun
    
    [Header("Pengaturan")]
    public float range = 1f;        // Tinggi duri muncul
    public float waitTime = 5f;     // Jeda 5 detik
    
    private Vector3 startPos;
    private bool isUp = false;      // Status apakah lagi di atas atau di bawah
    private float timer;

    void Start()
    {
        startPos = transform.position;
        timer = waitTime; // Biar pas mulai gak langsung naik
    }

    void Update()
    {
        // Tentukan posisi target berdasarkan status isUp
        Vector3 targetPos = isUp ? startPos + Vector3.up * range : startPos;
        
        // Tentukan kecepatan berdasarkan arah gerak
        float currentSpeed = isUp ? naikSpeed : turunSpeed;

        // Gerakkan spike
        transform.position = Vector3.MoveTowards(transform.position, targetPos, currentSpeed * Time.deltaTime);

        // Cek kalau sudah sampai di target
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            timer -= Time.deltaTime; // Hitung mundur waktu tunggu

            if (timer <= 0)
            {
                isUp = !isUp; // Tukar status (naik jadi turun, atau sebaliknya)
                timer = waitTime; // Reset timer ke 5 detik lagi
            }
        }
    }
}