using UnityEngine;
using System.Collections;

public class BossBattleManager : MonoBehaviour
{
    [Header("Enemy")]
    public EnemyShoot bearShoot;

    [Header("Player")]
    public PlayerMovement player;

    [Header("Phase Settings")]
    public float phase1ShootDelay = 2f;
    public float phase2ShootDelay = 1.2f;
    public float phase3ShootDelay = 0.6f;

    // TOTAL ROUND
    public int maxRounds = 3;

    private int currentRound = 0;

    private int currentPhase = 1;

    private bool battleEnded = false;

    void Start()
    {
        StartCoroutine(BattleLoop());
    }

    IEnumerator BattleLoop()
    {
        while (!battleEnded &&
               currentRound < maxRounds)
        {
            currentRound++;

            Debug.Log(
                "ROUND " + currentRound
            );

            // ======================
            // TURN MONYET
            // ======================

            Debug.Log("Giliran Monyet");

            player.canAttack = true;

            bearShoot.canShoot = false;

            yield return new WaitUntil(() =>
                player.IsMonkeyTurnFinished()
            );

            player.ResetMonkeyTurn();

            // ======================
            // TURN BERUANG
            // ======================

            Debug.Log("Giliran Beruang");

            player.canAttack = false;

            bearShoot.canShoot = true;

            // SPEED BERDASARKAN PHASE
            if (currentPhase == 1)
            {
                bearShoot.shootDelay =
                    phase1ShootDelay;
            }
            else if (currentPhase == 2)
            {
                bearShoot.shootDelay =
                    phase2ShootDelay;
            }
            else if (currentPhase == 3)
            {
                bearShoot.shootDelay =
                    phase3ShootDelay;
            }

            // BERUANG ATTACK 5 DETIK
            yield return new WaitForSeconds(5f);

            bearShoot.canShoot = false;
        }

        Debug.Log("3 ROUND SELESAI");
    }

    public void SetPhase(int phase)
    {
        currentPhase = phase;

        Debug.Log(
            "Masuk Phase " + phase
        );
    }

    public void EndBattle()
    {
        battleEnded = true;

        player.canAttack = false;

        bearShoot.canShoot = false;

        Debug.Log("Battle selesai");
    }
}