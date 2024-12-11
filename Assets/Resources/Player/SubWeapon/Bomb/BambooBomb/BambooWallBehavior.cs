using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BambooWallBehavior : MonoBehaviour
{
    const int bambooCount = 7;
    const float posXSub = 0.16f;
    float defaultPosX;
    const float defaultLocalHeight = -0.5f;
    const float endLocalHeight = 0.5f;

    float timer;
    const float growTime = 0.2f;

    float lifeTime;

    int cnt;
    bool bambooEnable = true;
    BambooBehavior[] bamboos;

    public void SetData(SubWeapon _sub)
    {
        lifeTime = _sub.GetLifeTime();
    }

    public void DeleteBamboo(int _num) { Destroy(bamboos[_num].gameObject); }

    void Start()
    {
        //bambooCount = transform.childCount;
        defaultPosX = posXSub * bambooCount / 2;
        bamboos = new BambooBehavior[bambooCount];
        for (int i = 0; i < bambooCount; i++)
        {
            transform.GetChild(i).rotation = Quaternion.identity;
            if (!transform.GetChild(i).GetComponent<BambooBehavior>()) { continue; }
            bamboos[i] = transform.GetChild(i).GetComponent<BambooBehavior>();
            bamboos[i].SetNum(i);
        }
    }

    void Update()
    {
        timer += Managers.instance.timeManager.GetDeltaTime();

        if (timer > lifeTime) { Destroy(gameObject); }
    }

    void FixedUpdate()
    {
        if (timer <= growTime)
        {
            float nowRate = Mathf.Clamp01(timer / growTime);
            float applyPosY = Mathf.Lerp(defaultLocalHeight, endLocalHeight, nowRate);

            for (int i = 0; i < bambooCount; i++)
            {
                if (bamboos[i] == null) { continue; }

                float posX = defaultPosX - posXSub * i;
                Vector3 applyLocalPos = new Vector3(posX, applyPosY);
                bamboos[i].transform.localPosition = applyLocalPos;
            }

            if (bambooEnable)
            {
                cnt++;
                if (cnt > 1)
                {
                    for (int i = 0; i < bambooCount; i++)
                    {
                        if (bamboos[i] == null) { continue; }
                        bamboos[i].DeleteCollision();
                        bamboos[i].EnableSprite();
                    }
                    bambooEnable = false;
                }
            }
        }
        else if (growTime < timer && timer < lifeTime - growTime)
        {
            if (!bambooEnable)
            {
                for (int i = 0; i < bambooCount; i++)
                {
                    if (bamboos[i] == null) { continue; }
                    bamboos[i].EnableCollision();
                }
                bambooEnable = true;
            }
        }
        else if (lifeTime - growTime <= timer && timer < lifeTime)
        {
            float subTime = timer - (lifeTime - growTime);

            float nowRate = Mathf.Clamp01(subTime / growTime);
            float applyPosY = Mathf.Lerp(endLocalHeight, defaultLocalHeight, nowRate);

            for (int i = 0; i < bambooCount; i++)
            {
                if (bamboos[i] == null) { continue; }

                float posX = defaultPosX - posXSub * i;
                Vector3 applyLocalPos = new Vector3(posX, applyPosY);
                bamboos[i].transform.localPosition = applyLocalPos;
            }

            if (bambooEnable)
            {
                for (int i = 0; i < bambooCount; i++)
                {
                    if (bamboos[i] == null) { continue; }
                    bamboos[i].DeleteCollision();
                }
                bambooEnable = false;
            }
        }
    }
}
