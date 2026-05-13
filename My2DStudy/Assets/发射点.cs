using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 发射点 : MonoBehaviour
{
    public Transform 发射点1;
    public GameObject 针1;
    public Text 分数文本;
    public Text 失败文本;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (针脚本.生成) 
        {
            Instantiate(针1, 发射点1.position, 发射点1.rotation);
            针脚本.生成 = false;
        }
        分数文本.text = "分数：" + 针脚本.分数;
        if (针脚本.失败) 
        {
            失败文本.enabled = true;
        }
    }
}
