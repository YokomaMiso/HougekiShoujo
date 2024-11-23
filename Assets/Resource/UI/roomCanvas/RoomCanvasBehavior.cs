using System;
using System.Collections;
using UnityEngine;
using static MachingRoomData;

public enum TEAM_NUM { A = 0, B = 1 };

public class RoomCanvasBehavior : MonoBehaviour
{
    RoomManager rm;

    [SerializeField] CharaVisualInRoomCanvas charaVisual;
    [SerializeField] GameObject playerBanners;
    [SerializeField] StageSelectBehavior stageSelect;

    [SerializeField] GameObject stageBinder;
    [SerializeField] GameObject stageDisplay;

    readonly Vector3[] teamDefaultPos = new Vector3[2] { new Vector3(450, 352), new Vector3(440, -128) };
    readonly Vector3 teamPosSub = new Vector3(30, -80);

    bool joinedStartedRoom;

    [SerializeField] AudioClip roomBGM;

    void Start()
    {
        rm = Managers.instance.roomManager;

        //自分の所属チームを振り分ける
        if (OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID).myTeamNum == -1) { rm.PlayerBannerDivider(); }

        if (Managers.instance.playerID != 0) { joinedStartedRoom = OSCManager.OSCinstance.GetRoomData(0).gameStart; }

        MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
        int charaID = myRoomData.selectedCharacterID;
        PlayerData nowPlayerData = Managers.instance.gameManager.playerDatas[charaID];
        charaVisual.SetCharaVisual(nowPlayerData);

        if (Managers.instance.playerID != 0) { stageBinder.SetActive(false); }
        else { stageDisplay.SetActive(false); }

        SoundManager.PlayBGM(roomBGM);
    }

    void Update()
    {
        if (!Managers.instance.UsingCanvas())
        {
            if (!stageSelect.gameObject.activeInHierarchy)
            {
                CharaSelect();
                TeamSelect();
                PressSubmit();
                PressCancel();
                GameStart();
                OpenOption();
                CheckStageSelectButton();
            }
        }

        PlayerBannerDisplayUpdate();
    }

    void PlayerBannerDisplayUpdate()
    {
        for (int i = 0; i < playerBanners.transform.childCount; i++)
        {
            playerBanners.transform.GetChild(i).gameObject.SetActive(false);
        }

        int[] teamCount = new int[2] { 0, 0 };

        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(i);
            if (oscRoomData.myTeamNum < 0)
            {
                playerBanners.transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }

            playerBanners.transform.GetChild(i).gameObject.SetActive(true);
            playerBanners.transform.GetChild(i).GetComponent<PlayerBannerBehavior>().BannerIconUpdate(oscRoomData);

            Vector3 applyPos = teamDefaultPos[oscRoomData.myTeamNum] + teamPosSub * teamCount[oscRoomData.myTeamNum];
            playerBanners.transform.GetChild(i).transform.localPosition = applyPos;
            teamCount[oscRoomData.myTeamNum]++;
        }
    }

    void TeamSelect()
    {
        if (OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID).ready) { return; }

        float inputValue = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis).y;
        if (Mathf.Abs(inputValue) < 0.9f) { return; }

        int teamID;
        if (inputValue > 0) { teamID = (int)TEAM_NUM.A; }
        else { teamID = (int)TEAM_NUM.B; }

        rm.PlayerBannerChanger(teamID);
    }

    public void TeamSelectFromUI(int _num)
    {
        if (OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID).ready) { return; }
        int teamID = _num;

        rm.PlayerBannerChanger(teamID);
    }

    void CharaSelect()
    {
        float input = InputManager.GetAxisDelay<Vector2>(Vec2AxisActions.LStickAxis, 0.5f).x;
        if (Mathf.Abs(input) >= 0.9f)
        {
            if (input > 0) { rm.CharaSelect(1); }
            else { rm.CharaSelect(-1); }

            MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
            int charaID = myRoomData.selectedCharacterID;
            PlayerData nowPlayerData = Managers.instance.gameManager.playerDatas[charaID];

            charaVisual.SetCharaVisual(nowPlayerData);
        }
    }

    public void CharaSelectFromUI(int _num)
    {
        rm.CharaSelect(_num);

        MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
        int charaID = myRoomData.selectedCharacterID;
        PlayerData nowPlayerData = Managers.instance.gameManager.playerDatas[charaID];

        charaVisual.SetCharaVisual(nowPlayerData);
    }

    void OpenOption()
    {
        RoomData myRoomData = OSCManager.OSCinstance.roomData;
        if (myRoomData.ready) { return; }

        if (InputManager.GetKeyDown(BoolActions.Option))
        {
            Managers.instance.CreateOptionCanvas();
            return;
        }
    }

    void CheckStageSelectButton()
    {
        //そもそもホストじゃないなら
        if (Managers.instance.playerID != 0) { return; }

        if (InputManager.GetKeyDown(BoolActions.LeftShoulder))
        {
            stageSelect.gameObject.SetActive(true);
        }
    }

    public void CheckStageSelectButtonFromUI()
    {
        //そもそもホストじゃないなら
        if (Managers.instance.playerID != 0) { return; }

        stageSelect.gameObject.SetActive(true);
    }

    void PressSubmit()
    {
        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            rm.PressSubmit();
        }
    }
    public void PressFromUI()
    {
        if (Managers.instance.playerID == 0)
        {
            rm.PressSubmit();
        }
        else
        {
            if (OSCManager.OSCinstance.roomData.ready) { rm.PressCancel(); }
            else { rm.PressSubmit(); }
        }
    }
    void PressCancel()
    {
        if (InputManager.GetKeyDown(BoolActions.EastButton))
        {
            RoomData myRoomData = OSCManager.OSCinstance.roomData;
            if (myRoomData.ready) { rm.PressCancel(); }
            else { rm.BackToTitle(); }
        }
    }
    public void PressBackTitle()
    {
        rm.BackToTitle();
    }

    void GameStart()
    {
        RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);

        bool start = hostRoomData.gameStart;
        if (joinedStartedRoom)
        {
            if (!start) { joinedStartedRoom = false; }
            return;
        }

        if (!start) { return; }

        GAME_STATE sendState = GAME_STATE.IN_GAME;

        Managers.instance.ChangeScene(sendState);
        Managers.instance.ChangeState(sendState);
    }
}
