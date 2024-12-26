using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 伤害文本对象池 : MonoBehaviour
{
    // 对象池中存储的伤害文本对象
    private Queue<GameObject> 池子 = new Queue<GameObject>();

    // 伤害文本的预制体
    public GameObject 伤害文本预制体;

    // 最大对象池容量
    public int 最大容量 = 20;

    // 初始化对象池
    private void Start()
    {
        初始化对象池();
    }

    // 初始化对象池，创建预定数量的伤害文本
    private void 初始化对象池()
    {
        for (int i = 0; i < 最大容量; i++)
        {
            GameObject 新的伤害文本 = Instantiate(伤害文本预制体);
            新的伤害文本.SetActive(false);  // 初始状态为不可见
            池子.Enqueue(新的伤害文本);
        }
    }

    // 从池子中获取一个伤害文本对象
    public GameObject 获取伤害文本()
    {
        if (池子.Count > 0)
        {
            GameObject 伤害文本 = 池子.Dequeue();
            伤害文本.SetActive(true);  // 激活对象
            return 伤害文本;
        }
        else
        {
            // 如果池子为空，就创建一个新的
            GameObject 新的伤害文本 = Instantiate(伤害文本预制体);
            return 新的伤害文本;
        }
    }

    // 将伤害文本对象返回池中
    public void 回收伤害文本(GameObject 伤害文本)
    {
        伤害文本.SetActive(false);  // 隐藏对象
        池子.Enqueue(伤害文本);  // 回收到池子
    }
}
