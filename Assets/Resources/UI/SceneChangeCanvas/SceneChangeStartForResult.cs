using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeStartForResult : SceneChange
{
    readonly Vector3 newsPaperStartPos = new Vector3(2048, 0);
    readonly Vector3 newsPaperArrivePos = new Vector3(0, 0);
    readonly Vector3 newsPaperEndPos = new Vector3(0, 300);
    readonly Vector3 newsPaperDefaultScale = Vector3.one;
    readonly Vector3 newsPaperEndScale = Vector3.one * 3;

    Transform newsPaper;

    float newsPaperMoveTime;
    float newsPaperStopTime;
    float newsPaperZoomStartTime;

    [SerializeField] AudioClip paperStartSFX;
    [SerializeField] AudioClip paperInSFX;
    bool paperIn;

    protected override void Start()
    {
        lifeTime = 3.0f;
        newsPaperMoveTime = lifeTime - 1.5f;
        newsPaperStopTime = lifeTime - 1.0f;
        newsPaperZoomStartTime = lifeTime - 0.25f;
        newsPaper = transform.GetChild(1);

        SoundManager.PlaySFXForUI(paperStartSFX);
    }

    void Update()
    {
        TimerUpdate();
        float nowRate;

        if (timer >= newsPaperZoomStartTime)
        {
            nowRate = Mathf.Pow((timer - newsPaperZoomStartTime) / (lifeTime - newsPaperZoomStartTime), 3);
            newsPaper.localPosition = Vector3.Lerp(newsPaperArrivePos, newsPaperEndPos, nowRate);
            newsPaper.localScale = Vector3.Lerp(newsPaperDefaultScale, newsPaperEndScale, nowRate);
        }
        else if (newsPaperStopTime >= timer && timer > newsPaperMoveTime)
        {
            if (!paperIn) 
            {
                SoundManager.PlaySFXForUI(paperInSFX,Managers.instance.transform);
                paperIn = true;
            }
            nowRate = Mathf.Pow((timer - newsPaperMoveTime) / (newsPaperStopTime - newsPaperMoveTime), 2);
            newsPaper.localPosition = Vector3.Lerp(newsPaperStartPos, newsPaperArrivePos, nowRate);
        }

        DestroyCheck(true);
    }
}
