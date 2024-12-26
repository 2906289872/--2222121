using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class 单位属性
{
    public string 名称;            // 单位名称 
    public string 攻击目标标签; // 要攻击的目标标签，例如 "红方" 或 "敌人" 
    public float 攻击力;          // 攻击力 
    public float 最大攻击力;      // 最大攻击力 
    public float 防御;            // 防御 
    public float 最大防御;        // 最大防御 
    public float 移动速度;        // 移动速度 
    public float 最大移动速度;    // 最大移动速度 
    public float 生命值;          // 当前生命值 
    public float 最大生命值;      // 最大生命值 
    public float 魔法值;          // 当前魔法值 
    public float 最大魔法值;      // 最大魔法值 
    public float 回复血量速度;    // 每秒回复生命值 
    public float 最大回复血量速度;// 最大生命回复速度 
    public float 回复魔法速度;    // 每秒回复魔法值 
    public float 最大回复魔法速度;// 最大魔法回复速度 
    public float 攻击距离;        // 攻击距离 
    public float 最大攻击距离;    // 最大攻击距离 
    public float 攻击速度;        // 攻击速度 
    public float 最大攻击速度;    // 最大攻击速度 
    public float 攻击范围;        // 攻击范围（用于特殊攻击） 
    public float 最大攻击范围;    // 最大攻击范围 
    public 单位属性(string 名称, float 攻击力, float 最大攻击力, float 防御, float 最大防御,
        float 移动速度, float 最大移动速度, float 生命值, float 最大生命值, float 魔法值,
        float 最大魔法值, float 回复血量速度, float 最大回复血量速度, float 回复魔法速度,
        float 最大回复魔法速度, float 攻击距离, float 最大攻击距离, float 攻击速度,
        float 最大攻击速度, float 攻击范围 = 0, float 最大攻击范围 = 0)
    {
        this.名称 = 名称;
        this.攻击力 = Mathf.Min(攻击力, 最大攻击力);
        this.最大攻击力 = 最大攻击力;
        this.防御 = Mathf.Min(防御, 最大防御);
        this.最大防御 = 最大防御;
        this.移动速度 = Mathf.Min(移动速度, 最大移动速度);
        this.最大移动速度 = 最大移动速度;
        this.生命值 = Mathf.Min(生命值, 最大生命值);
        this.最大生命值 = 最大生命值;
        this.魔法值 = Mathf.Min(魔法值, 最大魔法值);
        this.最大魔法值 = 最大魔法值;
        this.回复血量速度 = Mathf.Min(回复血量速度, 最大回复血量速度);
        this.最大回复血量速度 = 最大回复血量速度;
        this.回复魔法速度 = Mathf.Min(回复魔法速度, 最大回复魔法速度);
        this.最大回复魔法速度 = 最大回复魔法速度;
        this.攻击距离 = Mathf.Min(攻击距离, 最大攻击距离);
        this.最大攻击距离 = 最大攻击距离;
        this.攻击速度 = Mathf.Min(攻击速度, 最大攻击速度);
        this.最大攻击速度 = 最大攻击速度;
        this.攻击范围 = Mathf.Min(攻击范围, 最大攻击范围);
        this.最大攻击范围 = 最大攻击范围;
    }

}

public abstract class 单位 : MonoBehaviour
{
    public 单位属性 属性;       // 单位属性 
    public Animator 动画控制器; // Animator，用于控制动画 
    public Transform 当前目标;   // 当前目标 
    public float 搜索范围 = 10f; // 搜索敌人的范围 
    protected Vector3 移动目标位置; // 用于存储移动目标位置
    protected NavMeshAgent 导航代理; // 导航组件 

    protected float 下一次攻击时间 = 0f; // 控制攻击间隔的计时器 
    protected float 攻击失败持续时间 = 0f; // 攻击失败的累计时间 
    protected const float 攻击失败最大时间 = 0.1f;

    public Image 血条;

    protected virtual void Start()
    {
        if (属性 == null || 动画控制器 == null)
        {
            Debug.LogError("属性或动画控制器未设置！");
            return;
        }

        导航代理 = GetComponent<NavMeshAgent>();
        if (导航代理 == null)
        {
            Debug.LogError("缺少 NavMeshAgent 组件！");
            return;
        }

        导航代理.speed = 属性.移动速度;
        导航代理.stoppingDistance = 属性.攻击距离;
        初始化(属性, 动画控制器);
    }

    protected virtual void Update()
    {
        动画控制器.speed = 属性.攻击速度;
        自动寻找并攻击目标();
        自动回复();
    }

    public void 初始化(单位属性 属性, Animator 动画控制器)
    {
        this.属性 = 属性;
        this.动画控制器 = 动画控制器;

        // 将所有属性设置为最大值

        属性.攻击力 = 属性.最大攻击力;
        属性.防御 = 属性.最大防御;
        属性.移动速度 = 属性.最大移动速度;
        属性.生命值 = 属性.最大生命值;
        属性.魔法值 = 属性.最大魔法值;
        属性.回复血量速度 = 属性.最大回复血量速度;
        属性.回复魔法速度 = 属性.最大回复魔法速度;
        属性.攻击距离 = 属性.最大攻击距离;
        属性.攻击速度 = 属性.最大攻击速度;
        属性.攻击范围 = 属性.最大攻击范围;

        // 如果有导航代理（NavMeshAgent），设置移动速度
        if (导航代理 != null)
        {
            导航代理.speed = 属性.移动速度;
        }
        血条更新();
    }


