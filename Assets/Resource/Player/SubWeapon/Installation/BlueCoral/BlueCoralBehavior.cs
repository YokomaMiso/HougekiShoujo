using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlueCoralBehavior : InstallationBehavior
{
    bool[] hitedPlayer = new bool[6];
    float speedRate;
    float buffLifeTime;

    Transform vfx;
    MeshRenderer vfxRenderer;
    Vector3 vfxScale;
    float vfxDefaultScaleY;

    protected override void Start()
    {
        lifeTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetLifeTime();

        vfx = transform.GetChild(0);
        vfxRenderer = vfx.GetComponent<MeshRenderer>();
        vfxScale = vfx.localScale;
        vfxDefaultScaleY = vfxScale.y;

        speedRate = ownerPlayer.GetPlayerData().GetSubWeapon().GetSpeedRate();
        buffLifeTime = 5.0f;

        transform.localScale = Vector3.zero;
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();

        timer += deltaTime;
        transform.localScale = Vector3.one * Mathf.Clamp01(timer / lifeTime) * 3;

        vfx.localRotation = Quaternion.Euler(0, 360 * timer, 0);
        vfxScale.y = Mathf.Lerp(vfxDefaultScaleY, vfxDefaultScaleY / 3, Mathf.Clamp01(timer / lifeTime));
        vfx.localScale = vfxScale;

        float nowRate = Mathf.Pow(Mathf.Clamp01(timer / lifeTime), 3);
        vfxRenderer.material.color = new Color(1, 1, 1, 1.0f - nowRate);

        if (timer > lifeTime) { Destroy(gameObject); }
    }

    protected override void TimeSetting()
    {
        if (imageAnimator == null) { return; }
        imageAnimator.speed = 3 * Managers.instance.timeManager.TimeRate();
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }

    void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) { return; }

        int id = player.GetPlayerID();
        if (hitedPlayer[id]) { return; }

        hitedPlayer[id] = true;
        player.AddComponent<SpeedBuff>().SetRateAndTime(speedRate, buffLifeTime);

        //自分自身ならリターン
        if (id == Managers.instance.playerID) { return; }

        //敵チームならリターン
        MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
        MachingRoomData.RoomData roomData = OSCManager.OSCinstance.GetRoomData(id);
        if (roomData.myTeamNum != myRoomData.myTeamNum) { return; }

        //感謝ボイスを鳴らす
        player.PlayVoice(player.GetPlayerData().GetPlayerVoiceData().GetThanks(), Camera.main.transform);
    }
}
