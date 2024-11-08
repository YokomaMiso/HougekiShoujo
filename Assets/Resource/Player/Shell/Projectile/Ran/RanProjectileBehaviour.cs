using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class RanProjectileBehaviour : ProjectileBehavior
{
    int state = 0;
    const float firstSpawn = 0.6f;
    //const float spawnInterval = 0.15f;

    Vector3 inputVector;

    protected override void Start()
    {
        base.Start();
        inputVector = ownerPlayer.GetInputVector().normalized;
        if (inputVector == Vector3.zero) { inputVector = Vector3.forward; }
    }

    public override void SetData(Shell _data)
    {
        lifeTime = _data.GetLifeTime();
        explosion = _data.GetExplosion();
    }

    protected override void SpawnExplosion()
    {
        Vector3[] offsetSide = new Vector3[2] { -transform.right, transform.right };

        for (int i = 0; i < 2; i++)
        {
            Vector3 forwardVector = inputVector.normalized * explosion.GetScale() * 1.5f;
            float explosionScale = explosion.GetScale();
            Vector3 offset = offsetSide[i] * explosionScale;
            Vector3 addPos = offset + forwardVector;

            Vector3 spawnPos = transform.position + addPos;
            GameObject explosionInstance = explosion.GetBody();
            GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
            obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
            obj.GetComponent<ExplosionBehavior>().SetData(explosion);
        }
        state++;
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        timer += deltaTime;

        if (timer >= firstSpawn) { SpawnExplosion(); }

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}

