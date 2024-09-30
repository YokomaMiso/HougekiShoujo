using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeEnd : SceneChange
{
    protected override void SetPosition()
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 pos = basePos[i];
            ribbons[i].transform.localPosition = pos;
        }
    }
    void Update()
    {
        TimerUpdate();
        DestroyCheck(false);
        MoveRibbons(basePos, endPos);
    }
}
