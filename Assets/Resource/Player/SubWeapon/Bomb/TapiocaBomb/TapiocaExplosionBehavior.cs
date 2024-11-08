using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TapiocaExplosionBehavior : ExplosionBehavior
{
    bool hitCheck = false;
    bool[] hitedPlayer = new bool[6];

    public override void SetData(Explosion _data)
    {
        imageAnimator = transform.GetChild(0).GetComponent<Animator>();

        float scale = _data.GetScale();
        transform.localScale = new Vector3(scale, scale, scale);

        imageAnimator.runtimeAnimatorController = _data.GetAnim();
    }

    protected override void Start()
    {
        //lifeTime = imageAnimator.GetCurrentAnimatorStateInfo(0).length - 0.75f;
        lifeTime = 10;
    }
    protected override void OnTriggerStay(Collider other)
    {
        Player nowPlayer = other.GetComponent<Player>();
        if (nowPlayer == null) { return; }

        int id = nowPlayer.GetPlayerID();
        if (hitedPlayer[id]) { return; }

        //ヒットしたボイスを鳴らす
        hitedPlayer[id] = true;
        //トラップ被弾ボイスを鳴らす
        nowPlayer.PlayVoice(nowPlayer.GetPlayerData().GetPlayerVoiceData().GetDamageTrap(), Camera.main.transform);

        nowPlayer.GetComponent<PlayerMove>().ReceiveSlip(12.0f);
    }
}
