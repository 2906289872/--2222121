using UnityEngine;

public class 远程单位 : 基类
{
    public GameObject 弹丸预制体; // 发射的预制体
    public Transform 发射点;    // 发射点的变换

    protected override void 触发伤害()
    {
        if (当前目标 == null || 弹丸预制体 == null || 发射点 == null)
        {
            Debug.LogWarning("目标、弹丸预制体或发射点未设置！");
            return;
        }

        // 创建弹丸
        GameObject 弹丸实例 = Instantiate(弹丸预制体, 发射点.position, Quaternion.identity);
     
        范围弹丸 弹丸脚本 = 弹丸实例.GetComponent<范围弹丸>();
        if (弹丸脚本 != null)
        {
            弹丸脚本.初始化(当前目标.transform, 属性配置.攻击力, 敌人标签); // 传递目标、攻击力和敌人标签
        }
    }
}
