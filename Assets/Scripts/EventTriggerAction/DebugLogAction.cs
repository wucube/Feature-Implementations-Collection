using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogAction : BaseAction
{
    [Header("DebugLogAction"), TextArea]
    public string DebugContent = "DebugLogAction Execute";

    public bool log = true;
    public bool LogWarning = false;
    public bool LogError = false;

    protected override void RunningLogic()
    {
        base.RunningLogic();
        if (log)
            Debug.Log(DebugContent);
        if (LogWarning)
            Debug.LogWarning(DebugContent);
        if (LogError)
            Debug.LogError(DebugContent);

        RunOver();
    }
}
