using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class 针脚本 : MonoBehaviour
{
    public Rigidbody2D 刚体;//写一个变量获取刚体组件
    public float 针速度 = 10f;
    public bool 已吸附 = false;
    public static bool 生成 = true;
    public float 半径 = 1;
    public float 旋转速度 = 100f;
    public Transform 旋转点 ;
    public static int 分数 = 0;
    public static bool 失败 = false;
    
    // Start is called before the first frame update
    void Start()
    {
        刚体 = GetComponent<Rigidbody2D>();
        旋转点 = GameObject.Find("球").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)&&!已吸附) //鼠标左键事件，0代表鼠标左键
        {
            //发射
            刚体.velocity = new Vector2(0, 针速度);
            
        }
        if (已吸附) 
        {
            transform.RotateAround(旋转点.position, Vector3.forward, -旋转速度 * Time.deltaTime);
            Vector3 dir = (transform.position - 旋转点.position).normalized;
            transform.position = 旋转点.position + dir * 半径;
        }





    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("针")) 
        {
            Time.timeScale = 0;
            Debug.Log("碰到针了");
            失败 = true;
        }

        if (other.gameObject.CompareTag("球"))
        {
            Debug.Log("碰到球了");
            刚体.velocity = new Vector2(0, 0);
            已吸附 = true;
            生成 = true;
            分数++;

        }







        Debug.Log("碰到了,加分以及停下来");
    }
}
