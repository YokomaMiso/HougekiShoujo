using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChikaneBlastProjectileBehavior : ProjectileBehavior
{
    int state = 0;
    const int maxState = 3;
    const int spawnCount = 6;
    float[] timeBorder = null;

    int direction = 1;
    Vector3 inputVector;

    protected override void Start()
    {
        base.Start();
        timeBorder = new float[3];
        timeBorder[0] = 0.3f;
        timeBorder[1] = 0.5f;
        timeBorder[2] = 0.7f;

        if (ownerPlayer.NowDirection() < 0) { direction = -1; }
        inputVector = ownerPlayer.GetInputVector().normalized;
        if (inputVector == Vector3.zero) { inputVector = Vector3.forward; }
    }

    public override void SetData(Shell _data)
    {
        lifeTime = _data.GetLifeTime();
        explosion = _data.GetExplosion();
        TagSetting();
    }
    protected override void TimeSetting()
    {
    }

    protected override void SpawnExplosion()
    {
        float spawnAngle = (360.0f / spawnCount) * direction;
        float offsetAngle = Mathf.Atan2(inputVector.x, inputVector.z) - spawnAngle / 2;
        //Vector3 offsetValue = Quaternion.Euler(0, spawnAngle, 0) * inputVector * 2.0f;
        Vector3 offsetValue = inputVector * 2.0f;

        for (int i = 0; i < 2; i++)
        {
            int spawnNum = i + state * 2;

            Vector3 offset = Quaternion.Euler(0, spawnAngle * spawnNum + offsetAngle, 0) * offsetValue;
            Vector3 spawnPos = ownerPlayer.transform.position + offset;
            GameObject explosionInstance = explosion.GetBody();
            GameObject obj = Instantiate(explosionInstance, spawnPos + Vector3.up, Quaternion.identity);
            obj.GetComponent<ExplosionBehavior>().SetPlayer(ownerPlayer);
            obj.GetComponent<ExplosionBehavior>().SetData(explosion);
        }

        state++;
    }

    protected override void Update()
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        timer += deltaTime;

        if (maxState > state && timer > timeBorder[state]) { SpawnExplosion(); }

        if (timer > lifeTime) { Destroy(gameObject); }
    }
}
