using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCanvasBehavior : MonoBehaviour
{
    int selectNum = 0;
    bool isCanSelect = true;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            if (SaveManager.CheckLoadData(i) == 1)
            {
                transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "データあるよ";
            }
        }
    }

    void Update()
    {
        CursorMove();
        CursorDisplay();
        DecideSelect();
    }


    void CursorMove()
    {
        float value = Input.GetAxis("Vertical");

        //カーソル移動
        if (Mathf.Abs(value) > 0.7f)
        {
            if (isCanSelect)
            {
                if (value > 0) { selectNum -= 1; }
                else { selectNum += 1; }

                if (selectNum < 0) { selectNum = 0; }
                if (selectNum > 3) { selectNum = 3; }

                isCanSelect = false;
            }
        }
        //前フレームの情報保存
        else if (Mathf.Abs(value) < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void CursorDisplay()
    {
        for (int i = 0; i < 4; i++)
        {
            Color color = Color.white;
            if (i == selectNum) { color = Color.yellow; }
            transform.GetChild(i).GetComponent<Image>().color = color;
        }
    }
    void DecideSelect()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (selectNum < 3)
            {
                SaveManager.LoadPlayerData(selectNum);
                GameManager.ChangeState(GAME_STATE.IN_GAME);
                Destroy(gameObject);
            }
            else
            {
                GameManager.ChangeState(GAME_STATE.TITLE);
                Destroy(gameObject);
            }
        }
    }
}
