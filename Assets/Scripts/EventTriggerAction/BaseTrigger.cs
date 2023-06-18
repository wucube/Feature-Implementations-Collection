using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 触发器决定事件何时被触发
public class BaseTrigger : BaseState
{
    [Header("Trigger")]
    [LabelText("执行Action")]
    public List<BaseState> actions;

    [LabelText("在DisOnEnable中注销事件")]
    public bool DeleteEventOnDisEnable = false;

    public void GetActions()
    {
        //获取子物体上的所有Action函数
        actions = new List<BaseState>(GetComponentsInChildren<BaseState>());
    }

    protected override void Awake()
    {
        GetActions();
    }

    public override void Execute()
    {
        if (executeType == ExecuteType.Running) return;

        base.Execute();

        Running();
    }

    public override void Running()
    {
        base.Running();

        //执行第一个命令
        if (actions != null && actions.Count > 0)
            actions[0].Execute();
        else
            RunOver();
    }

    public override void RunOver()
    {
        base.RunOver();
        Exit();
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected void OnDisable()
    {
        //base.OnDisable;

        if (DeleteEventOnDisEnable)
            DeleteSaveTypeEvent();
    }

    [Button("绑定事件")]
    public virtual void RegisterSaveTypeEvent()
    {

    }

    [Button("注销事件")]
    public virtual void DeleteSaveTypeEvent()
    {

    }
}