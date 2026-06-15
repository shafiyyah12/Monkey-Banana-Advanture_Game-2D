using UnityEngine;
using TMPro;

public class GameOverScore : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        // Ambil langsung dari memori PlayerPrefs yang sama
        int skorSelesai = PlayerPrefs.GetInt("SkorAkhirGame", 0);
        
        Debug.Log("SKOR KALAH DI AMBIL DARI MEMORI = " + skorSelesai);

        // Tampilkan angka bersih ke UI
        if (scoreText != null)
        {
            scoreText.text = skorSelesai.ToString();
        }
    }
}