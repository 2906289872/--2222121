using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class 关卡选择管理脚本 : MonoBehaviour
{
    public Button 按钮预制体; // 按钮预制体
    public Transform 父物体; // 父物体，用于存放实例化的按钮
    public int 初始关卡数量 = 20; // 初始关卡数量
    private List<Button> 按钮列表 = new List<Button>(); // 存储按钮实例的列表

    void Start()
    {
        初始化解锁状态();
        创建关卡按钮(初始关卡数量);
    }

    // 初始化解锁状态
    void 初始化解锁状态()
    {
        if (!PlayerPrefs.HasKey("解锁关卡"))
        {
            PlayerPrefs.SetInt("解锁关卡", 1); // 默认解锁第一关
            PlayerPrefs.Save();
        }
    }

    public void 创建关卡按钮(int 关卡数量)
    {
        // 清除现有按钮（如果需要重置）
        foreach (Button 按钮 in 按钮列表)
        {
            Destroy(按钮.gameObject);
        }
        按钮列表.Clear();

        int 解锁关卡 = PlayerPrefs.GetInt("解锁关卡");

        for (int i = 1; i <= 关卡数量; i++)
        {
            Button 新按钮 = Instantiate(按钮预制体, 父物体);
            新按钮.name = "关卡按钮_" + i; 
            Text 按钮文本 = 新按钮.GetComponentInChildren<Text>();

            if (i <= 解锁关卡)
            {
              
                按钮文本.text = "关卡 " + i;
                新按钮.GetComponent<Image>().color = Color.green; // 解锁颜色
                int 当前关卡索引 = i; // 捕获当前的关卡索引，避免闭包问题
                新按钮.onClick.AddListener(() => 选择关卡(当前关卡索引));
            }
            else
            {
                // 未解锁状态
                按钮文本.text = "未解锁";
                新按钮.GetComponent<Image>().color =new Color(255,255,0,255); // 未解锁颜色
                新按钮.interactable = false; // 禁用按钮
            }

            按钮列表.Add(新按钮);
        }
    }

    public void 解锁下一关(int 已完成关卡)
    {
        int 当前解锁关卡 = PlayerPrefs.GetInt("解锁关卡");
        if (已完成关卡 >= 当前解锁关卡)
        {
            PlayerPrefs.SetInt("解锁关卡", 已完成关卡 + 1);
            PlayerPrefs.Save();
        }
    }

    public void 选择关卡(int 关卡索引)
    {
        Debug.Log("选择了关卡: " + 关卡索引);
        SceneManager.LoadScene("关卡" + 关卡索引); // 假设每个关卡有一个对应的场景
    }

    public void 返回()
    {
        SceneManager.LoadScene("兵种场景");
    }
}