    public void 自动回复()
    {
        属性.生命值 = Mathf.Min(属性.生命值 + 属性.回复血量速度 * Time.deltaTime, 属性.最大生命值);
        属性.魔法值 = Mathf.Min(属性.魔法值 + 属性.回复魔法速度 * Time.deltaTime, 属性.最大魔法值);
        血条.fillAmount = 属性.生命值 / 属性.最大生命值;
    }

    protected virtual void 自动寻找并攻击目标()
    {
        if (当前目标 != null)
        {
            // 计算目标与单位之间的距离
            float 距离 = Vector3.Distance(transform.position, 当前目标.position);

            // 如果目标在攻击范围内，进行攻击
            if (距离 <= 属性.攻击距离)
            {
                攻击失败持续时间 = 0f; // 重置攻击失败时间 
                if (Time.time >= 下一次攻击时间)
                {
                    攻击(当前目标.gameObject);
                    下一次攻击时间 = Time.time + 1f / 属性.攻击速度;
                }
            }
            else
            {
                // 如果目标超出了攻击距离，停止攻击并重新寻找目标
                攻击失败持续时间 += Time.deltaTime;

                // 如果攻击失败的时间超过最大限制，重新寻找目标
                if (攻击失败持续时间 > 攻击失败最大时间)
                {
                    Debug.Log($"{属性.名称} 攻击不到目标，正在寻找新目标...");
                    当前目标 = null; // 重置当前目标 
                    攻击失败持续时间 = 0f; // 重置计时器 
                    寻找敌人(属性.攻击目标标签); // 重新寻找目标 
                }
                else
                {
                    // 如果目标还在攻击范围外，继续移动
                    移动(当前目标.position);
                }
            }
        }
        else
        {
            寻找敌人(属性.攻击目标标签); // 如果没有目标，寻找新的目标 
            if (当前目标 != null)
            {
                // 重新找到目标后，开始移动
                移动(当前目标.position);
            }
        }

    }

    protected virtual void 寻找敌人(string 敌方标签)
    {
        Collider[] 敌人们 = Physics.OverlapSphere(transform.position, 搜索范围);
        Transform 最近敌人 = null;
        float 最小距离 = float.MaxValue;

        foreach (var 敌人 in 敌人们)
        {
            if (敌人.CompareTag(敌方标签))
            {
                float 距离 = Vector3.Distance(transform.position, 敌人.transform.position);
                if (距离 < 最小距离)
                {
                    最小距离 = 距离;
                    最近敌人 = 敌人.transform;
                }
            }
        }

        if (最近敌人 != null)
        {
            当前目标 = 最近敌人;
        }
    }

    public virtual void 攻击(GameObject 目标)
    {
        if (目标 == null) return;
        导航代理.ResetPath(); // 清除当前的路径，停止导航
        导航代理.speed = 0f; // 停止移动
        导航代理.updatePosition = false; // 禁止位置更新  导航代理.ResetPath();

        Vector3 目标方向 = 目标.transform.position - transform.position;
        目标方向.y = 0;
        transform.rotation = Quaternion.LookRotation(目标方向); // 面向目标
        if (动画控制器 != null)
        {
            动画控制器.SetBool("攻击", true);
           
        }
        单位 敌方单位 = 目标.GetComponent<单位>();
        if (敌方单位 != null)
        {
            float 伤害 = Mathf.Max(属性.攻击力 - 敌方单位.属性.防御, 0);
            敌方单位.受伤(伤害, 属性);
        }
    }

    public void 血条更新()
    {


        血条.fillAmount = 属性.生命值 / 属性.最大生命值;
    }
    public virtual void 受伤(float 伤害值, 单位属性 攻击者属性)
    {
        属性.生命值 -= 伤害值;
        血条更新();
        if (攻击者属性 != null)
        {
            Debug.Log($"{属性.名称} 被 {攻击者属性.名称} 攻击，造成 {伤害值} 点伤害！");
        }
        else
        {
            Debug.Log($"{属性.名称} 受到了 {伤害值} 点伤害！");
        }
        if (属性.生命值 <= 0) 死亡(攻击者属性);
    }

    public virtual void 死亡(单位属性 击者属性)
    {
        Debug.Log($"{属性.名称} 已死亡！");
        Destroy(gameObject);
    }

    public virtual void 移动(Vector3 目标位置)
    {
        动画控制器.SetBool("攻击", false);
        导航代理.updatePosition = true; // 禁止位置更新  导航代理.ResetPath();
        if (导航代理 != null)
        {
            移动目标位置 = 目标位置; // 更新移动目标位置
            导航代理.speed = 属性.移动速度;
            导航代理.SetDestination(目标位置);
        }
    }
}
