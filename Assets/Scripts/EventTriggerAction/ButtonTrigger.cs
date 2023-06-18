using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTrigger : BaseTrigger
{

    [Header("ButtonTrigger")]
    public Button btn;

    protected void Start()
    {
        RegisterSaveTypeEvent();
    }
    // Start is called before the first frame update

    protected void OnDestroy()
    {
        DeleteSaveTypeEvent();
    }

    public override void RegisterSaveTypeEvent()
    {
        btn.onClick.AddListener(Execute);
    }

    public override void DeleteSaveTypeEvent()
    {
        btn.onClick.RemoveListener(Execute);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
