using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMAnnounceCanvasBehavior : MonoBehaviour
{
    Transform label;
    readonly Vector3 labelStartPos = new Vector3(-1360, -500);
    readonly Vector3 labelEndPos = new Vector3(-560, -500);

    float timer;
    const float waitTime = 0.5f;
    const float arriveTime = 1.0f;
    const float startBackTime = 3.5f;
    const float lifeTime = 4.0f;

    public void SetBGMText(BGMData _bgmData)
    {
        label = transform.GetChild(0);

        label.GetChild(0).GetComponent<Text>().text = "ÅÙ" + _bgmData.GetBGMTitle();
        label.GetChild(1).GetComponent<Text>().text = _bgmData.GetBGMArtistName();

        label.localPosition = labelStartPos;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < waitTime) { return; }

        else if (timer <= arriveTime)
        {
            float nowRate = (timer - waitTime) / (arriveTime - waitTime);
            label.localPosition = Vector3.Lerp(labelStartPos, labelEndPos, nowRate);
        }
        else if (timer < startBackTime)
        {
            label.localPosition = labelEndPos;
        }
        else if (timer < lifeTime)
        {
            float nowRate = (timer - startBackTime) / (lifeTime - startBackTime);
            label.localPosition = Vector3.Lerp(labelEndPos, labelStartPos, nowRate);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
