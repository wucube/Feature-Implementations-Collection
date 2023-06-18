using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    /// <summary>
    /// 根据角度获取对应单位向量
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 GetVectorFormAngle(float angle)
    {
        // angle = 0 -> 360

        //角度转弧度
        float angleRad = angle * (Mathf.PI/180f);
        //获取一个角度对应的 x,y 坐标值
        return new Vector3(Mathf.Cos(angleRad),0,Mathf.Sin(angleRad));
    }

    
    /// <summary>
    /// 根据方向向量获取对应角度
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        //根据正切值返回对应弧度，再转为对应角度
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0) n += 360;

        return n;
    }
}
