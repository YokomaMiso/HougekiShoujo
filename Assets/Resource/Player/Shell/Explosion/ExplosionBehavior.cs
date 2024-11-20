using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    protected Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }
    public Player GetPlayer() { return ownerPlayer; }

    protected Animator imageAnimator;
    protected string hitTag = "Player";
    [SerializeField, Range(0.0f, 1.0f)] protected float cameraShakeRate = 1.0f;

    protected float lifeTime;
    protected float timer;

    public virtual void SetData(Explosion _data)
    {
        imageAnimator = transform.GetChild(0).GetComponent<Animator>();

        float scale = _data.GetScale();
        transform.localScale = new Vector3(scale, scale, scale);

        imageAnimator.runtimeAnimatorController = _data.GetAnim();

    }

    protected virtual void Start()
    {
        lifeTime = imageAnimator.GetCurrentAnimatorStateInfo(0).length * 0.5f;

        GameObject obj = SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetExplosionSFX());
        obj.transform.position = this.transform.position;

        Vector3 distance = transform.position - Camera.main.transform.position;
        float weight = distance.magnitude / 5;
        if (weight < 1) { weight = 1; }

        float shakeValue = 1.0f / weight * cameraShakeRate;
        Camera.main.GetComponent<CameraMove>().SetCameraShake(shakeValue);
    }

    protected virtual void Update()
    {
        imageAnimator.speed = 2 * Managers.instance.timeManager.TimeRate();

        timer += Managers.instance.timeManager.GetDeltaTime();
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        //当たったオブジェクトからPlayer型を取得
        Player player = other.GetComponent<Player>();

        //Player型でなければ早期リターン
        if (!player) { return; }

        //自分のキャラクターじゃなければ早期リターン
        if (player && player.GetPlayerID() != Managers.instance.playerID) { return; }

        //自分のキャラクターの死亡判定を行う
        other.GetComponent<Player>().SetDead(ownerPlayer.GetPlayerID());
    }
}
