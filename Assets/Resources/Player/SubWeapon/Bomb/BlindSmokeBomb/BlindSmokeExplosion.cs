using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlindSmokeExplosion : ExplosionBehavior
{
    bool hitCheck = false;
    bool[] hitedPlayer = new bool[6];

    [SerializeField] GameObject blindCanvas;
    const float vfxStopSub = 1.5f;
    bool vfxStop;

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

    protected override void Update()
    {
        base.Update();
        if (vfxStop) { return; }
        if (timer > lifeTime - vfxStopSub)
        {
            transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            vfxStop = true;
        }
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

        //自分じゃないならリターン
        if (id != Managers.instance.playerID) { return; }

        if (hitCheck) { return; }

        Instantiate(blindCanvas);
        hitCheck = true;
    }
}
