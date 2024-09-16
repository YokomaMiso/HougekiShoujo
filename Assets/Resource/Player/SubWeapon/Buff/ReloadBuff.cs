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
            //PlayerReload�Ƀo�t�𑗂ꂽ�烊�^�[��
            if (buffNum >= 0) { return; }
        }

        //����̂Ɏ��s�����炱�̃N���X��j��
        Destroy(this);
    }

    protected override void BuffBehavior()
    {
        playerReload.ResetSpeedRate(buffNum);
    }
}
