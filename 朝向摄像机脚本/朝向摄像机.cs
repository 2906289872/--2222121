using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 朝向摄像机 : MonoBehaviour
{
    private Camera 主摄像机;

    private void Start()
    {
        // 获取主摄像机
        主摄像机 = Camera.main;
    }

    private void LateUpdate()
    {
        // 始终面向摄像机
        if (主摄像机 != null)
        {
            transform.LookAt(transform.position + 主摄像机.transform.rotation * Vector3.forward,
                             主摄像机.transform.rotation * Vector3.up);
        }
    }
}
