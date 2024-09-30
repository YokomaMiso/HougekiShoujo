using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoSplashCanvasBehavior : MonoBehaviour
{
    Image teamLogo;

    const float fadeInStart = 1;
    const float alphaMaxTime = 2;
    const float fadeOutStart = 4;
    const float splashSceneLifeTime = 6;

    float timer = 0;

    void Start()
    {
        teamLogo = transform.GetChild(1).GetComponent<Image>();
        teamLogo.color = Color.clear;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= fadeInStart)
        {
            //none
        }
        else if (timer <= alphaMaxTime)
        {
            float rate = Mathf.Clamp01(timer - fadeInStart);
            teamLogo.color = new Color(1, 1, 1, rate);
        }
        else if (timer <= fadeOutStart) { teamLogo.color = Color.white; }
        else if (timer <= splashSceneLifeTime)
        {
            //float rate = Mathf.Clamp01(1.0f - (timer - fadeOutStart));
            //teamLogo.color = new Color(1, 1, 1, rate);
        }
        else
        {
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            Managers.instance.ChangeState(GAME_STATE.TITLE);
        }
    }
}
