using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Tambahkan ini agar bisa membaca TextMeshPro

public class EndGameUI : MonoBehaviour
{
    [Header("UI Element")]
    public TextMeshProUGUI skorAkhirText; // Tempat naruh objek teks angka kamu

    void Start()
    {
        // Ambil nilai skor akhir yang tersimpan di memori PlayerPrefs
        int skorSelesai = PlayerPrefs.GetInt("SkorAkhirGame", 0);

        // Tampilkan HANYA ANGKA saja secara bersih ke UI
        if (skorAkhirText != null)
        {
            skorAkhirText.text = skorSelesai.ToString();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("SplashScreen");
    }
}