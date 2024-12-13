using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeStart : SceneChange
{
    [SerializeField] AudioClip startSFX;

    protected override void Start()
    {
        sfx = startSFX;
        base.Start();
    }

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
