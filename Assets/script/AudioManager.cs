using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Variabel static untuk menyimpan satu-satunya instance AudioManager
    private static AudioManager instance;

    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----- Audio Clips -----")] // Sedikit koreksi header biar ga bingung
    public AudioClip background;
    public AudioClip death;
    public AudioClip checkpoint;
    public AudioClip waLLTouch;
    public AudioClip portalIn;
    public AudioClip portalOut;

    private void Awake()
    {
        // Logika Singleton + DontDestroyOnLoad
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Menjaga objek ini tetap hidup saat pindah scene
        }
        else
        {
            Destroy(gameObject); // Menghancurkan duplikat jika balik ke scene Menu
        }
    }

    private void Start()
    {
        // Musik background langsung jalan di awal game
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true; // Memastikan musiknya looping terus-menerus
            musicSource.Play();
        }
    }
}