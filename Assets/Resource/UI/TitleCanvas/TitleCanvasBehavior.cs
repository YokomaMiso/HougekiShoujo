using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TITLE_STATE { SELECT = 0, INPUT_NAME, CHANGE_TO_ROOM };

public class TitleCanvasBehavior : MonoBehaviour
{
    [SerializeField, Header("決定音")] AudioClip submitSFX;

    [SerializeField] GameObject buttons;
    [SerializeField] GameObject inputName;
    GameObject[] uis = new GameObject[2];

    TITLE_STATE state = TITLE_STATE.SELECT;
    public TITLE_STATE GetTitleState() { return state; }

    void Start()
    {
        uis[0] = Instantiate(buttons,transform);
        uis[0].GetComponent<TitleButtons>().SetParent(this);
        uis[1] = Instantiate(inputName, transform);
        uis[1].GetComponent<InputName>().SetParent(this);
        UIsUpdate();
    }

    void Update()
    {

    }

    public void ChangeTitleState(TITLE_STATE _state)
    {
        if (state == _state) { return; }

        state = _state;
        switch (_state)
        {
            case TITLE_STATE.SELECT:
            case TITLE_STATE.INPUT_NAME:
                UIsUpdate();
                break;
            case TITLE_STATE.CHANGE_TO_ROOM:
                Managers.instance.ChangeState(GAME_STATE.ROOM);
                Managers.instance.ChangeScene(GAME_STATE.ROOM);
                break;
        }
    }

    void UIsUpdate()
    {
        for (int i = 0; i < uis.Length; i++) { uis[i].SetActive(i == (int)state); }
    }

    public void ChangeFromButtons(int _num)
    {
        switch (_num)
        {
            case 0:
                ChangeTitleState(TITLE_STATE.INPUT_NAME);
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

    public void End()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                    Application.Quit();//ゲームプレイ終了
#endif
    }
}
