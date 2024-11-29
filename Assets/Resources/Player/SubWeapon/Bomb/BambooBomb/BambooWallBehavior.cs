using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooWallBehavior : MonoBehaviour
{
    const float posXSub = 0.16f;
    const float defaultPosX = posXSub * 2;
    const float defaultLocalHeight = -0.5f;
    const float endLocalHeight = 0.5f;

    float timer;
    const float growTime = 0.2f;

    float lifeTime;

    public void SetData(SubWeapon _sub)
    {
        lifeTime = _sub.GetLifeTime();
    }

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++) { transform.GetChild(i).rotation = Quaternion.identity; }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= growTime + 0.2f)
        {
            float nowRate = Mathf.Clamp01(timer / growTime);
            float applyPosY = Mathf.Lerp(defaultLocalHeight, endLocalHeight, nowRate);

            for (int i = 0; i < transform.childCount; i++)
            {
                float posX = defaultPosX - posXSub * i;
                Vector3 applyLocalPos = new Vector3(posX, applyPosY);
                transform.GetChild(i).localPosition = applyLocalPos;
            }
        }
        if (lifeTime - growTime <= timer && timer <= lifeTime)
        {
            float subTime = lifeTime - growTime;

            float nowRate = Mathf.Clamp01(timer - subTime / lifeTime - subTime);
            float applyPosY = Mathf.Lerp(endLocalHeight, defaultLocalHeight, nowRate);

            for (int i = 0; i < transform.childCount; i++)
            {
                float posX = defaultPosX - posXSub * i;
                Vector3 applyLocalPos = new Vector3(posX, applyPosY);
                transform.GetChild(i).localPosition = applyLocalPos;
            }
        }

        if (timer > lifeTime) { Destroy(gameObject); }
    }
}
