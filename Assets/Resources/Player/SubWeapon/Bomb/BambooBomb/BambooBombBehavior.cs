using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooBombBehavior : BombBehavior
{
    [SerializeField] GameObject bambooWall;

    protected override void SpawnExplosion()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0;

        Vector3 vector = rb.velocity.normalized;
        float angle = Mathf.Atan2(vector.z, vector.x);

        Instantiate(bambooWall, spawnPos, Quaternion.Euler(0, angle, 0));
    }
}
