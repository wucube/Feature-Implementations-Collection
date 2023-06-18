using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEventTrigger : BaseTrigger
{
    [Header("EventTrigger")]
    public EventEnum eventEnum;
    // Start is called before the first frame update

    protected void Awake()
    {
        base.Awake();
        EventCenter.Instance().AddEventListener(eventEnum.ToString(), Execute);
    }

    protected void OnDestroy()
    {
        //base.OnDestroy();

        EventCenter.Instance().RemoveEventListener(eventEnum.ToString(), Execute);
    }
}

public enum EventEnum
{

}
