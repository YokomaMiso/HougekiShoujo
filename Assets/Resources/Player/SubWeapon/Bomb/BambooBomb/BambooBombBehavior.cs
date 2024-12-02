using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooBombBehavior : BombBehavior
{
    [SerializeField] GameObject bambooWall;
    bool spawned;
    public override void LaunchBomb(Vector3 _vector)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        addVector = _vector * 1.5f + Vector3.up;
        rb.AddForce(addVector, ForceMode.Impulse);

        SoundManager.PlaySFX(launchSFX, ownerPlayer.transform);
    }

    protected override void SpawnExplosion()
    {
        if (spawned) { return; }

        Vector3 spawnPos = transform.position;
        spawnPos.y = 0;

        Vector3 vector = addVector;
        vector.y = 0;
        float angle = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;

        GameObject obj = Instantiate(bambooWall, spawnPos, Quaternion.Euler(0, angle, 0));
        obj.GetComponent<BambooWallBehavior>().SetData(ownerPlayer.GetPlayerData().GetSubWeapon());
        obj.AddComponent<RoundCheckerSubWeapon>();

        spawned = true;
    }
}
