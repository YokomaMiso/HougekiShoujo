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
    public string[] hitTags;

    public void SetData(Shell _data)
    {
        imageAnimator = transform.GetChild(0).GetComponent<Animator>();
        imageAnimator.runtimeAnimatorController = _data.GetAnim();
        lifeTime = _data.GetLifeTime();
        explosion = _data.GetExplosion();
        TagSetting();
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
        spawnPos.y = explosion.GetScale();
        GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == hitTags[0])
        {
            if (other.GetComponent<Player>() != ownerPlayer)
            {
                SpawnExplosion();
                Destroy(gameObject);
            }
        }
        else if (other.tag == hitTags[1])
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
    }

    protected virtual void TagSetting()
    {
        hitTags = new string[2];
        hitTags[0] = playerTag;
        hitTags[1] = groundTag;
    }

    protected virtual void TimeSetting()
    {
        imageAnimator.speed = 1 * Managers.instance.timeManager.TimeRate();
    }
}