    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 摇杆类型 
/// </summary>
public enum JoystickType
{
    Fixed,
    ChangePos,
    Move
}

public class JoystickPanel : BasePanel
{   
    //摇杆的活动范围
    public float limitRadius = 140f;

    public JoystickType joystickType = JoystickType.Fixed;
    private Image imgTouchRange;
    private Image imgBG; 
    private Image imgControl;
    // Start is called before the first frame update
    void Start()
    {
        //摇杆背景图         
        imgBG = GetControl<Image>("ImgBG");
        //获取鼠标的按下、抬起、拖拽 这三个事件的监听者
        imgTouchRange = GetControl<Image>("ImgTouchRange"); //ImgTouchRange的图片像素透明度为1，这样才能吃事件

        imgControl = GetControl<Image>("ImgControl");

        //用UI管理器提供的添加自定义事件监听的方法，将对应的函数和事件关联
        UIManager.AddCustomEventListener(imgTouchRange, EventTriggerType.PointerDown, PointerDown);
        UIManager.AddCustomEventListener(imgTouchRange, EventTriggerType.PointerUp, PointerUp);
        UIManager.AddCustomEventListener(imgTouchRange, EventTriggerType.Drag, Drag);

        if(joystickType != JoystickType.Fixed)
        {
            //可变位置摇杆——摇杆背景图初始要隐藏自己
            imgBG.gameObject.SetActive(false);
        }   
    }

    private void PointerDown(BaseEventData data)
    {   //可变位置摇杆——按下显示
        imgBG.gameObject.SetActive(true);


        if(joystickType != JoystickType.Fixed)
        {
            //可变位置摇杆——出现在点击屏幕的位置
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imgTouchRange.rectTransform,
                (data   as PointerEventData).position,
                (data as PointerEventData).pressEventCamera,
                out localPos);

            imgBG.transform.localPosition = localPos;
        }
        
    }

    private void PointerUp(BaseEventData data)
    {
        //摇杆回归原位
        imgControl.transform.localPosition = Vector3.zero;

        //将当前摇杆的滑动方向分发出去
        EventCenter.Instance().EventTrigger<Vector2>("Joystick",Vector2.zero);

        if(joystickType != JoystickType.Fixed)
        {
            imgBG.gameObject.SetActive(false);
        }
    }

    private void Drag(BaseEventData data)
    {
        Vector2 localPos;

        //屏幕坐标转UI本地坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgBG.rectTransform,//父对象的中心点会影响转换后的坐标
            (data as PointerEventData).position,
            (data as PointerEventData).pressEventCamera,
            out localPos);

        //摇杆活动范围判断
        if(localPos.magnitude > limitRadius)
        {
            if(joystickType == JoystickType.Move)
            {
                //超出多少，就让背景图做相应的移动
                imgBG.transform.localPosition += (Vector3)(localPos.normalized * (localPos.magnitude - limitRadius));
            }
            
            //摇杆超出范围，就等这个范围
            imgControl.transform.localPosition = localPos.normalized * limitRadius;
        } 
        
        else
            imgControl.transform.localPosition = localPos;

        //将当前摇杆的滑动方向分发出去
        EventCenter.Instance().EventTrigger<Vector2>("Joystick",localPos.normalized);

    } 
}
