using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TEAM_NUM { A = 0, B = 1 };

public class RoomCanvasBehavior : MonoBehaviour
{
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

    int charaSelectNum = 0;
    const int maxCharaCount = 3;

    float timer = 0;
    const float charaChangeTimer = 0.5f;

    int[] bannerNum = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };
    int myNum;
    int testNum;
    const int empty = -1;

    Vector3[] bannerPos = new Vector3[8]
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

        //自分の所属チームを振り分ける
        PlayerBannerDivider();
    }

    void Update()
    {
        CharaSelect();
        if (Input.GetKeyDown(KeyCode.J)) { PlayerBannerChanger((int)TEAM_NUM.A); }
        else if (Input.GetKeyDown(KeyCode.K)) { PlayerBannerChanger((int)TEAM_NUM.B); }
        PlayerBannerDisplayUpdate();
        GameStart();
        OpenOption();
    }

    //チームの振り分けを行う関数
    void PlayerBannerDivider()
    {
        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < bannerNum.Length; i++)
        {
            if (bannerNum[i] == empty)
            {
                bannerNum[i] = Managers.instance.playerID;
                myNum = i;
                break;
            }
        }
    }
    //チーム移動を行う関数
    void PlayerBannerChanger(int _num)
    {
        //自分のチームを呼び出そうとしたら早期リターン
        if (myNum % 2 == _num) { return; }

        bool canMove = false;
        int nextNum = 0;
        //移動したいチームに空きがあれば番号を振る
        for (int i = 0; i < bannerNum.Length; i++)
        {
            if (i % 2 == _num && bannerNum[i] == empty)
            {
                bannerNum[i] = Managers.instance.playerID;
                canMove = true;
                nextNum = i;
                break;
            }
        }

        //移動しようとしたチームに空きがなければ何もせず早期リターン
        if (!canMove) { return; }

        //チームの移動に成功したら、前居た自分の位置をクリアする
        bannerNum[myNum] = empty;
        //自分の位置の番号を更新
        myNum = nextNum;
    }


    void TidyUpPlayerBanner()
    {
        for (int i = 0; i < bannerNum.Length - 2; i++)
        {
            //中身が空なら
            if (bannerNum[i] == empty)
            {
                //１つ下の中身が空じゃないなら
                if (bannerNum[i + 2] != empty)
                {
                    //１つ下の情報を自分の中身に入れ替える
                    bannerNum[i] = bannerNum[i + 2];
                    bannerNum[i + 2] = empty;
                    //自分の番号だった場合、番号を更新する
                    if (myNum == i) { myNum = i - 2; }
                }
            }
        }
    }
    void PlayerBannerDisplayUpdate()
    {
        TidyUpPlayerBanner();

        for (int i = 0; i < playerBanners.transform.childCount; i++)
        {
            playerBanners.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < bannerNum.Length; i++)
        {
            if (bannerNum[i] != empty)
            {
                playerBanners.transform.GetChild(bannerNum[i]).gameObject.SetActive(true);
                playerBanners.transform.GetChild(bannerNum[i]).transform.localPosition = bannerPos[i];
                if (bannerNum[i] == Managers.instance.playerID) { bannerSelecter.transform.localPosition = bannerPos[i]; }
            }
        }
    }

    void CharaSelect()
    {
        float input = Input.GetAxis("Horizontal");

        if (Mathf.Abs(input) > 0.9f)
        {
            if (timer == 0)
            {
                if (input > 0) { charaSelectNum = (charaSelectNum + 1) % maxCharaCount; }
                else { charaSelectNum = (charaSelectNum + (maxCharaCount - 1)) % maxCharaCount; }
            }

            timer += Time.deltaTime;
            if (timer > charaChangeTimer) { timer = 0; }
        }
        else if (Mathf.Abs(input) < 0.2f)
        {
            timer = 0;
        }

        Managers.instance.gameManager.SetCharacterNum(charaSelectNum);
        PlayerData nowPlayerData = Managers.instance.gameManager.playerDatas[charaSelectNum];

        CharaDisplayUpdate(nowPlayerData);
        ShellDisplayUpdate(nowPlayerData);
        SubWeaponDisplayUpdate(nowPlayerData);
    }

    void CharaDisplayUpdate(PlayerData _playerData)
    {
        charaVisual.sprite = _playerData.GetCharacterAnimData().GetCharaIdle();
        nameText.text = _playerData.GetCharaName();

        Color[] rollColor = new Color[3] { new Color(0.75f, 0.25f, 0.25f), new Color(0.25f, 0.75f, 0.25f), new Color(0.25f, 0.25f, 0.75f) };
        string[] rollKanji = new string[3] { "近", "中", "遠" };
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
            Debug.Log("aaaa");

            Managers.instance.ChangeScene(GAME_STATE.OPTION);
            Managers.instance.ChangeState(GAME_STATE.OPTION);
            Managers.instance.canvasManager.ChangeCanvas(GAME_STATE.OPTION);
            Destroy(gameObject);
            return;
        }
    }

    void GameStart()
    {
        if (Input.GetButtonDown("Submit"))
        {
            GAME_STATE sendState = GAME_STATE.IN_GAME;

            Managers.instance.ChangeScene(sendState);
            Managers.instance.ChangeState(sendState);
            Destroy(gameObject);
        }
    }
}
