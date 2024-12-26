using UnityEngine;

public class 旋转物体 : MonoBehaviour
{
    public bool 是否正在旋转 = false; // 是否正在旋转
    public Vector3 上一次鼠标位置; // 上一次鼠标位置

    public float 旋转速度 = 5.0f; // 控制旋转速度

    void Update()
    {
        // 检测鼠标按下
        if (Input.GetMouseButtonDown(0))
        {
            Ray 射线 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit 碰撞信息;

            // 检测鼠标是否点击了当前物体
            if (Physics.Raycast(射线, out 碰撞信息))
            {
                if (碰撞信息.transform == transform)
                {
                    是否正在旋转 = true;
                    上一次鼠标位置 = Input.mousePosition;
                }
            }
        }

        // 检测鼠标松开
        if (Input.GetMouseButtonUp(0))
        {
            是否正在旋转 = false;
        }

        // 如果正在旋转
        if (是否正在旋转)
        {
            Vector3 当前鼠标位置 = Input.mousePosition;
            float 鼠标移动差 = 当前鼠标位置.x - 上一次鼠标位置.x;

            // 根据鼠标水平移动改变物体的旋转（反向）
            transform.Rotate(Vector3.up, -鼠标移动差 * 旋转速度 * Time.deltaTime);

            上一次鼠标位置 = 当前鼠标位置;
        }
    }
}
