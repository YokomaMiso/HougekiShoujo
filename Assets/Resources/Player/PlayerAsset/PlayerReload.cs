using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReload : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    float reloadTime = 2;
    float timer;
    int shellNum = -1;

    public bool reloadFlagForOther;

    public void Init()
    {
        timer = 0;
    }

    void Start()
    {
        reloadTime = ownerPlayer.GetPlayerData().GetShell().GetReloadTime();
    }

    void Update()
    {
        if (!reloadFlagForOther) { return; }

        timer += Managers.instance.timeManager.GetDeltaTime();
        if (timer > reloadTime)
        {
            timer = 0;
            reloadFlagForOther = false;
            if (ownerPlayer.GetPlayerData().GetShell().GetShellType() != SHELL_TYPE.SPECIAL)
            {
                ownerPlayer.ChangeShellIconColor(1);
                ownerPlayer.canonState = CANON_STATE.RELOADED;
            }
            else
            {
                ownerPlayer.SubActionCutOff();
            }
        }
    }

    public int ReloadBehavior()
    {
        timer += Managers.instance.timeManager.GetDeltaTime();
        if (timer > reloadTime)
        {
            timer = 0;
            if (ownerPlayer.GetPlayerData().GetShell().GetShellType() != SHELL_TYPE.SPECIAL)
            {
                ownerPlayer.ChangeShellIconColor(1);
            }
            else
            {
                ownerPlayer.SubActionCutOff();
            }
            return shellNum;
        }

        return -1;
    }

    public void Reload(int _num)
    {
        shellNum = _num;

        float reloadRate = 0;
        if (ownerPlayer.GetPlayerData().GetShell().GetShellType() == SHELL_TYPE.SPECIAL)
        {
            float nowTimer = ownerPlayer.GetSubWeaponReload();
            float maxTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetReloadTime();
            reloadRate = Mathf.Clamp01(nowTimer / maxTime);
        }
        ownerPlayer.PlayVoice(ownerPlayer.GetPlayerData().GetPlayerVoiceData().GetReload(reloadRate));
        GameObject reloadSFX = SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetReloadSFX(), transform);
        reloadSFX.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);

        if (!ownerPlayer.IsMine())
        {
            reloadFlagForOther = true;
        }
    }
    public bool Reloading() { return timer != 0; }
    public void ReloadCancel() { timer = 0; }
}
