using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultExitButton : MonoBehaviour
{
    ResultCanvasBehavior ownerCanvas;
    public void SetOwnerCanvas(ResultCanvasBehavior _owner) { ownerCanvas = _owner; }

    float timer;
    const float canSubmitTime = 17.0f;

    Text text;

    float alphaTimer;
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
            alphaTimer += Time.deltaTime;
            if (alphaTimer > Mathf.PI) { alphaTimer -= Mathf.PI; }

            float nowRate = Mathf.Abs(Mathf.Sin(alphaTimer));
            text.color = Color.black * nowRate;

            if (Managers.instance.UsingCanvas()) { return; }

            if (Input.GetButtonDown("Submit")) { ownerCanvas.ReturnRoom(); }
        }
    }
}
