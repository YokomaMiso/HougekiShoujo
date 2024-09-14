using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPMine : ProjectileBehavior
{
    float angle = 0;

    void Start()
    {
        TagSetting();
    }
    protected override void SpawnExplosion()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 2;
        GameObject explosion = ownerPlayer.GetPlayerData().GetSubWeapon().GetExplosion();
        GameObject obj = Instantiate(explosion, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (timer < 3) { return; }

        if (other.tag == hitTags[0])
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
    }

    protected override void TagSetting()
    {
        hitTags = new string[1];
        hitTags[0] = playerTag;
    }

}