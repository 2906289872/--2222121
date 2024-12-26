using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 兵种场景UI脚本 : MonoBehaviour
{
  public void 确认()
    {
        SceneManager.LoadScene("关卡选择场景");
    }
    public void 返回()
    {
        SceneManager.LoadScene("主菜单场景");
    }
}
