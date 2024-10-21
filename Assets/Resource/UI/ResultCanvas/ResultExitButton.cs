using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultExitButton : MonoBehaviour
{
    ResultCanvasBehavior ownerCanvas;
    public void SetOwnerCanvas(ResultCanvasBehavior _owner) { ownerCanvas = _owner; }

    float timer;
    const float canSubmitTime = 19.0f;

    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        text.color = Color.clear;
    }

    void Update()
    {
        if (timer <= canSubmitTime) { timer += Time.deltaTime; }
        else
        {
            text.color = Color.black * Mathf.Cos(Time.time * 2);

            if (Managers.instance.UsingCanvas()) { return; }

            if (Input.GetButtonDown("Submit")) { ownerCanvas.ReturnRoom(); }
        }
    }
}
