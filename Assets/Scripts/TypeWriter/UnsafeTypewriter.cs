using System.Security.Principal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//使用unsafe代码实现OGC的文本打字机效果
unsafe public class UnsafeTypewriter : MonoBehaviour
{
    public Text text;
    public string dialogue;
    public float delay;

    private float timer;
    private int index;

    private char* strPtr;

    void Start()
    {
        //主动触发新字符串创建，防止修改到原来的字符串。
        
        text.text = dialogue + " ";
        fixed(char* strPtr = text.text)
        {
            this.strPtr = strPtr;

            //将新字符串置空
            for (int i = 0; i < text.text.Length; i++)
            {
                strPtr[i] = ' ';
            }
        }
    }
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= delay)
        {
            if(index < dialogue.Length)
            {
                timer -= delay;
                strPtr[index] = dialogue[index];

                //主动触发重建来刷新文本显示。手动调用 SetAllDirty和Rebuid没有效果，只能这样写
                if((index & 1) == 0)
                {
                    text.supportRichText = true;
                }
                else
                {
                    text.supportRichText = false;
                }

                index++;
            }
        }
        
    }
}
