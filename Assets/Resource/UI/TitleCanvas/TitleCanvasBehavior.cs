using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
public class TitleCanvasBehavior : MonoBehaviour
{
    GameManager gameManager;
    public void SetGameManager(GameManager _gameManager) { gameManager = _gameManager; }

    int selectNum = 0;
    bool isCanSelect = true;

    void Update()
    {
        CursorMove();
        CursorDisplay();
        DecideSelect();
    }

    void CursorMove()
    {
        float value = Input.GetAxis("Vertical");

        //�J�[�\���ړ�
        if (Mathf.Abs(value) > 0.7f)
        {
            if (isCanSelect)
            {
                if (value > 0) { selectNum = 0; }
                else { selectNum = 1; }
                isCanSelect = false;
            }
        }
        //�O�t���[���̏��ۑ�
        else if (Mathf.Abs(value) < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void CursorDisplay()
    {
        for (int i = 0; i < 2; i++)
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
            switch (selectNum)
            {
                case 0:
                    Managers.instance.ChangeState(GAME_STATE.ROOM);
                    Managers.instance.ChangeScene(GAME_STATE.ROOM);
                    Destroy(gameObject);
                    break;
                case 1:
                    Managers.instance.ChangeState(GAME_STATE.OPTION);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
