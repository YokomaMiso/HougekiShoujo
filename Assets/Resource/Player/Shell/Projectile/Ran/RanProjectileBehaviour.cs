using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class RanProjectileBehaviour : ProjectileBehavior
{
    int state = 0;
    const int maxSpawnCount = 2;
    const float firstSpawn = 0.6f;
    const float spawnInterval = 0.3f;

    Vector3 inputVector;

    readonly float[] forwardValue = new float[maxSpawnCount] { 2.25f, 0.25f };

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
        Vector3 forwardVector = inputVector.normalized * forwardValue[state];

        Vector3 spawnPos = transform.position + forwardVector;
        GameObject explosionInstance = explosion.GetBody();
        GameObject obj = Instantiate(explosionInstance, spawnPos, Quaternion.identity);
        obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
        obj.GetComponent<ExplosionBehavior>().SetData(explosion);

        state++;
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        timer += deltaTime;

        if (timer >= lifeTime) { Destroy(gameObject); }

        if (state >= maxSpawnCount) { return; }

        if (timer >= firstSpawn + spawnInterval * state) { SpawnExplosion(); }
    }
}

