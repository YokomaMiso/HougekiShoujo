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

        GameObject obj;

        switch (subWeaponData.GetSubType())
        {
            case SUB_TYPE.BUFF:
                float speedRate = subWeaponData.GetSpeedRate();
                float lifeTime = subWeaponData.GetLifeTime();
                switch (subWeaponData.GetBuffType())
                {
                    case BUFF_TYPE.SPEED:
                        transform.AddComponent<SpeedBuff>().SetRateAndTime(speedRate, lifeTime);
                        break;
                    case BUFF_TYPE.RELOAD:
                        transform.AddComponent<ReloadBuff>().SetRateAndTime(speedRate, lifeTime);
                        break;
                }
                break;
            case SUB_TYPE.INSTALLATION:
                obj = Instantiate(subWeaponData.GetInstallation(), transform.position, Quaternion.identity);
                obj.GetComponent<InstallationBehavior>().SetPlayer(ownerPlayer);
                break;
            case SUB_TYPE.BLINK:
                transform.AddComponent<Blink>();
                break;
            case SUB_TYPE.BOMB:
                Vector3 spawnPos = transform.position + Vector3.up + ownerPlayer.GetInputVector();
                obj = Instantiate(subWeaponData.GetInstallation(), spawnPos, Quaternion.identity);
                obj.GetComponent<BombBehavior>().SetPlayer(ownerPlayer);
                obj.GetComponent<BombBehavior>().LaunchBomb(ownerPlayer.GetInputVector());
                break;
        }

        ownerPlayer.PlayVoice(ownerPlayer.GetPlayerData().GetPlayerVoiceData().GetUseSub());
        reloadTimer = subWeaponData.GetReloadTime();
        if (ownerPlayer.IsMine()) { OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.useSub = !OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.useSub; }
    }
}
