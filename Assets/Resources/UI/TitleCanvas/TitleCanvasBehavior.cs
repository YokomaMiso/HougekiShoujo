using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TITLE_STATE { STAY = -1, SELECT = 0, TUTORIAL, INPUT_NAME, CREDIT, CHANGE_TO_CONNECTION };

public class TitleCanvasBehavior : MonoBehaviour
{
    [SerializeField, Header("TitleBGM")] AudioClip titleBGM;

    [SerializeField] GameObject buttons;
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject inputName;
    GameObject[] uis = new GameObject[3];

    TITLE_STATE state = TITLE_STATE.SELECT;
    public TITLE_STATE GetTitleState() { return state; }

    float titleCallTimer;
    const float titleCallTime = 0.7f;
    bool titleCalled;

    void Start()
    {
        uis[(int)TITLE_STATE.SELECT] = Instantiate(buttons, transform);
        uis[(int)TITLE_STATE.SELECT].GetComponent<TitleButtons>().SetParent(this);
        uis[(int)TITLE_STATE.TUTORIAL] = Instantiate(tutorial, transform);
        uis[(int)TITLE_STATE.TUTORIAL].GetComponent<TutorialWindow>().SetParent(this);
        uis[(int)TITLE_STATE.INPUT_NAME] = Instantiate(inputName, transform);
        uis[(int)TITLE_STATE.INPUT_NAME].GetComponent<InputName>().SetParent(this);
        //uis[(int)TITLE_STATE.CREDIT] = Instantiate(creditCanvas, transform);
        //uis[(int)TITLE_STATE.CREDIT].GetComponent<CreditCanvasBehavior>().SetParent(this);
        UIsUpdate();
        SoundManager.PlayBGM(titleBGM);
    }

    void Update()
    {
        if (titleCalled) { return; }
        titleCallTimer += Time.deltaTime;
        if (titleCallTimer > titleCallTime)
        {
            int characterID;
            if (Managers.instance.unlockFlag[(int)UNLOCK_ITEM.TSUBASA]) { characterID = Random.Range(0, Managers.instance.gameManager.playerDatas.Length); }
            else {characterID = Random.Range(0, Managers.instance.gameManager.playerDatas.Length - 1); }

            SoundManager.PlayVoiceForUI(Managers.instance.gameManager.playerDatas[characterID].GetPlayerVoiceData().GetTitleCall(), Managers.instance.transform);
            titleCalled = true;
        }
    }

    public void ChangeTitleState(TITLE_STATE _state)
    {
        if (state == _state) { return; }

        state = _state;
        switch (_state)
        {
            case TITLE_STATE.SELECT:
            case TITLE_STATE.TUTORIAL:
            case TITLE_STATE.INPUT_NAME:
            case TITLE_STATE.CREDIT:
                UIsUpdate();
                break;
            case TITLE_STATE.CHANGE_TO_CONNECTION:
                Managers.instance.ChangeState(GAME_STATE.SELECT_ROOM);
                Managers.instance.ChangeScene(GAME_STATE.SELECT_ROOM);
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
                if (Managers.instance.optionData.displayTutorial) { ChangeTitleState(TITLE_STATE.TUTORIAL); }
                else { ChangeTitleState(TITLE_STATE.INPUT_NAME); }
                break;
            case 1:
                Managers.instance.CreateOptionCanvas();
                break;
            case 2:
                Managers.instance.ChangeState(GAME_STATE.GALLERY);
                Managers.instance.ChangeScene(GAME_STATE.GALLERY);
                break;
            case 3:
                Managers.instance.ChangeState(GAME_STATE.CREDIT);
                Managers.instance.ChangeScene(GAME_STATE.CREDIT);
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


