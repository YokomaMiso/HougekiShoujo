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

    //バフデバフ用の変数
    [SerializeField] ReloadBuffEffectBehavior vfxBehavior;
    float[] speedRate = new float[8] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    const float defaultSpeedRate = 1.0f;

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
        NowSpeedRate();
    }

    public int ReloadBehavior()
    {
        timer += Managers.instance.timeManager.GetDeltaTime() * NowSpeedRate();
        if (timer > reloadTime)
        {
            timer = 0;
            return shellNum;
        }

        return -1;
    }

    public void Reload(int _num)
    {
        shellNum = _num;
        ownerPlayer.PlayVoice(ownerPlayer.GetPlayerData().GetPlayerVoiceData().GetReload());
        GameObject reloadSFX = SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetReloadSFX(), transform);
        reloadSFX.GetComponent<AudioSource>().pitch = NowSpeedRate() * Random.Range(0.9f, 1.1f);

        if (!ownerPlayer.IsMine())
        {
            reloadSFX.GetComponent<AudioSource>().volume *= 0.5f;
            reloadSFX.AddComponent<AudioLowPassFilter>();
        }
    }
    public bool Reloading() { return timer != 0; }
    public void ReloadCancel() { timer = 0; }

    public int AddSpeedRate(float _rate)
    {
        int emptyNum = -1;
        for (int i = 0; i < speedRate.Length; i++)
        {
            if (speedRate[i] == defaultSpeedRate)
            {
                emptyNum = i;
                speedRate[i] = _rate;
                break;
            }
        }

        return emptyNum;
    }
    public void ResetSpeedRate(int _num) { speedRate[_num] = defaultSpeedRate; }

    public float NowSpeedRate()
    {
        float multiplyAllRate = 1.0f;
        for (int i = 0; i < speedRate.Length; i++) { multiplyAllRate *= speedRate[i]; }
        vfxBehavior.DisplayBuff(multiplyAllRate);
        return multiplyAllRate;
    }

    public void ReloadBehaviorForOther() { NowSpeedRate(); } 
}
