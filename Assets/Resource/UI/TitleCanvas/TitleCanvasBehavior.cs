using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TITLE_STATE { STAY = -1, SELECT = 0, INPUT_NAME, CREDIT, CHANGE_TO_CONNECTION };

public class TitleCanvasBehavior : MonoBehaviour
{
    [SerializeField, Header("決定音")] AudioClip submitSFX;

    [SerializeField, Header("TitleBGM イントロ")] AudioClip titleBGMIntro;
    [SerializeField, Header("TitleBGM ループ")] AudioClip titleBGMLoop;

    [SerializeField] GameObject buttons;
    //[SerializeField] GameObject selectNetwork;
    [SerializeField] GameObject inputName;
    [SerializeField] GameObject creditCanvas;
    GameObject[] uis = new GameObject[3];

    TITLE_STATE state = TITLE_STATE.SELECT;
    public TITLE_STATE GetTitleState() { return state; }

    float titleCallTimer;
    const float titleCallTime = 0.5f;
    bool titleCalled;

    void Start()
    {
        uis[(int)TITLE_STATE.SELECT] = Instantiate(buttons, transform);
        uis[(int)TITLE_STATE.SELECT].GetComponent<TitleButtons>().SetParent(this);
        //uis[1] = Instantiate(selectNetwork, transform);
        //uis[1].GetComponent<SelectNetwork>().SetParent(this);
        uis[(int)TITLE_STATE.INPUT_NAME] = Instantiate(inputName, transform);
        uis[(int)TITLE_STATE.INPUT_NAME].GetComponent<InputName>().SetParent(this);
        uis[(int)TITLE_STATE.CREDIT] = Instantiate(creditCanvas, transform);
        uis[(int)TITLE_STATE.CREDIT].GetComponent<CreditCanvasBehavior>().SetParent(this);
        UIsUpdate();
        SoundManager.PlayBGMIntro(titleBGMIntro, titleBGMLoop);
    }

    void Update()
    {
        if (titleCalled) { return; }
        titleCallTimer += Time.deltaTime;
        if (titleCallTimer > titleCallTime)
        {
            //int characterID = Random.Range(0,Managers.instance.gameManager.playerDatas.Length);
            int characterID = 0;
            SoundManager.PlayVoice(Managers.instance.gameManager.playerDatas[characterID].GetPlayerVoiceData().GetTitleCall());
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
                ChangeTitleState(TITLE_STATE.INPUT_NAME);
                break;
            case 1:
                Managers.instance.CreateOptionCanvas();
                break;
            case 2:
                break;
            case 3:
                ChangeTitleState(TITLE_STATE.CREDIT);
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


