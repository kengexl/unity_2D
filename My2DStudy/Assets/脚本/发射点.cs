using UnityEngine;

public class 发射点 : MonoBehaviour
{
    public Transform 发射点1;
    public GameObject 针1;

    void Start()
    {
        if (主管理脚本.实例 != null)
        {
            主管理脚本.实例.注册发射配置(发射点1, 针1);
        }
    }
}
