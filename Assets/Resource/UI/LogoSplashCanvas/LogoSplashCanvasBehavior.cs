using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoSplashCanvasBehavior : MonoBehaviour
{
    Image teamLogo;

    const float fadeInStart = 0.5f;
    const float alphaMaxTime = 1.5f;
    //const float fadeOutStart = 3.5f;
    const float splashSceneLifeTime = 3.5f;

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
        //else if (timer <= fadeOutStart) { teamLogo.color = Color.white; }
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
