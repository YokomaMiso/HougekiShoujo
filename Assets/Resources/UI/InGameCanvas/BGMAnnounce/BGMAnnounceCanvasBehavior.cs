using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMAnnounceCanvasBehavior : MonoBehaviour
{
    Transform label;
    readonly Vector3 labelStartPos = new Vector3(-1360, -420);
    readonly Vector3 labelEndPos = new Vector3(-520, -420);

    float timer;
    const float waitTime = 0.5f;
    const float arriveTime = 1.0f;
    const float startBackTime = 3.5f;
    const float lifeTime = 4.0f;

    [SerializeField] Image record;

    public void SetBGMText(BGMData _bgmData)
    {
        label = transform.GetChild(0);

        label.GetChild(0).GetComponent<Text>().text = "ÅÙ" + _bgmData.GetBGMTitle();

        label.localPosition = labelStartPos;
    }
    void Update()
    {
        record.transform.localRotation = Quaternion.Euler(0f, 0f, Time.time * -360);

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
