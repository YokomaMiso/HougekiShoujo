using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SelectRoomCanvasBehavior : MonoBehaviour
{
    enum SELECT_ROOM_BUTTON_ID { CREATE = 0, JOIN, RANDOM, BACK_TO_TITLE, MAX_NUM };

    Image[] buttons;
    Image createButton;
    Image joinButton;
    Image randomButton;
    Image backTitleButton;

    //ボタンのカーソル
    int selectButonNum = 0;
    //ボタンの最大数
    const int buttonItemNum = 4;
    //カーソル移動可能かどうか
    bool isCanSelect = true;

    //joinButtonを押したかどうか
    bool selectJoin;

    //ルームbannerの親
    GameObject roomBanners;

    //ルームバナーのカーソル
    int selectRoomBannerNum = 0;
    //ボタンの最大数
    const int roomBannerItemNum = 8;

    void Start()
    {
        for (int i = 0; i < (int)SELECT_ROOM_BUTTON_ID.MAX_NUM; i++) { buttons[i] = transform.GetChild(i).GetComponent<Image>(); }
        buttons[selectButonNum].color = Color.yellow;

        roomBanners = transform.GetChild(4).gameObject;
        for (int i = 0; i < roomBannerItemNum; i++)
        {
            //検索した部屋番号を適用
            //roomBanners.GetComponent<RoomBanner>().SetData(/*部屋のデータ*/,i);
        }
    }

    void Update()
    {
        //シーンチェンジのキャンバスが生成されていたら早期リターン
        if (Managers.instance.UsingCanvas()) { return; }

        //Joinが押されていない
        if (!selectJoin)
        {
            ChangeButtonSelectNum();
            DecideButtonSelect();
        }
        //Joinが押された
        else
        {
            ChangeRoomBannerSelectNum();
            DecideRoom();
            BackButtonSelect();
        }
    }

    void ChangeButtonSelectNum()
    {
        float value = Input.GetAxis("Horizontal");

        //カーソル移動
        if (Mathf.Abs(value) > 0.7f)
        {
            if (isCanSelect)
            {
                buttons[selectButonNum].color = Color.white;
                if (value < 0) { selectButonNum = (selectButonNum + buttonItemNum - 1) % buttonItemNum; }
                else { selectButonNum = (selectButonNum + 1) % buttonItemNum; }
                buttons[selectButonNum].color = Color.yellow;
                isCanSelect = false;
            }
        }
        //前フレームの情報保存
        else if (Mathf.Abs(value) < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void DecideButtonSelect()
    {
        //決定が押されていないならリターン
        if (!Input.GetButtonDown("Submit")) { return; }

        switch ((SELECT_ROOM_BUTTON_ID)selectButonNum)
        {
            case SELECT_ROOM_BUTTON_ID.CREATE:
                //ConnectionSceneに移動し、部屋を作成
                Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
                Managers.instance.ChangeState(GAME_STATE.CONNECTION);
                break;

            case SELECT_ROOM_BUTTON_ID.JOIN:
                selectJoin = true;
                isCanSelect = true;
                break;

            case SELECT_ROOM_BUTTON_ID.RANDOM:
                //ConnectionSceneに移動し、部屋に参加or作成
                Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
                Managers.instance.ChangeState(GAME_STATE.CONNECTION);
                break;

            case SELECT_ROOM_BUTTON_ID.BACK_TO_TITLE:
                Managers.instance.ChangeScene(GAME_STATE.TITLE);
                Managers.instance.ChangeState(GAME_STATE.TITLE);
                break;
        }
    }

    void ChangeRoomBannerSelectNum()
    {
        Vector3 value = Vector3.zero;
        value.x = Input.GetAxis("Horizontal");
        value.y = Input.GetAxis("Vertical");

        //カーソル横移動
        if (Mathf.Abs(value.x) > 0.7f)
        {
            if (isCanSelect)
            {
                roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.white;
                if (value.x < 0) { selectRoomBannerNum = (selectRoomBannerNum + roomBannerItemNum - 1) % roomBannerItemNum; }
                else { selectRoomBannerNum = (selectRoomBannerNum + 1) % roomBannerItemNum; }
                roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.yellow;
                isCanSelect = false;
            }
        }
        //カーソル縦移動
        else if (Mathf.Abs(value.x) > 0.7f)
        {
            if (isCanSelect)
            {
                roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.white;
                if (value.y < 0) { selectRoomBannerNum = (selectRoomBannerNum + 2) % roomBannerItemNum; }
                else { selectRoomBannerNum = (selectRoomBannerNum + roomBannerItemNum - 2) % roomBannerItemNum; }
                roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.yellow;
                isCanSelect = false;
            }
        }
        //前フレームの情報保存
        else if (value.magnitude < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void DecideRoom()
    {
        //決定が押されていないならリターン
        if (!Input.GetButtonDown("Submit")) { return; }

        //ConnectionSceneに移動し、選択した部屋に参加
        Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
        Managers.instance.ChangeState(GAME_STATE.CONNECTION);
    }

    void BackButtonSelect()
    {
        //決定が押されてるならリターン
        if (Input.GetButtonDown("Submit")) { return; }
        //キャンセルが押されていないならリターン
        if (!Input.GetButtonDown("Cancel")) { return; }

        roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.white;
        selectJoin = false;
    }
}
