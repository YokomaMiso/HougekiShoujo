using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChar2BehaviourScript : ProjectileBehavior
{

    public GameObject fragmentPrefab;

    float addAngle = 0;
    static bool hasEmitter = false;

    const int spawnCount = 15;
    static int nowCount = 0;
    const float spawnInterval = 0.1f;

    const float height = 5.0f;

    protected override void Start()
    {
        base.Start();
        if (transform.GetChild(0).localScale.x < 0) { addAngle = 180; }

        hasEmitter = false;
        nowCount = 0;
    }

    protected override void Update()
    {
        if (hasEmitter) { return; }

        float timeRate = timer / lifeTime;

        if (timeRate < 0.5f) { imageAnimator.transform.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * (90 + addAngle); }
        else
        {
            imageAnimator.transform.GetComponent<ObjectBillboard>().FixedAngles = Vector3.forward * -(90 + addAngle);
            if (transform.position.y < height)
            {
                hasEmitter = true;
                StartCoroutine(SplitMissile());
                
            }
        }

        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        TimeSetting();
        timer += deltaTime;

        float horizonTimeRate = timeRate * 2;
        if (horizonTimeRate > 1) { horizonTimeRate = 1; }
        Vector3 currentHorizon = Vector3.Lerp(defaultPosition, targetPoint, timeRate);
        float currentVertical = Mathf.Sin(timeRate * Mathf.PI) * 15;

        transform.position = currentHorizon + Vector3.up * currentVertical;
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }

    protected override void SpawnExplosion()
    {

    }

    IEnumerator SplitMissile()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnFragment();
            yield return new WaitForSeconds(spawnInterval);
        }
        Destroy(gameObject);
    }

    void SpawnFragment()
    {
        Vector3 spawnPos = transform.position;
        GameObject fragment = Instantiate(fragmentPrefab, spawnPos, Quaternion.identity);
        FragmentBehavior fragmentBehavior = fragment.GetComponent<FragmentBehavior>();
        if (fragmentBehavior != null)
        {
            fragmentBehavior.SetExplosionData(explosion.GetBody());
            fragmentBehavior.SetExpData(explosion);
            fragmentBehavior.SetOwnPlayer(ownerPlayer);
        }
    }
}
