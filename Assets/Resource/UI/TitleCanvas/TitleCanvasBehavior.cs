using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleCanvasBehavior : MonoBehaviour
{
    GameManager gameManager;
    public void SetGameManager(GameManager _gameManager) { gameManager = _gameManager; }

    int selectNum = 0;
    const int selectItemNum = 5;
    bool isCanSelect = true;

    [SerializeField, Header("���艹")] AudioClip submitSFX;

    GameObject hoverFrame;

    void Start()
    {
        hoverFrame = transform.GetChild(selectItemNum).gameObject;    
    }

    void Update()
    {
        if (Managers.instance.UsingOption()) { return; }

        CursorMove();
        DecideSelect();
    }

    void CursorMove()
    {
        float value = Input.GetAxis("Horizontal");

        //�J�[�\���ړ�
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
        //�O�t���[���̏��ۑ�
        else if (Mathf.Abs(value) < 0.2f)
        {
            isCanSelect = true;
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
                    break;
                case 1:
                    Managers.instance.CreateOptionCanvas();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    End();
                    break;
            }
        }
    }

    void End()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
       #else
                    Application.Quit();//�Q�[���v���C�I��
       #endif
    }
}
