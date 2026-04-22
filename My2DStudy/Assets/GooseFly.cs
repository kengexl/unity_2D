using UnityEngine;

public class GooseFly : MonoBehaviour
{
    [Header("飞行区域")]
    public float leftLimit = -5f;
    public float rightLimit = 5f;
    public float topLimit = 2f;
    public float bottomLimit = -2f;

    [Header("飞行设置")]
    public float moveSpeed = 2f;
    public float floatSpeed = 0.5f;
    public bool canFly = true;

    private int moveDir = 1;
    private float originalY;
    private Vector2 startPos; // 初始出生位置

    void Start()
    {
        startPos = transform.position; // 记录初始点
        originalY = startPos.y;
    }

    void Update()
    {
        if (!canFly) return;
        Fly();
        CheckOutOfArea();
    }

    void Fly()
    {
        // 水平移动
        transform.Translate(Vector2.right * moveDir * moveSpeed * Time.deltaTime);

        // 边界掉头
        if (transform.position.x > rightLimit)
        {
            moveDir = -1;
            Flip();
        }
        else if (transform.position.x < leftLimit)
        {
            moveDir = 1;
            Flip();
        }

        // 上下浮动
        float newY = originalY + Mathf.Sin(Time.time * floatSpeed) * 0.3f;
        transform.position = new Vector2(transform.position.x, newY);
    }

    // 超出范围 → 重置回初始位置
    void CheckOutOfArea()
    {
        Vector2 pos = transform.position;
        if (pos.x < leftLimit || pos.x > rightLimit || pos.y < bottomLimit || pos.y > topLimit)
        {
            ResetToStart();
        }
    }

    void ResetToStart()
    {
        transform.position = startPos;
        moveDir = 1;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}