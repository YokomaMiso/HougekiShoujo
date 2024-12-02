using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkShadowBehavior : MonoBehaviour
{
    SpriteRenderer sr;

    public static float timeSub;

    float timer;
    const float lifeTime = 0.8f;

    public void SetTime(int _num) { timer = timeSub * _num; }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.material = Managers.instance.gameManager.spriteDefaultMat;

        float nowRate = timer / lifeTime;
        float alpha = (1.0f - nowRate) * 0.8f;
        sr.color = new Color(0.4f, 0.4f, 1.0f, alpha);
    }

    void Update()
    {
        timer += Time.deltaTime;

        float nowRate = timer / lifeTime;
        float alpha = (1.0f - nowRate) * 0.8f;
        sr.color = new Color(0.4f, 0.4f, 1.0f, alpha);
        if (timer > lifeTime) { Destroy(gameObject); }
    }
}
