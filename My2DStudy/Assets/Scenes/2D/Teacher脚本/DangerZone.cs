using UnityEngine;

public class DangerZone : MonoBehaviour
{
    // 必须在 Inspector 拖入你的 HatController 物体
    public HatController hatController;

    void OnTriggerEnter2D(Collider2D col)
    {
        // 先判断有没有赋值，再调用
        if (hatController != null)
        {
            hatController.OnDangerZoneEnter(col);
        }
        else
        {
            Debug.LogError("DangerZone 没有赋值 HatController！请在 Inspector 拖入帽子物体");
        }
    }
}