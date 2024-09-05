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

    Image shellIcon;
    Text shellText;

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

        shellIcon = charaSelect.transform.GetChild(1).GetComponent<Image>();
        shellText = shellIcon.transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        CharaSelect();
        PlayerBannerController();
        GameStart();
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
        shellIcon.sprite = Managers.instance.gameManager.playerDatas[charaSelectNum].GetShell().GetShellIcon();
        shellText.text = Managers.instance.gameManager.playerDatas[charaSelectNum].GetShell().GetShellExplain();
    }

    void PlayerBannerController()
    {
        bannerSelecter.transform.localPosition = bannerPos[Managers.instance.playerID];
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
