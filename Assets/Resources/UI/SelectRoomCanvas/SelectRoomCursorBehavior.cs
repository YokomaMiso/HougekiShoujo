using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoomCursorBehavior : MonoBehaviour
{
    Transform arrow;
    float value;

    void Start() { arrow = transform.GetChild(0); }

    void Update()
    {
        value = Mathf.Sin(Time.time * 10) * 24;
        arrow.localPosition = Vector2.right * value;
    }

    public void SetPosition(RectTransform _object)
    {
        Vector3 sizeDelta = _object.sizeDelta/100;
        sizeDelta.y = 0;

        transform.position = _object.position - sizeDelta/2;
        //transform.position = _object.position;
    }
}
