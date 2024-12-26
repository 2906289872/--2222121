using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 全局游戏管理 : MonoBehaviour
{
    // 游戏货币
    public int 金币 = 0;
    public int 钻石 = 0;

    // 单例模式
    public static 全局游戏管理 实例;




   
    public List<string> 已选择兵种名称列表 = new List<string>();

    private void Awake()
    {
        // 确保只有一个实例
        if (实例 == null)
        {
            实例 = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
