using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeStart : SceneChange
{
    protected override void SetPosition()
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 pos = startPos[i];
            ribbons[i].transform.localPosition = pos;
        }
    }
    void Update()
    {
        TimerUpdate();
        DestroyCheck(true);
        MoveRibbons(startPos, basePos);
    }

    
}
