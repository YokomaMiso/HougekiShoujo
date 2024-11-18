using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PileBunkerBehavior : InstallationBehavior
{
    bool[] hitedPlayer = new bool[6];
    float speedRate;
    float buffLifeTime;
    SpriteRenderer vfxSpriteRenderer;

    protected override void Start()
    {
        lifeTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetLifeTime();

        transform.GetChild(0).gameObject.SetActive(false);

        imageAnimator = transform.GetChild(1).GetComponent<Animator>();
        imageAnimator.gameObject.SetActive(true);
        vfxSpriteRenderer = imageAnimator.GetComponent<SpriteRenderer>();

        speedRate = ownerPlayer.GetPlayerData().GetSubWeapon().GetSpeedRate();
        buffLifeTime = 1.0f;

        hitedPlayer[ownerPlayer.GetPlayerID()] = true;

        Vector3 distance = transform.position - Camera.main.transform.position;
        float weight = distance.magnitude / 5;
        if (weight < 1) { weight = 1; }

        float shakeValue = 1.0f / weight * 0.75f;
        Camera.main.GetComponent<CameraMove>().SetCameraShake(shakeValue);
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        TimeSetting();

        timer += deltaTime;

        float nowRate = Mathf.Pow(Mathf.Clamp01((timer + 0.2f) / lifeTime), 4);
        vfxSpriteRenderer.color = new Color(1, 1, 1, 1.0f - nowRate);

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
