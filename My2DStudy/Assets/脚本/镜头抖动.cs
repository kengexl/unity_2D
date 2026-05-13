using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 镜头抖动 : MonoBehaviour
{
    public float 震动强度 = 0.1f;
    public float 震动持续时间 = 0.1f;
    private float 震动时间 = 0;
    private Vector3 原始位置;

    void Start()
    {
        原始位置 = transform.localPosition;
    }

    void Update()
    {
        if (震动时间 > 0)
        {
            transform.localPosition = 原始位置 + Random.insideUnitSphere * 震动强度;
            震动时间 -= Time.unscaledDeltaTime;
        }
        else
        {
            transform.localPosition = 原始位置;
        }
    }

    public void 震动()
    {
        震动时间 = 震动持续时间;
    }
}
