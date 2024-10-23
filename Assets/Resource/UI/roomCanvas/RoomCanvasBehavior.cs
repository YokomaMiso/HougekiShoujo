using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MachingRoomData;

public enum TEAM_NUM { A = 0, B = 1 };

public class RoomCanvasBehavior : MonoBehaviour
{
    RoomManager rm;

    GameObject charaSelect;
    GameObject teamsBG;
    GameObject bannerSelecter;
    GameObject playerBanners;

    Image charaVisual;
    Image nameBar;
    Text nameText;
    GameObject difficulity;

    Image rollDisplay;
    Text rollText;

    Image shellIcon;
    Text shellText;

    Image subWeaponIcon;
    Text subWeaponText;

    float timer = 0;
    const float charaChangeTimer = 0.5f;

    readonly int[] teamPosX = new int[2] { -680, 680 };
    readonly int[] bannerPosY = new int[4] { 240, 80, -80, -240 };

    bool joinedStartedRoom;

    void Start()
    {
        rm = Managers.instance.roomManager;

        charaSelect = transform.GetChild(0).gameObject;
        teamsBG = transform.GetChild(1).gameObject;
        bannerSelecter = transform.GetChild(2).gameObject;
        playerBanners = transform.GetChild(3).gameObject;

        charaVisual = charaSelect.transform.GetChild(0).GetComponent<Image>();
        nameBar = charaVisual.transform.GetChild(0).GetComponent<Image>();
        nameText = nameBar.transform.GetChild(0).GetComponent<Text>();
        difficulity = charaVisual.transform.GetChild(3).gameObject;

        rollDisplay = charaVisual.transform.GetChild(1).GetComponent<Image>();
        rollText = charaVisual.transform.GetChild(2).GetComponent<Text>();

        shellIcon = charaSelect.transform.GetChild(1).GetComponent<Image>();
        shellText = shellIcon.transform.GetChild(0).GetComponent<Text>();

        subWeaponIcon = charaSelect.transform.GetChild(2).GetComponent<Image>();
        subWeaponText = subWeaponIcon.transform.GetChild(0).GetComponent<Text>();

        //é©ï™ÇÃèäëÆÉ`Å[ÉÄÇêUÇËï™ÇØÇÈ
        if (OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID).myTeamNum == -1) { rm.PlayerBannerDivider(); }

        if (Managers.instance.playerID != 0)
        {
            joinedStartedRoom = OSCManager.OSCinstance.GetRoomData(0).gameStart;
        }
    }

    void Update()
    {
        if (!Managers.instance.UsingCanvas())
        {
            CharaSelect();
            TeamSelect();
            PressSubmit();
            PressCancel();
            GameStart();
            OpenOption();
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

            Vector3 applyPos = new Vector3(teamPosX[oscRoomData.myTeamNum], bannerPosY[teamCount[oscRoomData.myTeamNum]]);
            playerBanners.transform.GetChild(i).transform.localPosition = applyPos;
            if (i == Managers.instance.playerID) { bannerSelecter.transform.localPosition = applyPos; }
            teamCount[oscRoomData.myTeamNum]++;
        }
    }

    void TeamSelect()
    {
        if (OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID).ready) { return; }

        int teamID = -1;

        if (InputManager.GetKeyDown(BoolActions.LeftShoulder)) { teamID = (int)TEAM_NUM.A; }
        else if (InputManager.GetKeyDown(BoolActions.RightShoulder)) { teamID = (int)TEAM_NUM.B; }

        if (teamID < 0) { return; }

        rm.PlayerBannerChanger(teamID);
    }

    void CharaSelect()
    {
        int myID = Managers.instance.playerID;
        float input = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis).x;

        if (Mathf.Abs(input) > 0.9f)
        {
            if (timer == 0)
            {
                if (input > 0) { rm.CharaSelect(1); }
                else { rm.CharaSelect(-1); }
            }

            timer += Time.deltaTime;
            if (timer > charaChangeTimer) { timer = 0; }
        }
        else if (Mathf.Abs(input) < 0.2f)
        {
            timer = 0;
        }

        int charaID = OSCManager.OSCinstance.GetRoomData(Managers.instance.playerID).selectedCharacterID;
        PlayerData nowPlayerData = Managers.instance.gameManager.playerDatas[charaID];

        CharaDisplayUpdate(nowPlayerData);
        ShellDisplayUpdate(nowPlayerData);
        SubWeaponDisplayUpdate(nowPlayerData);
    }

    void CharaDisplayUpdate(PlayerData _playerData)
    {
        charaVisual.sprite = _playerData.GetCharacterAnimData().GetCharaIdle();
        charaVisual.GetComponent<Animator>().runtimeAnimatorController = _playerData.GetCharacterAnimData().GetIdleAnimForUI();
        nameText.text = _playerData.GetCharaName();

        Color[] rollColor = new Color[3] { new Color(0.75f, 0.25f, 0.25f), new Color(0.25f, 0.75f, 0.25f), new Color(0.25f, 0.25f, 0.75f) };
        string[] rollKanji = new string[3] { "ãﬂ", "íÜ", "âì" };
        int rollNumber = (int)_playerData.GetShell().GetShellType();

        rollDisplay.color = rollColor[rollNumber];
        rollText.text = rollKanji[rollNumber];

        for (int i = 0; i < 3; i++)
        {
            Color difficulityColor = Color.gray * 0.5f;
            if (i < _playerData.GetDifficulity()) { difficulityColor = Color.yellow; }
            difficulity.transform.GetChild(i).GetComponent<Image>().color = difficulityColor;
        }
    }

    void ShellDisplayUpdate(PlayerData _playerData)
    {
        shellIcon.sprite = _playerData.GetShell().GetShellIcon();
        shellText.text = _playerData.GetShell().GetShellExplain();
    }
    void SubWeaponDisplayUpdate(PlayerData _playerData)
    {
        subWeaponIcon.sprite = _playerData.GetSubWeapon().GetIcon();
        subWeaponText.text = _playerData.GetSubWeapon().GetSubWeaponExplain();
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

    void PressSubmit()
    {
        if (InputManager.GetKeyDown(BoolActions.SouthButton))
        {
            rm.PressSubmit();
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
