using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPMine : ProjectileBehavior
{
    [SerializeField] Explosion empExplosion;

    void Start()
    {
        TagSetting();
        lifeTime = Mathf.Infinity;
    }
    protected override void TimeSetting()
    {
    }

    protected override void SpawnExplosion()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 2;
        GameObject explosion = empExplosion.GetBody();
        GameObject obj = Instantiate(explosion, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(empExplosion);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (timer < 3) { return; }

        Debug.Log(timer);

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