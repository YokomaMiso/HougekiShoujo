using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EMPMine : InstallationBehavior
{
    [SerializeField] Explosion empExplosion;

    void Start()
    {
        lifeTime = Mathf.Infinity;
    }
    protected override void TimeSetting()
    {
    }

    protected override void InstallationAction()
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
        if (timer < 1.5f) { return; }

        if (other.GetComponent<Player>())
        {
            InstallationAction();
            Destroy(gameObject);
        }
    }
}