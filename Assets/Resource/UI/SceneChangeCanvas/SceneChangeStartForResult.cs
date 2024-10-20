using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeStartForResult : SceneChange
{
    protected override void Start()
    {
        lifeTime = 2.5f;
    }

    void Update()
    {
        TimerUpdate();
        DestroyCheck(true);
    }
}
