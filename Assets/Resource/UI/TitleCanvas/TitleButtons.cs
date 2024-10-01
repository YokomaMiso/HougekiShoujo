using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtons : MonoBehaviour
{
    TitleCanvasBehavior parent;

    int selectNum = 0;
    const int selectItemNum = 5;
    bool isCanSelect = true;

    GameObject hoverFrame;

    public void SetParent(TitleCanvasBehavior _parent) { parent = _parent; }

    void Start()
    {
        hoverFrame = transform.GetChild(selectItemNum).gameObject;
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
                if (value < 0) { selectNum = (selectNum + selectItemNum - 1) % selectItemNum; }
                else { selectNum = (selectNum + 1) % selectItemNum; }
                hoverFrame.transform.localPosition = transform.GetChild(selectNum).transform.localPosition;
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
