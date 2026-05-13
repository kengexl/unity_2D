using UnityEngine;

public class 针脚本 : MonoBehaviour
{
    public Rigidbody2D 刚体;
    public float 针速度 = 10f;
    public bool 已吸附 = false;
    public float 半径 = 1;
    public float 旋转速度 = 100f;
    public Transform 旋转点;
    public Animator 动画;

    void Start()
    {
        刚体 = GetComponent<Rigidbody2D>();
        旋转点 = GameObject.Find("球").GetComponent<Transform>();
        动画 = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !已吸附)
        {
            刚体.velocity = new Vector2(0, 针速度);
        }

        if (已吸附)
        {
            transform.RotateAround(旋转点.position, Vector3.forward, -旋转速度 * Time.deltaTime);
            Vector3 dir = (transform.position - 旋转点.position).normalized;
            transform.position = 旋转点.position + dir * 半径;
            动画.speed = 0;
           
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (主管理脚本.实例 == null)
            return;

        镜头抖动 镜头 = FindObjectOfType<镜头抖动>();
        if (镜头 != null)
            镜头.震动();

        主管理脚本.实例.击中特效播放();

        if (other.gameObject.CompareTag("针"))
        {
            主管理脚本.实例.针碰撞失败();
        }

        if (other.gameObject.CompareTag("球"))
        {
            Debug.Log("碰到球了");
            刚体.velocity = new Vector2(0, 0);
            已吸附 = true;
            主管理脚本.实例.针已吸附();
        }

        Debug.Log("碰到了,加分以及停下来");
    }
}
