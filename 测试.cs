using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 测试 : MonoBehaviour
{
    public Animator 动画;
    void 播放攻击动画()
    {
        动画.SetBool("攻击", true);
    }
 public void 攻击()
    {
        Debug.Log("攻击");
    }
}
