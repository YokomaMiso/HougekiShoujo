using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPMine : ProjectileBehavior
{
    float angle = 0;
    [SerializeField] GameObject explosion;

    protected override void Start()
    {
        hitTags = new string[1];
        hitTags[0] = playerTag;
    }
    protected override void OnDestroy()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 2;
        //GameObject explosion = ownerPlayer.GetPlayerData().GetSubWeapon().GetExplosion();
        GameObject obj = Instantiate(explosion, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (timer < 3) { return; }

        if (other.tag == hitTags[0])
        {
            if (other.GetComponent<Player>() != ownerPlayer) { Destroy(gameObject); }
        }
    }
}