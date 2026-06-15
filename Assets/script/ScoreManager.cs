using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    private static ScoreManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Mendaftarkan fungsi reset secara aman saat scene berubah
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Bersihkan pendaftaran jika object dihancurkan
        if (instance == this)
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
    }

    // Fungsi ini otomatis jalan SETIAP KALI ada perpindahan scene aktif
    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        // PENGAMAN: Jika kembali ke menu UTAMA atau baru mulai bermain lagi di STAGE 1, reset skor jadi 0
        if (next.name == "SplashScreen" || next.name == "MainMenu" || next.name == "Stage1")
        {
            ResetScore();
            ResetPlayerData();
            Debug.Log("Game Baru/Menu Dimuat: Semua data & skor berhasil di-RESET ke 0!");
        }
    }

    public static void AddScore(int amount)
{
    score += amount;
    // Paksa simpan skor terbaru ke memori Unity
    PlayerPrefs.SetInt("SkorAkhirGame", score);
    PlayerPrefs.Save();
    Debug.Log("Score Updated: " + score);
}

    public static void ResetScore()
    {
        score = 0;
    }

    private void ResetPlayerData()
    {
        // Mengirim perintah reset data kesehatan monyet secara aman
        System.Type type = System.Type.GetType("PlayerMovement");
        if (type != null)
        {
            System.Reflection.FieldInfo healthField = type.GetField("currentHealth", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            System.Reflection.FieldInfo appleField = type.GetField("jumlahKantongApel", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            
            if (healthField != null) healthField.SetValue(null, 100);
            if (appleField != null) appleField.SetValue(null, 0);
        }
    }
}