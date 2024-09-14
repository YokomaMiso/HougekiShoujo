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

    Image rollDisplay;
    Text rollText;

    Image shellIcon;
    Text shellText;

    Image subWeaponIcon;
    Text subWeaponText;

    float timer = 0;
    const float charaChangeTimer = 0.5f;

    readonly Vector3[] bannerPos = new Vector3[8]
    {
        new Vector3(-680,240,0),
        new Vector3(680,240,0),

        new Vector3(-680,80,0),
        new Vector3(680,80,0),

        new Vector3(-680,-80,0),
        new Vector3(680,-80,0),

        new Vector3(-680,-240,0),
        new Vector3(680,-240,0),
    };

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

        rollDisplay = charaVisual.transform.GetChild(1).GetComponent<Image>();
        rollText = charaVisual.transform.GetChild(2).GetComponent<Text>();

        shellIcon = charaSelect.transform.GetChild(1).GetComponent<Image>();
        shellText = shellIcon.transform.GetChild(0).GetComponent<Text>();

        subWeaponIcon = charaSelect.transform.GetChild(2).GetComponent<Image>();
        subWeaponText = subWeaponIcon.transform.GetChild(0).GetComponent<Text>();

        //DEBUG
        //OSCManager.OSCinstance.receiveRoomData.SetBannerNum(0, 0);
        //OSCManager.OSCinstance.receiveRoomData.SetBannerNum(2, 2);
        //OSCManager.OSCinstance.receiveRoomData.SetBannerNum(3, 3);
        //OSCManager.OSCinstance.receiveRoomData.SetBannerNum(4, 4);
        //OSCManager.OSCinstance.receiveRoomData.SetBannerNum(5, 5);

        //é©ï™ÇÃèäëÆÉ`Å[ÉÄÇêUÇËï™ÇØÇÈ
        if (rm.myNum == -1) { rm.PlayerBannerDivider(); }
    }

    void Update()
    {
        CharaSelect();
        TeamSelect();
        PressSubmit();
        PressCancel();
        GameStart();
        PlayerBannerDisplayUpdate();
        OpenOption();
    }

    void PlayerBannerDisplayUpdate()
    {
        for (int i = 0; i < playerBanners.transform.childCount; i++)
        {
            playerBanners.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            bool isMine = (i == rm.myNum);
            RoomData roomData = rm.ReadRoomData(isMine);

            if (isMine)
            {
            }
            if (roomData.GetBannerNum(i) != rm.empty)
            {
                playerBanners.transform.GetChild(i).gameObject.SetActive(true);
                playerBanners.transform.GetChild(i).GetComponent<PlayerBannerBehavior>().BannerIconUpdate(roomData);
                playerBanners.transform.GetChild(i).transform.localPosition = bannerPos[i];
                if (isMine) { bannerSelecter.transform.localPosition = bannerPos[i]; }
            }
        }
    }

    void TeamSelect()
    {
        if (rm.ReadRoomData(true).GetReadyPlayers(Managers.instance.playerID)) { return; }

        int teamID = -1;

        if (Input.GetButtonDown("LB")) { teamID = (int)TEAM_NUM.A; }
        else if (Input.GetButtonDown("RB")) { teamID = (int)TEAM_NUM.B; }

        if (teamID < 0) { return; }

        rm.PlayerBannerChanger(teamID);
    }

    void CharaSelect()
    {
        int myID = Managers.instance.playerID;
        float input = Input.GetAxis("Horizontal");

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

        int charaID = rm.ReadRoomData(true).GetSelectedCharacterID(myID);
        PlayerData nowPlayerData = Managers.instance.gameManager.playerDatas[charaID];

        CharaDisplayUpdate(nowPlayerData);
        ShellDisplayUpdate(nowPlayerData);
        SubWeaponDisplayUpdate(nowPlayerData);
    }

    void CharaDisplayUpdate(PlayerData _playerData)
    {
        charaVisual.sprite = _playerData.GetCharacterAnimData().GetCharaIdle();
        nameText.text = _playerData.GetCharaName();

        Color[] rollColor = new Color[3] { new Color(0.75f, 0.25f, 0.25f), new Color(0.25f, 0.75f, 0.25f), new Color(0.25f, 0.25f, 0.75f) };
        string[] rollKanji = new string[3] { "ãﬂ", "íÜ", "âì" };
        int rollNumber = (int)_playerData.GetShell().GetShellType();

        rollDisplay.color = rollColor[rollNumber];
        rollText.text = rollKanji[rollNumber];
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
        if (Input.GetButtonDown("Menu"))
        {
            Managers.instance.ChangeScene(GAME_STATE.OPTION);
            Managers.instance.ChangeState(GAME_STATE.OPTION);
            Managers.instance.canvasManager.ChangeCanvas(GAME_STATE.OPTION);
            Destroy(gameObject);
            return;
        }
    }

    void PressSubmit()
    {
        if (Input.GetButtonDown("Submit"))
        {
            rm.PressSubmit();
        }
    }
    void PressCancel()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            rm.PressCancel();
        }
    }
    void GameStart()
    {
        RoomData oscRoomData = rm.ReadRoomData(Managers.instance.playerID == 0);

        bool start = oscRoomData.gameStart;

        if (!start) { return; }

        GAME_STATE sendState = GAME_STATE.IN_GAME;

        Managers.instance.ChangeScene(sendState);
        Managers.instance.ChangeState(sendState);
        Destroy(gameObject);
    }
}
