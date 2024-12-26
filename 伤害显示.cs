using UnityEngine;
using UnityEngine.UI;

public class 伤害显示 : MonoBehaviour
{
    public Text 伤害文本;          // 使用Text组件
    public float 飘动速度 = 1.0f; // 文本上升的速度
    public float 显示时长 = 1.0f; // 文本显示的时长
    public float 偏移X = 0.0f;    // X轴偏移
    public float 偏移Y = 0.5f;    // Y轴偏移
    public float 偏移Z = 0.0f;    // Z轴偏移

    private Vector3 随机方向;      // 随机的飘动方向
    private float 计时器 = 0.0f;   // 用于跟踪时间

    public void 初始化(Vector3 目标位置)
    {
        // 设置初始位置并加上偏移值
        transform.position = 目标位置 + new Vector3(偏移X, 偏移Y, 偏移Z);
    }

    void Start()
    {
        // 随机设置飘动方向（稍微偏离垂直方向）
        随机方向 = new Vector3(Random.Range(-0.5f, 0.5f), 1, 0).normalized;

        // 设置文本的随机缩放
        float 随机缩放 = Random.Range(0.8f, 1.2f);
        transform.localScale = Vector3.one * 随机缩放;

        // 在设定的时间后销毁对象
        Destroy(gameObject, 显示时长);
    }

    void Update()
    {
        // 控制文本随机方向上升
        transform.position += 随机方向 * 飘动速度 * Time.deltaTime;

        // 颜色渐变（透明度逐渐降低）
        计时器 += Time.deltaTime;
        float 剩余时间比例 = 1.0f - (计时器 / 显示时长);

        if (伤害文本 != null)
        {
            Color 颜色 = 伤害文本.color;
            颜色.a = 剩余时间比例;
            伤害文本.color = 颜色;
        }
    }
  
}
