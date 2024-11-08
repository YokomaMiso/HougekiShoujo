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

    public void Init()
    {
        timer = 0;
    }

    void Start()
    {
        reloadTime = ownerPlayer.GetPlayerData().GetShell().GetReloadTime();
    }

    public int ReloadBehavior()
    {
        timer += Managers.instance.timeManager.GetDeltaTime();
        if (timer > reloadTime)
        {
            timer = 0;
            ownerPlayer.ChangeShellIconColor(1);
            return shellNum;
        }

        return -1;
    }

    public void Reload(int _num)
    {
        shellNum = _num;
        ownerPlayer.PlayVoice(ownerPlayer.GetPlayerData().GetPlayerVoiceData().GetReload());
        GameObject reloadSFX = SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetReloadSFX(), transform);
        reloadSFX.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);

        if (!ownerPlayer.IsMine())
        {
            reloadSFX.GetComponent<AudioSource>().volume *= 0.5f;
            reloadSFX.AddComponent<AudioLowPassFilter>();
        }
    }
    public bool Reloading() { return timer != 0; }
    public void ReloadCancel() { timer = 0; }
}
