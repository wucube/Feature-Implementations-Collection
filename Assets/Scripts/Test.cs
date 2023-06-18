using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    int i;
    void Awake() 
    {
        Debug.Log("禁用脚本也能打印出来");
        
    }


    void  OnEnable() 
    {
        i++;
        Debug.Log(i);
    }
    // Start is called before the first frame update
    void Start()
    {
        string s = string.Empty;
        GameObject go = new GameObject();
        DestroyImmediate(go);
        if(!go)
            s += "A";
        if(go is null)
            s += "B";
        if(go == null)
            s += "C";
        if((System.Object)go == null)
            s += "D";
        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
