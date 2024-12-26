using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 主菜单场景UI脚本 : MonoBehaviour
{
    public void 开始游戏()
    {

        SceneManager.LoadScene("兵种场景");
    }

    public void 打开设置()
    {
    
        SceneManager.LoadScene("游戏设置场景");
    }

  
    public void 打开商店()
    {
       
        SceneManager.LoadScene("商店场景");
    }

   
    public void 退出游戏()
    {
        Application.Quit();
    }
}
