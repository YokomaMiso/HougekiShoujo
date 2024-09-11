using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    float recoilTime = 1;
    float timer;
    bool isRecoil;

    public void SetRecoil() { isRecoil = true; }
    public bool GetIsRecoil() { return isRecoil; }

    public void Init()
    {
        timer = 0;
        isRecoil = false;
    }

    void Start()
    {
        recoilTime = ownerPlayer.GetPlayerData().GetShell().GetRecoilTime();
    }

    void Update()
    {
        if (isRecoil)
        {
            timer += Managers.instance.timeManager.GetDeltaTime();
            if (timer > recoilTime)
            {
                isRecoil = false;
                timer = 0;
            }
        }
    }
}
