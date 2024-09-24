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
            //PlayerMove�Ƀo�t�𑗂ꂽ�烊�^�[��
            if (buffNum >= 0) { return; }
        }

        //����̂Ɏ��s�����炱�̃N���X��j��
        Destroy(this);
    }

    protected override void BuffBehavior()
    {
        playerMove.ResetSpeedRate(buffNum);

    }
}
