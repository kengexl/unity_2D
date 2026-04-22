using UnityEngine;
using TMPro;

public class HatController : MonoBehaviour
{
    [Header("特效")]
    public GameObject effect;

    [Header("分数设置")]
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public int addScore = 10;
    public int winScore = 100;

    [Header("生命设置")]
    public TextMeshProUGUI lifeText;
    public int currentLife = 5;
    public int maxLife = 5;
    public int loseLife = 1;
    public int getLife = 1;

    private Vector3 rawPosition;
    private Vector3 hatPosition;
    private float maxWidth;
    private Rigidbody2D rb;
    private bool gameStopped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hatPosition = transform.position;

        Vector3 screenPos = new Vector3(Screen.width, 0, 0);
        Vector3 moveWidth = Camera.main.ScreenToWorldPoint(screenPos);
        float hatWidth = GetComponent<Renderer>().bounds.extents.x;
        maxWidth = moveWidth.x - hatWidth;

        UpdateUI();
    }

    void FixedUpdate()
    {
        if (gameStopped) return;

        rawPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hatPosition = new Vector3(rawPosition.x, hatPosition.y, 0);
        hatPosition.x = Mathf.Clamp(hatPosition.x, -maxWidth, maxWidth);
        rb.MovePosition(hatPosition);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (gameStopped) return;

        if (col.CompareTag("Ball"))
        {
            // ==== 修复特效看不见关键代码 ====
            if(effect != null)
            {
                // 不挂在帽子身上，防止遮挡/错位
                GameObject neweffect = Instantiate(effect, transform.position, effect.transform.rotation);
                neweffect.transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
                Destroy(neweffect, 1f);
            }

            score += addScore;
            currentLife = Mathf.Min(currentLife + getLife, maxLife);
            Destroy(col.gameObject);

            UpdateUI();
            CheckWin();
        }
    }

    public void OnDangerZoneEnter(Collider2D col)
    {
        if (gameStopped) return;

        if (col.CompareTag("Ball"))
        {
            currentLife = Mathf.Max(0, currentLife - loseLife);
            Destroy(col.gameObject);
            UpdateUI();
            CheckLose();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (lifeText != null) lifeText.text = "Life: " + currentLife;
    }

    void CheckWin()
    {
        if (score >= winScore)
        {
            if(scoreText != null) scoreText.text = "You Win";
            StopEntireGame();
        }
    }

    void CheckLose()
    {
        if (currentLife <= 0)
        {
            if(lifeText != null) lifeText.text = "Game Over";
            StopEntireGame();
        }
    }

    void StopEntireGame()
    {
        gameStopped = true;
        Time.timeScale = 0;
        enabled = false;
    }
}