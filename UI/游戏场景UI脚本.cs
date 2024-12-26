using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 游戏场景UI脚本 : MonoBehaviour
{
    public GameObject 是否投降提示;
    void Start()
    {
        是否投降提示.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public   void 是否投降方法()
    {
        Time.timeScale = 0f;
        是否投降提示.SetActive(true);
    }

    public void 确认投降()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("主菜单场景");

    }

    public void 拒绝投降()
    {
        是否投降提示.SetActive(false);
        Time.timeScale = 1;

    }
}
