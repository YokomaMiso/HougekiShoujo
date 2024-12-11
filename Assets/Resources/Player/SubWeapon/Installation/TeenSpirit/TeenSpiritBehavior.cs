using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeenSpiritBehavior : InstallationBehavior
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
        buffLifeTime = 8.0f;

        hitedPlayer[ownerPlayer.GetPlayerID()] = true;

        GameObject obj = SoundManager.PlaySFX(launchSFX, this.transform);
        obj.GetComponent<AudioSource>().pitch = 1;
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        TimeSetting();

        timer += deltaTime;
        float nowRate = Mathf.Sqrt(Mathf.Clamp01(timer / lifeTime));
        transform.localScale = Vector3.one * nowRate * 3;

        nowRate = Mathf.Pow(Mathf.Clamp01(timer / lifeTime), 3);
        vfxSpriteRenderer.color = new Color(1, 1, 1, 1.0f - nowRate);
        if (timer > lifeTime) { Destroy(gameObject); }
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
        //player.AddComponent<SpeedBuff>().SetRateAndTime(speedRate, buffLifeTime);
        Vector3 direction = (player.transform.position - ownerPlayer.transform.position).normalized;
        player.GetComponent<PlayerMove>().ReceiveSlip(0.5f, direction * 10);

        //”í’eƒ{ƒCƒX‚ð–Â‚ç‚·
        player.PlayVoice(player.GetPlayerData().GetPlayerVoiceData().GetDamageTrap(), Camera.main.transform);
    }
}
