using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TitleButtons : MonoBehaviour
{
    TitleCanvasBehavior parent;

    int selectNum = 0;
    const int selectItemNum = 5;

    TitleButtonBehavior[] buttons=new TitleButtonBehavior[selectItemNum];

    public void SetParent(TitleCanvasBehavior _parent) { parent = _parent; }

    void Start()
    {
        for(int i = 0; i < selectItemNum; i++)
        {
            buttons[i] = transform.GetChild(i).GetComponent<TitleButtonBehavior>();
            buttons[i].SetGroupParent(this,i);
        }

        buttons[selectNum].SetState(1);
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
            buttons[selectNum].SetState(0);
            if (value.x < 0) { selectNum = (selectNum + selectItemNum - 1) % selectItemNum; }
            else { selectNum = (selectNum + 1) % selectItemNum; }
            buttons[selectNum].SetState(1);
            Managers.instance.PlaySFXForUI(2);
        }
    }

    public void CursorMoveFromTouch(int _num)
    {
        buttons[selectNum].SetState(0);

        selectNum = _num;
        for (int i = 0; i < selectItemNum; i++)
        {
            int num = 0;
            if (_num == i) { num = 1; }
            buttons[i].SetState(num);
            Managers.instance.PlaySFXForUI(2);
        }
    }

    void DecideSelect()
    {
        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            Managers.instance.PlaySFXForUI(3);
            parent.ChangeFromButtons(selectNum);
        }
    }
    public void DecideSelectFromTouch()
    {
        Managers.instance.PlaySFXForUI(3);
        parent.ChangeFromButtons(selectNum);
    }
    void PressCancel()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(0);
            selectNum = selectItemNum - 1;
            transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(1);
            Managers.instance.PlaySFXForUI(2);
        }
    }
}
