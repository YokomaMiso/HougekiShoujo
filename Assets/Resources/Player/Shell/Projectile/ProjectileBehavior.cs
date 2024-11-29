using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
    protected Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    protected Animator imageAnimator;
    protected float lifeTime;
    protected float timer = 0;
    protected Explosion explosion;

    protected string playerTag = "Player";
    protected string groundTag = "Ground";

    //Canon
    protected float angle = 0;
    protected float speed = 5;

    //Mortar
    protected Vector3 defaultPosition;
    protected Vector3 targetPoint;

    public virtual void SetData(Shell _data)
    {
        imageAnimator = transform.GetChild(0).GetComponent<Animator>();
        imageAnimator.runtimeAnimatorController = _data.GetAnim();
        lifeTime = _data.GetLifeTime();
        explosion = _data.GetExplosion();
        speed = _data.GetSpeed();

        if (_data.GetShellType() != SHELL_TYPE.BLAST) { SoundManager.PlaySFX(ownerPlayer.GetPlayerData().GetPlayerSFXData().GetFlySFX(), transform); }
    }

    protected virtual void Start()
    {
        if (angle != 0) { imageAnimator.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * angle; }
    }

    protected virtual void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        TimeSetting();

        timer += deltaTime;
        if (timer > lifeTime) { SpawnExplosion(); Destroy(gameObject); }
    }

    protected virtual void SpawnExplosion()
    {
        Vector3 spawnPos = transform.position;
        
        GameObject explosionInstance = explosion.GetBody();
        GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag)
        {
            if (other.GetComponent<Player>() != ownerPlayer)
            {
                SpawnExplosion();
                Destroy(gameObject);
            }
        }
        else if (other.tag == groundTag)
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
    }

    protected virtual void TimeSetting()
    {
        imageAnimator.speed = 1 * Managers.instance.timeManager.TimeRate();
    }

    public virtual void SetAngle(float _angle)
    {
        angle = _angle;
    }

    public void ProjectileStart(Vector3 _point)
    {
        targetPoint = _point;
        defaultPosition = this.transform.position;
    }

}