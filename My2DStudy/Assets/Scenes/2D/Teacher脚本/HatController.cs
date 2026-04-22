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
            // ========== 特效跟着帽子走 ==========
            if (effect != null)
            {
                GameObject neweffect = Instantiate(effect, transform.position, effect.transform.rotation);
                
                // 让特效成为帽子的子物体 → 跟着走
                neweffect.transform.SetParent(transform);
                
                // 强制 Z 轴靠前，保证看得见
                neweffect.transform.localPosition = new Vector3(0, 0, -1);
                
                // 1秒后销毁
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