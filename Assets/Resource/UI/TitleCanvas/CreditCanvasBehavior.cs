using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditCanvasBehavior : MonoBehaviour
{
    TitleCanvasBehavior parent;
    public void SetParent(TitleCanvasBehavior _parent) { parent = _parent; }

    RectTransform creditTexts;
    const float limit = 1620;

    void Start()
    {
        creditTexts = transform.GetChild(0).GetComponent<RectTransform>();
    }

    void Update()
    {
        if (InputManager.GetKeyDown(BoolActions.SouthButton) || InputManager.GetKeyDown(BoolActions.EastButton)) { parent.ChangeTitleState(TITLE_STATE.SELECT); }

        float movement = InputManager.GetAxis(Vec2AxisActions.LStickAxis).y * Time.deltaTime * 800;
        float nowPosY = Mathf.Clamp(creditTexts.localPosition.y - movement, -limit, limit);

        creditTexts.localPosition = Vector3.up * nowPosY;
    }
}
