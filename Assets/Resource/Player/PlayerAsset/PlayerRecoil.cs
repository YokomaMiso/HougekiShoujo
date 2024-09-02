using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    const float recoilTime = 1;
    float timer;
    bool isRecoil;

    public void SetRecoil() { isRecoil = true; }
    public bool GetIsRecoil() { return isRecoil; }

    void Update()
    {
        if (isRecoil)
        {
            timer += ownerPlayer.managerMaster.timeManager.GetDeltaTime();
            if (timer > recoilTime)
            {
                isRecoil = false;
                timer = 0;
            }
        }
    }
}
