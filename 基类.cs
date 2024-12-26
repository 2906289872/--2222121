using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class 属性配置
{
    [Header("角色属性")]
    public float 攻击力;
    public float 最大攻击力;
    public float 防御;
    public float 最大防御;
    public float 移动速度;
    public float 最大移动速度;
    public float 生命值;
    public float 最大生命值;
    public float 魔法值;
    public float 最大魔法值;
    public float 回复血量速度;
    public float 最大回复血量速度;
    public float 回复魔法速度;
    public float 最大回复魔法速度;
    public float 攻击距离;
    public float 最大攻击距离;
    public float 攻击速度;
    public float 最大攻击速度;

    public 属性配置(float 攻击力, float 最大攻击力, float 防御, float 最大防御, float 移动速度, float 最大移动速度, float 生命值, float 最大生命值, 
        float 魔法值, float 最大魔法值, float 回复血量速度, float 最大回复血量速度, float 回复魔法速度, float 最大回复魔法速度, float 攻击距离,
        float 最大攻击距离, float 攻击速度, float 最大攻击速度)
    {
        this.攻击力 = 攻击力;
        this.最大攻击力 = 最大攻击力;
        this.防御 = 防御;
        this.最大防御 = 最大防御;
        this.移动速度 = 移动速度;
        this.最大移动速度 = 最大移动速度;
        this.生命值 = 生命值;
        this.最大生命值 = 最大生命值;
        this.魔法值 = 魔法值;
        this.最大魔法值 = 最大魔法值;
        this.回复血量速度 = 回复血量速度;
        this.最大回复血量速度 = 最大回复血量速度;
        this.回复魔法速度 = 回复魔法速度;
        this.最大回复魔法速度 = 最大回复魔法速度;
        this.攻击距离 = 攻击距离;
        this.最大攻击距离 = 最大攻击距离;
        this.攻击速度 = 攻击速度;
        this.最大攻击速度 = 最大攻击速度;
    }
}


[System.Serializable]
public abstract class 基类 : MonoBehaviour
{
    public 属性配置 属性配置; // 在 Inspector 面板中设置

    public Image 血条;
   
    public GameObject 伤害文本预制体; // 用于指定伤害文本的预制体
    public Transform 文本父物体;    // 用于指定文本的父物体
    public int 攻击音效;


    [Header("标签设置")]
    public string 敌人标签;

    [Header("动画与导航")]
    public Animator 动画控制器;
    private NavMeshAgent 导航代理;

   
    protected GameObject 当前目标;      // 当前攻击目标
    private float 上次攻击时间;       // 上次攻击时间
    public float 攻击间隔 = 1.0f;     // 攻击间隔（秒）

    protected virtual void Start()
    {
        初始化属性(属性配置);
        初始化组件();
        更新血条();
    }

    protected virtual void Update()
    {
        更新目标();
        执行战斗逻辑();
    }

    // 初始化方法，通过怪物属性配置设置属性
    public void 初始化属性(属性配置 属性)
    { this.属性配置=属性;

        属性.攻击力 = 属性.最大攻击力;
        属性.攻击距离 = 属性.最大攻击距离;
        属性.攻击速度 = 属性.最大攻击速度;
        属性.防御 = 属性.最大防御;
        属性.生命值 = 属性.最大生命值;
        属性.魔法值 = 属性.最大魔法值;
        属性.回复血量速度 = 属性.最大回复血量速度;
        属性.回复魔法速度 = 属性.最大回复魔法速度;
        属性.移动速度 = 属性.最大移动速度;
    }


    private void 初始化组件()
    {
        动画控制器 = GetComponent<Animator>();
        导航代理 = GetComponent<NavMeshAgent>();
        if (导航代理 != null)
        {
            导航代理.speed =属性配置.移动速度;
        }
    }

    public void 更新血条()
    {
        血条.fillAmount=属性配置.生命值/属性配置.最大生命值;
    }
    private void 更新目标()
    {
        // 如果当前目标为空或者目标已死亡，寻找最近的敌人
        if (当前目标 == null || 当前目标.GetComponent<基类>().属性配置.生命值 <= 0)
        {
            当前目标 = 寻找最近的敌人();
        }

        // 如果目标超出攻击距离，也重新选择最近的目标
        if (当前目标 != null)
        {
            float 距离 = Vector3.Distance(transform.position, 当前目标.transform.position);
            if (距离 > 属性配置.攻击距离)
            {
                当前目标 = 寻找最近的敌人();
            }
        }
    }


    private void 执行战斗逻辑()
    {
        if (当前目标 == null) return;
       
        float 距离 = Vector3.Distance(transform.position, 当前目标.transform.position);

        if (距离 <= 属性配置.攻击距离)
        {
            停止移动并攻击();
        }
        else
        {
            移动到目标();
        }
    }

