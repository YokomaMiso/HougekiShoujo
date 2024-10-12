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
    float timer = 0;
    const float resetTime = 1.0f;

    void Start()
    {
        transform.localPosition = startPos;
        image = GetComponent<RawImage>();
    }

    void Update()
    {
        PositionUpdate();

        timer += Time.deltaTime;
        if (timer > resetTime) { timer -= resetTime; }

        var rect = image.uvRect;
        rect.x = timer;
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
}
