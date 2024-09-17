using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSubAction : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player)
    {
        ownerPlayer = _player;
        subWeaponData = ownerPlayer.GetPlayerData().GetSubWeapon();
    }

    SubWeapon subWeaponData;
    float reloadTimer;

    public void Init()
    {
        reloadTimer = 0;
    }

    void Update()
    {
        if (CanUse()) { return; }

        reloadTimer -= Managers.instance.timeManager.GetDeltaTime();
    }

    bool CanUse() { return reloadTimer <= 0; }
    public float ReloadTime() { return reloadTimer; }
    public void UseSubWeapon()
    {
        if (!CanUse()) { return; }

        switch (subWeaponData.GetSubType())
        {
            case SUB_TYPE.BUFF:
                float speedRate = subWeaponData.GetSpeedRate();
                float lifeTime = subWeaponData.GetLifeTime();
                transform.AddComponent<Buff>().SetRateAndTime(speedRate, lifeTime);
                break;
            case SUB_TYPE.INSTALLATION:
                GameObject obj = Instantiate(subWeaponData.GetMine(), transform.position + Vector3.up, Quaternion.identity);
                obj.GetComponent<InstallationBehavior>().SetPlayer(ownerPlayer);
                break;
            case SUB_TYPE.BLINK:
                transform.AddComponent<Blink>();
                break;
        }

        reloadTimer = subWeaponData.GetReloadTime();
        if (ownerPlayer.IsMine()) { OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.useSub = !OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.useSub; }
    }
}
