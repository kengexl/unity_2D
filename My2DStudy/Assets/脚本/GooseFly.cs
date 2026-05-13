using UnityEngine;

public class GooseFly : MonoBehaviour
{
    [Header("飞行区域")]
    public float leftLimit = -5f;   // 左边界
    public float rightLimit = 5f;  // 右边界
    public float topLimit = 2f;    // 上边界
    public float bottomLimit = -2f;// 下边界

    [Header("飞行设置")]
    public float moveSpeed = 2f;   // 水平速度
    public float floatSpeed = 0.5f;// 上下浮动速度
    public bool canFly = true;     // 总开关

    private int moveDir = -1;        // 1=右，-1=左
    private float originalY;        // 初始Y位置
    private Vector2 spawnPos;       // 鹅初始出生位置

    void Start()
    {
        spawnPos = transform.position;
        originalY = transform.position.y;
    }

    void Update()
    {
        if (!canFly) return;

        Fly();
        CheckBoundsReset();
    }

    void Fly()
    {
        // 水平移动
        transform.Translate(Vector2.right * moveDir * moveSpeed * Time.deltaTime);

        // 边界检测 掉头翻转
    

        // 上下浮动模拟飞行
        float newY = originalY + Mathf.Sin(Time.time * floatSpeed) * 0.3f;
        transform.position = new Vector2(transform.position.x, newY);
    }

    // 检测是否超出飞行区域，超出就重置回初始点
    void CheckBoundsReset()
    {
        Vector2 pos = transform.position;
        if (pos.x < leftLimit || pos.x > rightLimit ||
            pos.y < bottomLimit || pos.y > topLimit)
        {
            transform.position = spawnPos;
            moveDir = -1;
        }
    }

    // 翻转朝向
    
    // Scene窗口可视化飞行区域框（绿色线框）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        // 计算区域中心和大小
        float centerX = (leftLimit + rightLimit) / 2f;
        float centerY = (topLimit + bottomLimit) / 2f;
        float width = rightLimit - leftLimit;
        float height = topLimit - bottomLimit;

        Gizmos.DrawWireCube(new Vector2(centerX, centerY), new Vector2(width, height));
    }
}