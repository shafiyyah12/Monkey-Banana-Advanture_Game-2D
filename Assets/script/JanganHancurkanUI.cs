using UnityEngine;

public class JanganHancurkanUI : MonoBehaviour
{
    private static JanganHancurkanUI instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Bikin Canvas UI awet antar stage
        }
        else
        {
            Destroy(gameObject); // Hancurkan duplikat Canvas di stage berikutnya
        }
    }
}