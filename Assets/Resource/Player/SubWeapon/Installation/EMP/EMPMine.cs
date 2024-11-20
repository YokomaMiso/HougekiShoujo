using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EMPMine : InstallationBehavior
{
    [SerializeField] Explosion empExplosion;

    protected override void Start()
    {
        explosion = empExplosion;
        //base.Start();
        lifeTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetLifeTime();
        changeToLoopAnimTime = 0.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (applyLoop) { return; }
        if (timer > changeToLoopAnimTime)
        {
            //imageAnimator.runtimeAnimatorController = loopAnim;
            applyLoop = true;
        }
    }

    public override void InstallationAction()
    {
        Vector3 spawnPos = transform.position;

        GameObject explosionInstance = explosion.GetBody();
        GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);

        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(empExplosion);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (timer < changeToLoopAnimTime) { return; }

        if (other.GetComponent<Player>())
        {
            InstallationAction();
            Destroy(gameObject);
        }
    }
}