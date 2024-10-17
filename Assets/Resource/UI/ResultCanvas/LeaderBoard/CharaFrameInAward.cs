using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaFrameInAward : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    Vector3 destroyPos;

    float timer;
    const float arriveTime = 0.5f;
    const float moveFromArriveTime = 5.5f;
    const float lifeTime = 6.0f;

    readonly Vector3[] nameBorderPos = new Vector3[2] { new Vector3(100, -845), new Vector3(100,-560) };

    GameObject nameBorderInstance;
    GameObject nameBorderPrefab;
    ResultScoreBoard.KDFData kdf;
    PlayerData pd;

    public void SetPos(Vector3 _start, Vector3 _end)
    {
        startPos = _start;
        endPos = _end;
    }

    public void SpawnNameBorder(GameObject _nameBorder, ResultScoreBoard.KDFData _kdf, PlayerData _pd)
    {
        nameBorderPrefab = _nameBorder;
        kdf = _kdf;
        pd = _pd;
    }

    void Start()
    {
        transform.localPosition = startPos;
        destroyPos = endPos * 3;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < moveFromArriveTime)
        {
            float nowRate = Mathf.Pow(Mathf.Clamp01(timer / arriveTime), 2);
            transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
        }
        else
        {


            float nowRate = Mathf.Sqrt(Mathf.Clamp01((timer - moveFromArriveTime) / (lifeTime - moveFromArriveTime)));
            transform.localPosition = Vector3.Lerp(endPos, destroyPos, nowRate);
        }

        if (timer > arriveTime)
        {
            if (nameBorderInstance == null)
            {

                nameBorderInstance = Instantiate(nameBorderPrefab, transform);
                nameBorderInstance.GetComponent<NameBorder>().SetPos(nameBorderPos[0], nameBorderPos[1]);
                nameBorderInstance.GetComponent<NameBorder>().SetData(kdf, pd);
            }
        }
    }
}
