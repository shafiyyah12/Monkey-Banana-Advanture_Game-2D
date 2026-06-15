using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BearHealth : MonoBehaviour
{
    public int health = 6;

    [Header("Boss Battle")]
    public BossBattleManager bossManager;

    private SpriteRenderer[] sprites;

    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        // PENGAMAN: Jika beruang sudah mati, jangan proses damage lagi biar tidak bug
        if (health <= 0) return;

        health -= damage;
        Debug.Log("HP Beruang: " + health);

        StopAllCoroutines();
        StartCoroutine(HitFlash());

        // FASE 2
        if (health <= 4 && health > 2)
        {
            if (bossManager != null)
            {
                bossManager.SetPhase(2);
            }
        }

        // FASE 3
        if (health <= 2)
        {
            if (bossManager != null)
            {
                bossManager.SetPhase(3);
            }
        }

        // --- KONDISI BERUANG MATI (TETAP WAJIB ADA) ---
        if (health <= 0)
        {
            // PERBAIKAN: Baris ScoreManager.AddScore(100) SUDAH DIHAPUS dari sini 
            // karena sistem checkpoint/finish level kamu ternyata sudah otomatis menambah skor tersebut.

            Animator anim = GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetTrigger("death");
            }

            if (bossManager != null)
            {
                bossManager.EndBattle();
            }

            Invoke(nameof(LoadEndGame), 1f);
        }
    }

    void LoadEndGame()
    {
        SceneManager.LoadScene("Endgame");
    }

    IEnumerator HitFlash()
    {
        foreach (SpriteRenderer sr in sprites)
        {
            sr.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer sr in sprites)
        {
            sr.color = Color.white;
        }
    }
}