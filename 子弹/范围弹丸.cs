using UnityEngine;

public class 范围弹丸 : MonoBehaviour
{
    private Transform 攻击目标;
    private float 攻击力;
    public int 攻击音效;
    private string 敌人标签; // 添加敌人标签字段
    public float 伤害范围; // 范围弹丸自己的范围值
    public GameObject 爆炸特效预制体;

    // 初始化方法，增加敌人标签参数
    public void 初始化(Transform 目标, float 攻击力, string 敌人标签)
    {
        this.攻击目标 = 目标;
        this.攻击力 = 攻击力;
        this.敌人标签 = 敌人标签;
    }

    private void Update()
    {
        if (攻击目标 == null)
        {
            // 如果目标丢失，触发范围伤害
            触发范围伤害(transform.position);
            Destroy(gameObject);
            return;
        }

        // 移动弹丸朝向目标
        transform.position = Vector3.MoveTowards(transform.position, 攻击目标.position, Time.deltaTime * 10f);

        // 检查是否命中目标
        if (Vector3.Distance(transform.position, 攻击目标.position) < 0.1f)
        {
            触发范围伤害(transform.position);
            Destroy(gameObject);
        }
    }

    private void 触发范围伤害(Vector3 爆炸位置)
    {
        // 显示爆炸特效
        if (爆炸特效预制体 != null)
        {
            Instantiate(爆炸特效预制体, 爆炸位置, Quaternion.identity);
        }

        // 查找范围内的敌人
        Collider[] 命中目标 = Physics.OverlapSphere(爆炸位置, 伤害范围);
        foreach (Collider 碰撞体 in 命中目标)
        {
            基类 目标属性 = 碰撞体.GetComponent<基类>();

            // 检查是否为当前攻击目标或符合敌人标签
            if (目标属性 != null && 碰撞体.CompareTag(敌人标签))
            {
                float 实际伤害 = Mathf.Max(攻击力 - 目标属性.属性配置.防御, 0);
                目标属性.受伤(实际伤害);

                Debug.Log($"弹丸命中 {目标属性.name}，造成 {实际伤害} 点伤害！");

                // 显示伤害文本
                目标属性.显示伤害文本(实际伤害, 碰撞体.transform.position,敌人标签);

                // 如果目标生命值为0，触发死亡逻辑
                if (目标属性.属性配置.生命值 <= 0)
                {
                    //目标属性.死亡();
                }
            }
            else
            {
                Debug.Log($"弹丸忽略了 {碰撞体.name}，因为它不是目标标签的敌人。");
            }
        }
    }
}
