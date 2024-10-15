using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CautionTapeBehavior : MonoBehaviour
{
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 endPos;
    float posTimer;
    [SerializeField] float arriveTime = 0.5f;

    RawImage image;
    float uvTimer = 0;
    const float resetTime = 1.0f;

    bool destroy;
    Vector3 destroyPos;
    float destroyTimer;
    const float destroyTime = 0.5f;

    public void SetDestroy() { destroy = true; }

    void Start()
    {
        transform.localPosition = startPos;
        image = GetComponent<RawImage>();
        destroyPos = endPos + endPos - startPos;
    }

    void Update()
    {
        PositionUpdate();
        DestroyPositionUpdate();

        uvTimer += Time.deltaTime;
        if (uvTimer > resetTime) { uvTimer -= resetTime; }

        var rect = image.uvRect;
        rect.x = uvTimer;
        image.uvRect = rect;
    }

    void PositionUpdate()
    {
        if (posTimer > arriveTime) { return; }

        posTimer += Time.deltaTime;
        if (posTimer > arriveTime)
        {
            transform.localPosition = endPos;
            return;
        }

        float nowRate = Mathf.Sqrt(posTimer / arriveTime);
        transform.localPosition = Vector3.Lerp(startPos, endPos, nowRate);
    }

    void DestroyPositionUpdate()
    {
        if (!destroy) { return; }

        destroyTimer += Time.deltaTime;
        if (destroyTimer > destroyTime)
        {
            transform.localPosition = destroyPos;
            return;
        }

        float nowRate = Mathf.Pow(destroyTimer / destroyTime, 2);
        transform.localPosition = Vector3.Lerp(endPos, destroyPos, nowRate);
    }
}
