using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("UI System")]
    [Tooltip("Pastikan ini menunjuk ke teks UI angka skor di canvas")]
    public TextMeshProUGUI skorText;

    [Header("Heart UI")]
    public Image hatiImage;
    public Sprite hatiFull;
    public Sprite hatiHalf;

    [Header("Apple UI (Kantong Penyimpanan)")]
    [Tooltip("Masukkan 3 Slot Apel UI secara berurutan: Element 0 = Kiri, Element 1 = Tengah, Element 2 = Kanan")]
    public GameObject[] slotApelUI; 

    [Header("Attack")]
    public GameObject bananaPrefab;
    public Transform shootPoint;
    public Transform bearTarget;

    // BOSS BATTLE
    public bool canAttack = false;

    [Header("Turn System")]
    public int maxMonkeyThrow = 3;
    private int currentThrow = 0;
    private bool monkeyTurn = true;
    private int monkeyThrowCount = 0;
    private bool monkeyFinished = false;

    [Header("Bear Attack")]
    public GameObject bearProjectilePrefab;
    public Transform bearShootPoint;
    public Transform playerTarget;

    [Header("Data Static (Abadi Antar Stage)")]
    private static int currentHealth = 100;
    private static int jumlahKantongApel = 0; 

    [Header("Movement & Physics")]
    public float speed = 7f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float bounceUp = 13f;
    public float bounceSide = 9f;
    public float fallLimit = -12f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool isDead = false;
    private Vector3 ukuranAsli;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ukuranAsli = transform.localScale;

        // Beri jeda 1 frame saat ganti stage agar semua visual UI siap di-render terlebih dahulu
        StartCoroutine(JedaSinkronisasiUI());

        Debug.Log("Scene Loaded | Health: " + currentHealth + " | Jumlah Apel di Kantong: " + jumlahKantongApel);
    }

    System.Collections.IEnumerator JedaSinkronisasiUI()
    {
        yield return new WaitForEndOfFrame();
        UpdateUI();
        UpdateApelUI();
    }

    void Update()
    {
        if (isDead) return;

        if (transform.position.y < fallLimit)
            Mati();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.35f, whatIsGround);

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        if (moveInput > 0)
        {
            transform.localScale = ukuranAsli;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-ukuranAsli.x, ukuranAsli.y, ukuranAsli.z);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (canAttack && monkeyTurn && Input.GetKeyDown(KeyCode.F))
        {
            ShootBanana();
            currentThrow++;
            monkeyThrowCount++;

            if (monkeyThrowCount >= 3) monkeyFinished = true;

            if (currentThrow >= maxMonkeyThrow)
            {
                monkeyTurn = false;
                Invoke("BearTurn", 1f);
            }
        }
    }

    void ShootBanana()
    {
        if (bananaPrefab == null || shootPoint == null) return;

        GameObject banana = Instantiate(bananaPrefab, shootPoint.position, Quaternion.identity);
        BananaProjectile bp = banana.GetComponent<BananaProjectile>();

        if (bp != null && bearTarget != null)
        {
            bp.SetTarget(bearTarget);
        }
    }

    void BearTurn()
    {
        StartCoroutine(BearAttackRoutine());
    }

    System.Collections.IEnumerator BearAttackRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (bearProjectilePrefab != null && bearShootPoint != null)
        {
            GameObject projectile = Instantiate(bearProjectilePrefab, bearShootPoint.position, Quaternion.identity);
            BananaProjectile bp = projectile.GetComponent<BananaProjectile>();

            if (bp != null && playerTarget != null)
            {
                bp.SetTarget(playerTarget);
            }
        }

        yield return new WaitForSeconds(1.5f);
        currentThrow = 0;
        monkeyTurn = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // LOGIKA AMBIL APEL
        if (collision.CompareTag("HealthPowerUp"))
        {
            bool itemDiambil = false;

            if (currentHealth < 100)
            {
                currentHealth = Mathf.Min(currentHealth + 50, 100);
                UpdateUI();
                itemDiambil = true;
            }
            else if (jumlahKantongApel < 3)
            {
                jumlahKantongApel++; 
                UpdateApelUI();
                itemDiambil = true;
            }

            if (itemDiambil)
            {
                CollectItem(collision.gameObject);
            }
        }

        if (collision.CompareTag("Collectible"))
        {
            ScoreManager.AddScore(10);
            UpdateUI(); // Memperbarui teks skor
            CollectItem(collision.gameObject);
        }

        if (collision.CompareTag("spikes"))
        {
            TakeDamage(50);
        }

        if (collision.CompareTag("checkpoint"))
        {
            Animator flagAnim = collision.GetComponent<Animator>();
            if (flagAnim != null) flagAnim.SetTrigger("Raise");

            isDead = true;
            rb.linearVelocity = Vector2.zero;
            Invoke("NextLevel", 1.5f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (jumlahKantongApel > 0)
        {
            jumlahKantongApel--; 
            UpdateApelUI();
            ApplyBounce();
            return;
        }

        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            Mati();
        }
        else
        {
            ApplyBounce();
        }
    }

    void UpdateUI()
    {
        // PERBAIKAN: Diubah agar HANYA MENAMPILKAN ANGKA saja tanpa tulisan "SKOR: "
        if (skorText != null)
            skorText.text = ScoreManager.score.ToString();

        if (hatiImage != null)
        {
            hatiImage.sprite = (currentHealth <= 50) ? hatiHalf : hatiFull;
        }
    }

    void UpdateApelUI()
    {
        if (slotApelUI == null || slotApelUI.Length == 0) return;

        for (int i = 0; i < slotApelUI.Length; i++)
        {
            if (slotApelUI[i] != null)
            {
                slotApelUI[i].SetActive(i < jumlahKantongApel);
            }
        }
    }

    void CollectItem(GameObject item)
    {
        Collider2D col = item.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Animator a = item.GetComponent<Animator>();
        if (a != null) a.SetTrigger("diambil");

        Destroy(item, 0.5f);
    }

    void Mati()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Stage3" || currentSceneName == "Stage 3")
        {
            Debug.Log("Monyet mati di Stage 3! Membuka scene GameOver.");
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            Debug.Log("Monyet mati di " + currentSceneName + "! Mengulang stage (Respawn).");
            currentHealth = 100;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void ApplyBounce()
    {
        rb.linearVelocity = Vector2.zero;
        float arah = (transform.localScale.x > 0) ? -1f : 1f;
        rb.AddForce(new Vector2(arah * bounceSide, bounceUp), ForceMode2D.Impulse);
    }

    public bool IsMonkeyTurnFinished() { return monkeyFinished; }
    public void ResetMonkeyTurn() { monkeyThrowCount = 0; monkeyFinished = false; }
}