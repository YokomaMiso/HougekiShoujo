using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    Vector3[] bannerPos = new Vector3[6]
    {
        new Vector3(-680,240,0),
        new Vector3(-680,40,0),
        new Vector3(-680,-160,0),
        new Vector3(680,240,0),
        new Vector3(680,40,0),
        new Vector3(680,-160,0),
    };

    void Start()
    {
        charaSelect = transform.GetChild(0).gameObject;
        teamsBG = transform.GetChild(1).gameObject;
        bannerSelecter = transform.GetChild(2).gameObject;
        playerBanners = transform.GetChild(3).gameObject;

        charaVisual = charaSelect.transform.GetChild(0).GetComponent<Image>();
        nameBar = charaVisual.transform.GetChild(0).GetComponent<Image>();
        nameText= nameBar.transform.GetChild(0).GetComponent<Text>();

        rollDisplay = charaVisual.transform.GetChild(1).GetComponent<Image>();
        rollText = charaVisual.transform.GetChild(2).GetComponent<Text>();

        shellIcon = charaSelect.transform.GetChild(1).GetComponent<Image>();
        shellText = shellIcon.transform.GetChild(0).GetComponent<Text>();

        subWeaponIcon = charaSelect.transform.GetChild(2).GetComponent<Image>();
        subWeaponText = subWeaponIcon.transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        CharaSelect();
        PlayerBannerController();
        GameStart();
        OpenOption();
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
        string[] rollKanji = new string[3] { "‹ß", "’†", "‰“" };
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


    void PlayerBannerController()
    {
        bannerSelecter.transform.localPosition = bannerPos[Managers.instance.playerID];
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
