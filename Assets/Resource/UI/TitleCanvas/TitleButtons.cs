using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtons : MonoBehaviour
{
    TitleCanvasBehavior parent;

    int selectNum = 0;
    const int selectItemNum = 5;

    public void SetParent(TitleCanvasBehavior _parent) { parent = _parent; }

    void Start()
    {
        transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(1);
    }

    void Update()
    {
        if (Managers.instance.UsingCanvas()) { return; }

        CursorMove();
        DecideSelect();
        PressCancel();
    }

    void CursorMove()
    {
        Vector2 value = InputManager.GetAxisDelay<Vector2>(Vec2AxisActions.LStickAxis, 0.5f);

        //ƒJ[ƒ\ƒ‹ˆÚ“®
        if (Mathf.Abs(value.x) > 0.7f)
        {
            transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(0);
            if (value.x < 0) { selectNum = (selectNum + selectItemNum - 1) % selectItemNum; }
            else { selectNum = (selectNum + 1) % selectItemNum; }
            transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(1);

        }
    }

    void DecideSelect()
    {
        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            parent.ChangeFromButtons(selectNum);
        }
    }
    void PressCancel()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(0);
            selectNum = selectItemNum - 1;
            transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(1);
        }
    }
}