    private GameObject 寻找最近的敌人()
    {
        GameObject[] 敌人列表 = GameObject.FindGameObjectsWithTag(敌人标签);
        GameObject 最近的敌人 = null;
        float 最小距离 = Mathf.Infinity;

        foreach (GameObject 敌人 in 敌人列表)
        {
            float 距离 = Vector3.Distance(transform.position, 敌人.transform.position);
            基类 敌人属性 = 敌人.GetComponent<基类>();

            // 只考虑存活的敌人
            if (距离 < 最小距离 && 敌人属性 != null && 敌人属性.属性配置.生命值 > 0)
            {
                最小距离 = 距离;
                最近的敌人 = 敌人;
            }
        }

        return 最近的敌人;
    }


    private void 停止移动并攻击()
    {
        if (导航代理 != null) 导航代理.isStopped = true;
        AnimatorStateInfo 当前状态 = 动画控制器.GetCurrentAnimatorStateInfo(0);
        if (当前目标 != null)
        {
            // 使角色面朝目标
            Vector3 方向 = 当前目标.transform.position - transform.position;
            方向.y = 0; // 确保仅在水平平面上旋转
            transform.rotation = Quaternion.LookRotation(方向);
        }

        播放动画("攻击", true);
        动态调整动画速度();

    }

    private void 移动到目标()
    {
        if (导航代理 != null)
        {
            导航代理.isStopped = false;
            导航代理.SetDestination(当前目标.transform.position);
            Vector3 方向 = 当前目标.transform.position - transform.position;
            方向.y = 0; // 确保仅在水平平面上旋转
            if (方向.magnitude > 0.1f) // 避免过小的旋转抖动
            {
                transform.rotation = Quaternion.LookRotation(方向);
            }
        }
       
        播放动画("移动", true);
        动态调整动画速度();
    }

    private void 播放动画(string 动作名, bool 状态 = false)
    {
        if (动画控制器 == null) return;
        // 确保所有动画互相排斥
        动画控制器.SetBool("移动", false);
        动画控制器.SetBool("攻击", false);
        // 激活指定的动画
        动画控制器.SetBool(动作名, 状态);
    }

    // 动画事件触发的伤害处理逻辑
    protected virtual void 触发伤害()
    {
       
        if (当前目标 != null)
        {
         
            基类 敌人属性 = 当前目标.GetComponent<基类>();
            if (敌人属性 != null)
            {
                float 实际伤害 = Mathf.Max(属性配置.攻击力 - 敌人属性.属性配置.防御, 0);
                受伤(实际伤害);
            }
        }
    }


    protected virtual void 死亡()
    {
        Destroy(gameObject);
    }
  public  void 受伤(float 伤害值)
    {
        属性配置.生命值 -= 伤害值; 
        更新血条();

        string 目标标签 = 当前目标.tag;
        // 创建伤害文本
        显示伤害文本(伤害值, 当前目标.transform.position, 目标标签);

        if (属性配置.生命值 < 0)
        {
            死亡();
        }
      
    }


    private void 动态调整动画速度()
    {
        if (动画控制器 == null) return;

        // 获取当前动画状态信息
        AnimatorStateInfo 当前状态 = 动画控制器.GetCurrentAnimatorStateInfo(0);

        // 判断当前动画，并根据名字或标签设置速度
        if (当前状态.IsName("攻击动画"))
        {
            动画控制器.speed = 属性配置.攻击速度; // 根据攻击速度调整动画速度
        }
        else if (当前状态.IsName("移动动画"))
        {
            动画控制器.speed = 属性配置.移动速度; // 按比例调整移动动画速度
        }
        else
        {
            动画控制器.speed = 1.0f; // 其他动画恢复默认速度
        }
    }
    public void 显示伤害文本(float 实际伤害, Vector3 目标位置,string  标签) 
    {
        if (伤害文本预制体 == null || 文本父物体 == null)
        {
            Debug.LogWarning("伤害文本预制体或文本父物体未设置！");
            return;
        }

        // 实例化预制体 
        GameObject 伤害文本实例 = Instantiate(伤害文本预制体, 文本父物体);

        // 设置文本内容 
        Text 伤害文本组件 = 伤害文本实例.GetComponent<Text>();
        if (伤害文本组件 != null)
        {
            伤害文本组件.text = 实际伤害.ToString();
            // 根据传入的标签设置文本颜色 
            if (标签 == "敌人")
            {
                伤害文本组件.color = Color.red; // 敌人伤害为红色 
            }
            else if (标签 == "队友")
            {
                伤害文本组件.color = new Color(250f, 250, 0,250);
            }
            else
            {
                伤害文本组件.color = Color.white; // 默认颜色 
            }
        
    }

        // 设置初始位置 
        伤害文本实例.transform.position = 目标位置 + Vector3.up * 1.0f; // 根据需要调整偏移 

        // 如果预制体中有伤害显示脚本，可以直接使用它 
        伤害显示 伤害脚本 = 伤害文本实例.GetComponent<伤害显示>();
        伤害脚本.初始化(目标位置);
    }

}
