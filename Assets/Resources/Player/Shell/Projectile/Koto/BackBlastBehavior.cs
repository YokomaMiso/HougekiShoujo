using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlastBehavior : MonoBehaviour
{
    [SerializeField] Explosion backBlastExplosion;
    const float backBlastTime = 0.25f;
    float startTimer;

    protected Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    void Update()
    {
        startTimer += Managers.instance.timeManager.GetDeltaTime();
        if (startTimer >= backBlastTime)
        {
            startTimer = backBlastTime;
            SpawnBackBlast();
            Destroy(gameObject);
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
}
