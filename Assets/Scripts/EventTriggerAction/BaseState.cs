using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//BaseState 为 Trigger 和 Action 的统一基类
public abstract class BaseState : MonoBehaviour
{

    [TextArea,LabelText("说明")]
    public string content;
    
    [LabelText("当前状态")]
    public ExecuteType executeType;

    public BaseState parentState;

    public enum ExecuteType
    {
        [LabelText("未执行")] None,
        [LabelText("准备执行")] Enter,
        [LabelText("正在执行")] Running,
        [LabelText("执行完成 待机")] RunOver,
        [LabelText("执行完成 退出")] Exit,
    }

    protected virtual void  Awake() 
    {
        executeType = ExecuteType.None;
    }
    protected virtual void OnEnter()
    {
        executeType = ExecuteType.Enter;
    }

    protected virtual void OnRunning()
    {
        executeType = ExecuteType.Running;
    }
    
    protected virtual void OnRunOver()
    {
        executeType = ExecuteType.RunOver;
    }
    protected virtual void OnExit()
    {
        executeType = ExecuteType.Exit;
    }

    [Button("执行该状态")]
    public virtual void Execute()
    {
        OnEnter();
    }

    public virtual void Running()
    {
        OnRunning();
    }

    public virtual void RunOver()
    {
        OnRunOver();
    }
    public virtual void Exit()
    {
        OnExit();
    }
    protected virtual void OnAddState()
    {
        
    }


}
