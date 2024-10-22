using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditBackGroundBehavior : MonoBehaviour
{
    float timer;
    const float lifeTime = 150;

    readonly Vector3 backStartPos = Vector3.right * 320;
    readonly Vector3 backEndPos = Vector3.left * 320;

    RawImage back;
    RawImage front;

    void Start()
    {
        back = transform.GetChild(0).GetComponent<RawImage>();
        front = transform.GetChild(1).GetComponent<RawImage>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        var frontUVRect = front.uvRect;
        frontUVRect.x = timer/2;
        front.uvRect = frontUVRect;

        back.transform.localPosition = Vector3.Lerp(backStartPos, backEndPos, timer / lifeTime);
    }
}
