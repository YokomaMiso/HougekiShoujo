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
    }

    void CursorMove()
    {
        float value = Input.GetAxis("Horizontal");

        //カーソル移動
        if (Mathf.Abs(value) > 0.7f)
        {
            if (isCanSelect)
            {
                transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(0);
                if (value < 0) { selectNum = (selectNum + selectItemNum - 1) % selectItemNum; }
                else { selectNum = (selectNum + 1) % selectItemNum; }
                transform.GetChild(selectNum).GetComponent<TitleButtonBehavior>().SetState(1);
                isCanSelect = false;
            }
        }
        //前フレームの情報保存
        else if (Mathf.Abs(value) < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void DecideSelect()
    {
        if (Input.GetButtonDown("Submit"))
        {
            parent.ChangeFromButtons(selectNum);
        }
    }
}
