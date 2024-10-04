using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class NewCharBehaviourScript : ProjectileBehavior
{

    const int spawnCount = 3;
    static int nowCount = 0;
    const float spawnInterval = 0.5f;


    Vector3 inputVector;
    float explosionSpacing = 2.5f;

    protected override void Start()
    {
        base.Start();
        inputVector = ownerPlayer.GetInputVector().normalized;
        if (inputVector == Vector3.zero) { inputVector = Vector3.forward; }
        nowCount = 0;
        StartCoroutine(ExecuteRepeatedly());
        

    }

    public override void SetData(Shell _data)
    {
        lifeTime = _data.GetLifeTime();
        explosion = _data.GetExplosion();
    }


    protected override void SpawnExplosion()
    {
        Vector3 forwardVector = inputVector;
        forwardVector.Normalize();
        Vector3 offset = forwardVector*0.5f;

        Vector3 spawnPos = transform.position+offset + forwardVector * explosionSpacing*nowCount;
        GameObject explosionInstance = explosion.GetBody();
        GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);
        nowCount++;
    }

    IEnumerator ExecuteRepeatedly()
    {
        while (nowCount<spawnCount)
        {
            SpawnExplosion();

            yield return new WaitForSeconds(spawnInterval);
        }
        Destroy(gameObject);
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        timer += deltaTime;

    }
}

