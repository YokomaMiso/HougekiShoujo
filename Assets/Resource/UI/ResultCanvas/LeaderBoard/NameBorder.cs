using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameBorder : MonoBehaviour
{
    protected Vector3 startPos;
    protected Vector3 endPos;

    float timer;
    const float arriveTime = 0.75f;
    bool arrive;

    protected Text killCountText;
    protected Outline[] outlines;
    Vector3 killcountTextDefaultPos = new Vector3(-320, 10);
    const float killCountSpawnTime = 1.0f;
    bool spawned;
    const float scaleFixTime = 1.5f;
    const float shakeTime = 1.75f;

    protected Animator charaIdleAnim;

    public void SetPos(Vector3 _start,Vector3 _end)
    {
        startPos = _start;
        endPos = _end;
        transform.localPosition = startPos;
    }

    public virtual void SetData(ResultScoreBoard.KDFData _kdf,PlayerData _pd)
    {
        //プレイヤー名
        transform.GetChild(0).GetComponent<Text>().text = _kdf.playerName;
        //キル数
        killCountText = transform.GetChild(3).GetComponent<Text>();
        killCountText.text = _kdf.killCount.ToString();
        killCountText.color = Color.clear;
        killCountText.transform.localScale = Vector3.one * 5;
        //キルカウントのアウトラインを無効化
        outlines = killCountText.GetComponents<Outline>();
        for (int i = 0; i < outlines.Length; i++) { outlines[i].enabled = false; }

        if (transform.childCount > 4)
        {
            charaIdleAnim = transform.GetChild(4).GetComponent<Animator>();
            charaIdleAnim.runtimeAnimatorController = _pd.GetCharacterAnimData().GetIdleAnimForUI();
        }

        transform.localPosition = startPos;
    }
    void Update()
    {
        timer += Time.deltaTime;

        PositionUpdate();
        KillCountBehavior();
    }

    void PositionUpdate()
    {
        if (arrive) { return; }

        float nowRate = Mathf.Sqrt(timer / arriveTime);
        if (timer > arriveTime) { nowRate = 1.0f; arrive = true; }
        transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
    }

    void KillCountBehavior()
    {
        if (timer < killCountSpawnTime) { return; }

        if (!spawned)
        {
            killCountText.color = Color.white;
            for (int i = 0; i < outlines.Length; i++) { outlines[i].enabled = true; }
            spawned = true;
        }

        if (killCountSpawnTime < timer && timer <= scaleFixTime)
        {
            float nowRate = Mathf.Sqrt((timer - killCountSpawnTime) / (scaleFixTime - killCountSpawnTime));
            killCountText.transform.localScale = Vector3.Lerp(Vector3.one * 10, Vector3.one, nowRate);
        }
        else if (scaleFixTime < timer && timer <= shakeTime)
        {
            float nowRate = 1.0f - ((timer - scaleFixTime) / (shakeTime - scaleFixTime));

            killCountText.transform.localScale = Vector3.one;
            killCountText.transform.localPosition = killcountTextDefaultPos + new Vector3(Random.Range(-nowRate, nowRate), Random.Range(-nowRate, nowRate), 0) * 32;
        }
        else
        {
            killCountText.transform.localScale = Vector3.one;
            killCountText.transform.localPosition = killcountTextDefaultPos;
        }
    }
}
