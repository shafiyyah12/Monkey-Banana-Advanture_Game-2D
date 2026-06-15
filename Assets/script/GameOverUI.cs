using UnityEngine;
using TMPro; // Wajib karena kita pakai TextMeshPro

public class GameOverUI : MonoBehaviour
{
    [Header("UI Element")]
    public TextMeshProUGUI skorAkhirText;

    void Start()
    {
        // PERBAIKAN: Ambil nilai skor akhir yang tersimpan di memori PlayerPrefs
        int skorSelesai = PlayerPrefs.GetInt("SkorAkhirGame", 0);

        // PERBAIKAN: Tampilkan HANYA ANGKA saja secara bersih tanpa kata "SKOR:"
        if (skorAkhirText != null)
        {
            skorAkhirText.text = skorSelesai.ToString();
        }
    }
}