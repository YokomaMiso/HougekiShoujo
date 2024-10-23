using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtons : MonoBehaviour
{
    TitleCanvasBehavior parent;

    int selectNum = 0;
    const int selectItemNum = 5;
    bool isCanSelect = true;

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
        Vector2 value = InputManager.GetAxisDelay<Vector2>(Vec2AxisActions.LStickAxis, 2);

        //カーソル移動
        if (Mathf.Abs(value.x) > 0.7f)
        {
            if (isCanSelect)
            {
                transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(0);
                if (value.x < 0) { selectNum = (selectNum + selectItemNum - 1) % selectItemNum; }
                else { selectNum = (selectNum + 1) % selectItemNum; }
                transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(1);
                isCanSelect = false;
            }
        }
        //前フレームの情報保存
        else if (Mathf.Abs(value.x) < 0.2f)
        {
            isCanSelect = true;
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
            isCanSelect = false;
        }
    }
}
