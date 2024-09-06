using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
    protected Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    [SerializeField] protected Animator imageAnimator;
    [SerializeField] protected float lifeTime;
    protected float timer = 0;

    [SerializeField, Tag] protected string playerTag;
    [SerializeField, Tag] protected string groundTag;
    protected string[] hitTags;

    protected virtual void Start()
    {
        imageAnimator.speed = 1 * Managers.instance.timeManager.TimeRate();
        hitTags = new string[2];
        hitTags[0] = playerTag;
        hitTags[1] = groundTag;
    }

    protected virtual void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        timer += deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }

    protected virtual void OnDestroy()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 2;
        GameObject explosion = ownerPlayer.GetPlayerData().GetShell().GetExplosion();
        GameObject obj = Instantiate(explosion, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == hitTags[0] || other.tag == hitTags[1])
        {
            if (other.GetComponent<Player>() != ownerPlayer) { Destroy(gameObject); }
        }
    }
}