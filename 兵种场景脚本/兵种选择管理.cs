using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 兵种选择管理 : MonoBehaviour
{
    [System.Serializable]
    public class 兵种信息
    {
        public int 消耗碎片;
        public string 名称;
        public Sprite 图片;
        public string 描述;
        public GameObject 模型;
    }

    public List<兵种信息> 兵种列表 = new List<兵种信息>();
    private List<GameObject> 可移除按钮列表 = new List<GameObject>();

    public GameObject 按钮预制体;
    public Transform 按钮父物体;
    public Transform 描述父物体;
    public GameObject 描述预制体;
    public Transform 已选择的父物体;

    private int 当前选择索引 = 0;
    private GameObject 当前被选中的按钮;
    private GameObject 当前模型;
    private GameObject 当前描述文本;

    public Text 当前能量文本;
    public int 当前能量 = 5;
    public GameObject 提示框;

    public Text 显示属性文本;


    private void Start()
    {
        提示框.SetActive(false);
        生成所有兵种按钮();
        显示默认兵种();
        更新能量文本();
        从本地存储加载();
    }

 
    private void 保存到本地存储()
    {
        List<string> 已选择兵种名称列表 = new List<string>();
        foreach (var 按钮 in 可移除按钮列表)
        {
            var 名称文本 = 按钮.transform.Find("名称").GetComponent<Text>();
            if (名称文本 != null)
            {
                已选择兵种名称列表.Add(名称文本.text);
            }
        }

        // 转换为 JSON 并保存到 PlayerPrefs
        string jsonData = JsonUtility.ToJson(new 保存数据 { 兵种名称列表 = 已选择兵种名称列表 });
        PlayerPrefs.SetString("已选择兵种", jsonData);
        PlayerPrefs.Save();
    }

    [System.Serializable]
    private class 保存数据
    {
        public List<string> 兵种名称列表;
    }

    private void 从本地存储加载()
    {
        if (PlayerPrefs.HasKey("已选择兵种"))
        {
            string jsonData = PlayerPrefs.GetString("已选择兵种");
            var 保存内容 = JsonUtility.FromJson<保存数据>(jsonData);

            foreach (string 名称 in 保存内容.兵种名称列表)
            {
                var 对应兵种 = 兵种列表.Find(兵种 => 兵种.名称 == 名称);
                if (对应兵种 != null)
                {
                    当前能量 -= 对应兵种.消耗碎片;
                    更新能量文本();

                    var 新按钮 = Instantiate(按钮预制体, 已选择的父物体);

                    更新按钮内容(新按钮, 对应兵种);

                    可移除按钮列表.Add(新按钮);

                    var 按钮组件 = 新按钮.GetComponent<Button>();
                    按钮组件.onClick.AddListener(() =>
                    {
                        切换兵种模型(对应兵种);
                        当前被选中的按钮 = 新按钮;
                    });

                    Debug.Log($"成功加载按钮: {新按钮.name}");
                }
            }
        }
    }

    public void 清空兵种列表()
    {
        // 恢复能量并移除所有按钮
        foreach (var 按钮 in 可移除按钮列表)
        {
            var 名称文本 = 按钮.transform.Find("名称").GetComponent<Text>();
            if (名称文本 != null)
            {
                var 对应兵种 = 兵种列表.Find(兵种 => 兵种.名称 == 名称文本.text);
                if (对应兵种 != null)
                {
                    当前能量 += 对应兵种.消耗碎片;
                }
            }

            // 销毁按钮对象
            Destroy(按钮);
        }

        // 清空列表
        可移除按钮列表.Clear();

        // 更新能量显示
        更新能量文本();

        保存到本地存储();
    }
    private void 显示默认兵种()
    {
        if (兵种列表.Count > 0)
        {
            切换兵种模型(兵种列表[0]);
        }
    }

    private void 更新描述(string 描述内容)
    {
        if (当前描述文本 != null)
        {
            Destroy(当前描述文本);
        }

        if (描述预制体 != null && 描述父物体 != null)
        {
            当前描述文本 = Instantiate(描述预制体, 描述父物体);
            var 文本组件 = 当前描述文本.GetComponent<Text>();
            if (文本组件 != null)
            {
                文本组件.text = 描述内容;
            }
        }
    }

    private void 更新属性显示()
    {
        if (当前模型 != null)
        {
            var 属性 = 当前模型.GetComponent<基类>();
            if (属性 != null)
            {
                显示属性文本.text = $"生命值: {属性.属性配置.生命值}\n" +
                                     $"攻击力: {属性.属性配置.攻击力}\n" +
                                     $"防御: {属性.属性配置.防御}\n" +
                                     $"回血: {属性.属性配置.回复血量速度}\n" +
                                     $"攻击距离: {属性.属性配置.攻击距离}";
            }
        }
    }

    private void 生成所有兵种按钮()
    {
        foreach (var 兵种 in 兵种列表)
        {
            生成单个按钮(兵种);
        }
    }

    private void 生成单个按钮(兵种信息 兵种)
    {
        var 新按钮 = Instantiate(按钮预制体, 按钮父物体);
        更新按钮内容(新按钮, 兵种);

        var 按钮组件 = 新按钮.GetComponent<Button>();
        按钮组件.onClick.AddListener(() =>
        {
            切换兵种模型(兵种);
            当前被选中的按钮 = 新按钮;
        });
    }

    private void 更新按钮内容(GameObject 按钮, 兵种信息 兵种)
    {
        var 按钮图片 = 按钮.GetComponent<Image>();
        if (按钮图片 != null)
        {
            按钮图片.sprite = 兵种.图片;
        }

        var 名称文本 = 按钮.transform.Find("名称").GetComponent<Text>();
        if (名称文本 != null)
        {
            名称文本.text = 兵种.名称;
        }

        var 消耗碎片文本 = 按钮.transform.Find("消耗碎片文本").GetComponent<Text>();
        if (消耗碎片文本 != null)
        {
            消耗碎片文本.text = 兵种.消耗碎片.ToString();
        }
    }

    private void 切换兵种模型(兵种信息 兵种)
    {
        当前选择索引 = 兵种列表.IndexOf(兵种);

        if (当前模型 != null)
        {
            Destroy(当前模型);
        }

        if (兵种.模型 != null)
        {
            当前模型 = Instantiate(兵种.模型, Vector3.zero, Quaternion.identity);
        }

        更新描述(兵种.描述);
        更新属性显示();
    }

    public void 选择兵种()
    {
        if (当前选择索引 >= 0 && 当前选择索引 < 兵种列表.Count)
        {
            var 当前兵种 = 兵种列表[当前选择索引];

            if (是否可选择(当前被选中的按钮) && 当前能量 >= 当前兵种.消耗碎片)
            {
                当前能量 -= 当前兵种.消耗碎片;
              
              
                更新能量文本();
                var 新按钮 = Instantiate(按钮预制体, 已选择的父物体);
                可移除按钮列表.Add(新按钮);
                更新按钮内容(新按钮, 当前兵种);
              
                保存到本地存储(); 

                var 按钮组件 = 新按钮.GetComponent<Button>();
                按钮组件.onClick.AddListener(() =>
                {
                    切换兵种模型(当前兵种);
                    当前被选中的按钮 = 新按钮;
                   
                });
            }
            else
            {
                StartCoroutine(显示能量不足提示());
            }
        }
    }

    public void 移除按钮()
    {
        if (当前被选中的按钮 != null && 可移除按钮列表.Contains(当前被选中的按钮))
        {
            var 名称文本 = 当前被选中的按钮.transform.Find("名称").GetComponent<Text>();
            if (名称文本 != null)
            {
                var 对应兵种 = 兵种列表.Find(兵种 => 兵种.名称 == 名称文本.text);
                if (对应兵种 != null)
                {
                    当前能量 += 对应兵种.消耗碎片;
                    更新能量文本();
                }
            }

            可移除按钮列表.Remove(当前被选中的按钮);
            Destroy(当前被选中的按钮);
            当前被选中的按钮 = null;
            
            保存到本地存储();
        }
    }

    private IEnumerator 显示能量不足提示()
    {
        提示框.SetActive(true);
        yield return new WaitForSeconds(1f);
        提示框.SetActive(false);
    }

    private void 更新能量文本()
    {
        当前能量文本.text = $"能量剩余: {当前能量}";
    }

    private bool 是否可选择(GameObject 按钮)
    {
        return !可移除按钮列表.Contains(按钮);
    }
}
