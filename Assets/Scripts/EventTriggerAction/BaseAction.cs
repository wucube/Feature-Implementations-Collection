using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class BaseAction : BaseState
{
    [Header("Action")]
    [LabelText("延迟多少秒进入")]
    public float delayTime;
    [LabelText("延迟多少秒执行下一个")]
    public float waitTime;
    [LabelText("增加一段具体逻辑")]
    public UnityEvent unityEvent;

    protected override void Awake()
    {
        parentState = GetComponentInParent<BaseTrigger>();
    }

    public override void Execute()
    {
        base.Execute();

        if (delayTime > 0)
            Invoke(nameof(Running), delayTime);
        else
            Running();
    }

    public override void Running()
    {
        base.Running();
        RunningLogic();
    }

    protected virtual void RunningLogic()
    {
        unityEvent?.Invoke();
    }


    public override void RunOver()
    {
        base.RunOver();

        if (waitTime > 0)
            Invoke(nameof(Exit), waitTime);
        else
            Exit();
    }

    public override void Exit()
    {
        //检查爷节点Trigger, 执行下一条命令或者结束Trigger

        if(parentState != null && parentState is BaseTrigger baseTrigger)
        {
            //顺序执行
            var index = baseTrigger.actions.IndexOf(this);
            index++;

            if (index >= baseTrigger.actions.Count)
                baseTrigger.RunOver();
            else
                baseTrigger.actions[index].Execute();
        }

        base.Exit();
    }
}