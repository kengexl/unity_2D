using UnityEngine;
using UnityEngine.UI;

public class 主管理脚本 : MonoBehaviour
{
    public static 主管理脚本 实例 { get; private set; }

    [Header("UI 引用")]
    public Text 分数文本;
    public Text 失败文本;
    //背景颜色变化
    [Header("背景颜色变化")]
    public GameObject 背景颜色;
    public float 变化速度 = 0.1f;
    //针处理
    private Transform 发射点位置;
    private GameObject 针预制体;
    //分数处理
    private int 分数 = 0;
    private int 新分数 = 0;
    private bool 是否失败 = false;
    private bool 允许生成 = true;
    //背景颜色处理
    private SpriteRenderer 背景颜色渲染器_颜色控制;
    private float 背景颜色变化进度 = 0f;
    private Color 粉色;
    private Color 浅绿色;
    //击中特效
    public Transform 击中特效播放位置;
    public GameObject 击中特效预制体;

    void Awake()
    {
        实例 = this;
    }

    void Start()
    {
        if (背景颜色 != null)
        {
            背景颜色渲染器_颜色控制 = 背景颜色.GetComponent<SpriteRenderer>();
        }

        粉色 = GetHexColor("#FF9AC8");
        浅绿色 = GetHexColor("#96E8A8");
    }

    void Update()
    {
        if (允许生成 && !是否失败 && 针预制体 != null && 发射点位置 != null)
        {
            Instantiate(针预制体, 发射点位置.position, 发射点位置.rotation);
            允许生成 = false;
        }

        if (分数文本 != null)
            分数文本.text = "分数：" + 分数;

        if (失败文本 != null)
            失败文本.enabled = 是否失败;

        if (分数 != 新分数 && 背景颜色渲染器_颜色控制 != null)
        {
            新分数 = 分数;
            背景颜色变化进度 = Mathf.Min(背景颜色变化进度 + 变化速度, 1f);
            背景颜色渲染器_颜色控制.color = Color.Lerp(粉色, 浅绿色, 背景颜色变化进度);
        }
    }
    /*
     * 调试代码清理说明
     * =================
     * 
     * 【为何移除】
     *   此前 `击中特效播放()` 中包含大量 #if DEBUG_ENABLE || UNITY_EDITOR
     *   条件编译的 Debug.Log 日志，用于诊断粒子不显示问题。问题定位后，
     *   这些日志已完成使命，长期保留会导致：
     *     - 代码膨胀，核心逻辑被日志淹没，可读性下降
     *     - 条件编译块增加维护时的心智负担
     *     - 无实际作用的字符串插值带来不必要的内存分配
     * 
     * 【移除范围】
     *   仅移除了条件编译包裹的 Debug.Log / Debug.LogWarning 输出语句，
     *   保留了 try-catch 中的 Debug.LogError —— 这是捕获意外异常的
     *   生产级错误报告，不属于调试代码。
     * 
     * 【保留的错误处理原则】
     *   只有真正不可预期的异常路径（资源加载失败、组件丢失）才会触发
     *   LogError，这是程序健壮性的一部分，不应禁用。
     * 
     * 【后续需要调试时】
     *   在需要定位问题的方法入口添加 Debug.Log 即可，避免使用条件编译
     *   前缀，保持代码简洁。Unity 编辑器会自动显示日志，发布构建时会
     *   自动剥离 Debug.Log 调用（在 Build Settings 中勾选 Strip Debug 
     *   Code 即可）。
     */

    public void 击中特效播放()
    {
        if (击中特效预制体 == null || 击中特效播放位置 == null)
            return;

        GameObject go;
        try
        {
            go = Instantiate(
                击中特效预制体,
                击中特效播放位置.position,
                击中特效播放位置.rotation
            );
        }
        catch (System.Exception e)
        {
            Debug.LogError($"击中特效实例化失败: {e.Message}");
            return;
        }

        ParticleSystem ps;
        try
        {
            ps = go.GetComponent<ParticleSystem>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"获取 ParticleSystem 组件失败: {e.Message}");
            Destroy(go, 1f);
            return;
        }

        if (ps != null)
        {
            if (!ps.main.playOnAwake)
                ps.Play();

            float 生命周期 = Mathf.Max(ps.main.duration, ps.main.startLifetime.constantMax);
            float 销毁延迟 = 生命周期 > 0f ? 生命周期 + 0.5f : 1f;

            StartCoroutine(延迟销毁(go, 销毁延迟));
        }
        else
        {
            Destroy(go, 1f);
        }
    }

    /* 使用 unscaledTime 等待指定秒数后销毁对象，不受 Time.timeScale = 0 影响 */
    System.Collections.IEnumerator 延迟销毁(GameObject go, float 延迟秒数)
    {
        float 开始时间 = Time.unscaledTime;
        while (Time.unscaledTime - 开始时间 < 延迟秒数)
            yield return null;

        if (go != null)
            Destroy(go);
    }

    static Color GetHexColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        Debug.LogWarning("颜色解析失败：" + hex);
        return Color.white;
    }

    public void 注册发射配置(Transform 位置, GameObject 预制体)
    {
        发射点位置 = 位置;
        针预制体 = 预制体;
    }

    public void 针已吸附()
    {
        分数++;
        允许生成 = true;
    }

    public void 针碰撞失败()
    {
        是否失败 = true;
        Time.timeScale = 0;
        Debug.Log("碰到针了");
    }

    public int 获取分数()
    {
        return 分数;
    }

    public bool 是否游戏结束()
    {
        return 是否失败;
    }
}
