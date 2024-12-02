using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkShadowBehavior : MonoBehaviour
{
    SpriteRenderer sr;

    public static float timeSub;

    int teamNum = 0;

    float timer;
    const float lifeTime = 0.8f;
    public void SetTime(int _num) { timer = timeSub * _num; }
    public void SetTeamNum(int _num) { teamNum = _num; }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.material = Managers.instance.gameManager.spriteDefaultMat;

        float nowRate = timer / lifeTime;
        float alpha = (1.0f - nowRate) * 0.8f;

        Color color;
        if (teamNum == 0) { color = new Color(0.4f, 0.4f, 1.0f, alpha); }
        else { color = new Color(1.0f, 0.4f, 0.4f, alpha); }
        sr.color = color;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float nowRate = timer / lifeTime;
        float alpha = (1.0f - nowRate) * 0.8f;
        
        Color color;
        if (teamNum == 0) { color = new Color(0.4f, 0.4f, 1.0f, alpha); }
        else { color = new Color(1.0f, 0.4f, 0.4f, alpha); }
        sr.color = color;

        if (timer > lifeTime) { Destroy(gameObject); }
    }
}
