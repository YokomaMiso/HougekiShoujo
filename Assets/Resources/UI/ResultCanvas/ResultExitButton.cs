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
    Image button;

    float alphaTimer;

    readonly string[] pressText = new string[2]
    {
        " Press\n",
        " Press Here\n",
    };

    void Start()
    {
        text = GetComponent<Text>();
        text.color = Color.clear;

        button = transform.GetChild(0).GetComponent<Image>();
        button.color = Color.clear;

        button.sprite = InputManager.nowButtonSpriteData.GetSubmit();

        if (Managers.instance.GetSmartPhoneFlag()) { text.text = pressText[1]; }
        else { text.text = pressText[0]; }
    }

    void Update()
    {
        if (InputManager.isChangedController)
        {
            if (Managers.instance.GetSmartPhoneFlag()) { button.sprite = InputManager.nowButtonSpriteData.GetSubmit(); }
        }

        if (timer <= canSubmitTime) { timer += Time.deltaTime; }
        else
        {
            alphaTimer += Time.deltaTime;
            if (alphaTimer > Mathf.PI) { alphaTimer -= Mathf.PI; }

            float nowRate = Mathf.Abs(Mathf.Sin(alphaTimer));
            text.color = Color.black * nowRate;
            if (Managers.instance.GetSmartPhoneFlag()) { button.color = Color.clear; }
            else { button.color = new Color(1, 1, 1, nowRate); }

            if (Managers.instance.UsingCanvas()) { return; }

            if (InputManager.GetKeyDown(BoolActions.SouthButton)) { ownerCanvas.ReturnRoom(); }
        }
    }
}
