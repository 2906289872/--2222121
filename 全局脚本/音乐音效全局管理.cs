using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 音乐音效全局管理 : MonoBehaviour
{
    // 单例实例
    public static 音乐音效全局管理 实例;

    // 音效数组
    public AudioClip[] 音效数组;

    // 音频播放器
    private AudioSource audioSource;

    // 确保只有一个实例并且场景切换时保留
    private void Awake()
    {
        // 如果实例为空，设置为当前对象实例，否则销毁当前对象
        if (实例 == null)
        {
            实例 = this;
            DontDestroyOnLoad(gameObject); // 保持对象在场景切换时不销毁
        }
        else
        {
            Destroy(gameObject); // 销毁重复的实例
        }

        // 获取音频播放器
        audioSource = GetComponent<AudioSource>();
    }

    // 播放指定的音效
    public void 播放音效(int 索引)
    {
        if (索引 >= 0 && 索引 < 音效数组.Length)
        {
            audioSource.PlayOneShot(音效数组[索引]); // 播放对应的音效
        }
        else
        {
            Debug.LogWarning("音效索引无效！");
        }
    }
}
