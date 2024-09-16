using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBuff : Buff
{
    PlayerReload playerReload;

    void Start()
    {
        if (GetComponent<PlayerReload>())
        {
            playerReload = GetComponent<PlayerReload>();
            buffNum = playerReload.AddSpeedRate(speedRate);
            //PlayerReloadにバフを送れたらリターン
            if (buffNum >= 0) { return; }
        }

        //送るのに失敗したらこのクラスを破棄
        Destroy(this);
    }

    protected override void BuffBehavior()
    {
        playerReload.ResetSpeedRate(buffNum);
    }
}
