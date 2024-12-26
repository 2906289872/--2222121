using System.Collections.Generic;
using UnityEngine;

public class 游戏管理器 : MonoBehaviour
{
    public float 生成时间;
    public float 计时器;
    [Header("出生点与怪物")]
    public List<GameObject> 怪物组 = new List<GameObject>();
    public List<Transform> 生成的物体位置 = new List<Transform>();

    [Header("属性")]
    public List<属性配置>属性配置=new List<属性配置>();

    public List<int> 敌人索引列表;

    public Transform 父级;
    private void Update()
    {
        计时器 += Time.deltaTime;
        if (计时器 >= 生成时间)
        {
            计时器 = 0;
            foreach (var 出生点 in 生成的物体位置)
            {
                foreach (var 索引 in 敌人索引列表)
                {
                    GameObject 预制体 = 获取敌人预制体(索引);
                    if (预制体 != null)
                    {
                        GameObject 实例化物体 = Instantiate(预制体, 出生点.position, 出生点.rotation, 父级);
                        初始化敌人属性(实例化物体, 索引);
                    }
                }
            }

        }



    }
    private GameObject 获取敌人预制体(int 索引)
    {
        if (索引 >= 0 && 索引 < 怪物组.Count)
        {
            return 怪物组[索引];
        }
        else
        {
          
            return null;
        }
    }
    void 初始化敌人属性(GameObject 敌人,int 索引)
    {
        if (索引 >= 0 && 索引 < 属性配置.Count)
        {
            属性配置 属性 = 属性配置[索引];
                基类 脚本=敌人.GetComponent<基类>();
            if(脚本 != null)
            {
                脚本.初始化属性(属性);
            }
        }
    }
}
