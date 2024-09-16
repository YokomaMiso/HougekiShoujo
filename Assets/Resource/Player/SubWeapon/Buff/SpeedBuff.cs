using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    PlayerMove playerMove;

    void Start()
    {
        if (GetComponent<PlayerMove>())
        {
            playerMove = GetComponent<PlayerMove>();
            buffNum = playerMove.AddSpeedRate(speedRate);
            //PlayerMoveにバフを送れたらリターン
            if (buffNum >= 0) { return; }
        }

        //送るのに失敗したらこのクラスを破棄
        Destroy(this);
    }

    protected override void BuffBehavior()
    {
        playerMove.ResetSpeedRate(buffNum);

    }
}
