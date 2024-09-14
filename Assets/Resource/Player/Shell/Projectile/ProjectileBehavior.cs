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

    protected string playerTag = "Player";
    protected string groundTag = "Ground";
    protected string[] hitTags;

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
        GameObject explosion = ownerPlayer.GetPlayerData().GetShell().GetExplosion();
        spawnPos.y = explosion.transform.localScale.y;
        GameObject obj = Instantiate(explosion, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        bool check = false;
        for (int i = 0; i < hitTags.Length; i++)
        {
            if (other.tag == hitTags[i]) { check = true; break; }
        }

        if (check)
        {
            if (other.GetComponent<Player>() != ownerPlayer) { Destroy(gameObject); }
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