using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlastCanonBehavior : ProjectileBehavior
{
    [SerializeField] Explosion backBlastExplosion;
    const float backBlastTime = 0.3f;
    float startTimer;

    protected override void Start()
    {
        base.Start();
        imageAnimator.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        if (startTimer < backBlastTime)
        {
            startTimer += Managers.instance.timeManager.GetDeltaTime();
            if (startTimer >= backBlastTime)
            {
                startTimer = backBlastTime;
                SpawnBackBlast();
            }
        }

        if (startTimer > backBlastTime / 2)
        {
            base.Update();
            imageAnimator.gameObject.SetActive(true);
            transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
        }
    }

    void SpawnBackBlast()
    {
        float explosionRadius = backBlastExplosion.GetScale() + 0.25f;
        Vector3 spawnPos = ownerPlayer.transform.position - transform.forward * explosionRadius;

        GameObject obj = Instantiate(backBlastExplosion.GetBody(), spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(backBlastExplosion);
        float angle = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;
        obj.transform.GetChild(0).GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * angle;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (startTimer < backBlastTime / 2) { return; }

        if (other.tag == playerTag)
        {
            if (other.GetComponent<Player>() != ownerPlayer)
            {
                SpawnExplosion();
                Destroy(gameObject);
            }
        }
        else if (other.tag == groundTag)
        {
            SpawnExplosion();
            Destroy(gameObject);
        }
    }
}
