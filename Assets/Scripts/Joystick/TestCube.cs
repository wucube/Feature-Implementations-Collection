using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    private Vector3 moveDir;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.Instance().AddEventListener<Vector2>("Joystick",CheckDirChange);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDir * Time.deltaTime, Space.World);

        Debug.Log(moveDir);
    }

    private void CheckDirChange(Vector2 dir)
    {
        moveDir.x = dir.x;
        moveDir.z = dir.y;
    }
}
