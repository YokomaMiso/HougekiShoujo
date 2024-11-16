using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PileBunkerBehavior : InstallationBehavior
{
    bool[] hitedPlayer = new bool[6];
    float speedRate;
    float buffLifeTime;

    protected override void Start()
    {
        lifeTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetLifeTime();

        transform.GetChild(0).gameObject.SetActive(false);

        imageAnimator = transform.GetChild(1).GetComponent<Animator>();
        imageAnimator.gameObject.SetActive(true);

        speedRate = ownerPlayer.GetPlayerData().GetSubWeapon().GetSpeedRate();
        buffLifeTime = 8.0f;

        hitedPlayer[ownerPlayer.GetPlayerID()] = true;
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        TimeSetting();

        timer += deltaTime;
        //transform.localScale = Vector3.one * Mathf.Clamp01(timer / lifeTime) * 3;
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

        //”í’eƒ{ƒCƒX‚ð–Â‚ç‚·
        player.PlayVoice(player.GetPlayerData().GetPlayerVoiceData().GetDamageTrap(), Camera.main.transform);
    }
}
