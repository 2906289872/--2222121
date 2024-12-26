using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 水晶 : 基类
{

    protected override void Start()
    {
        初始化属性(属性配置);

      
    }


    protected override void Update()
    {
        更新血条();
      
    }
    public virtual void 受伤(float 伤害值)
    {
        base.受伤(伤害值); // 调用基类的受伤逻辑
      
    }
    protected override void 死亡()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
   
}
