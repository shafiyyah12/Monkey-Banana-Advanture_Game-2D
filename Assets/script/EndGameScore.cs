using UnityEngine;
using TMPro;

public class EndGameScore : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        // Ambil langsung dari memori PlayerPrefs, kalau kosong default-nya 0
        int skorSelesai = PlayerPrefs.GetInt("SkorAkhirGame", 0);
        
        Debug.Log("SKOR DI AMBIL DARI MEMORI = " + skorSelesai);

        // Tampilkan angka bersih ke UI
        if (scoreText != null)
        {
            scoreText.text = skorSelesai.ToString();
        }
    }
}