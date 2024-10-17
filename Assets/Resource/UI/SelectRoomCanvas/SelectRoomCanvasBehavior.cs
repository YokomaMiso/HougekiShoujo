using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SelectRoomCanvasBehavior : MonoBehaviour
{
    enum SELECT_ROOM_BUTTON_ID { CREATE = 0, JOIN, RANDOM, BACK_TO_TITLE, MAX_NUM };

    Image[] buttons = new Image[(int)SELECT_ROOM_BUTTON_ID.MAX_NUM];
    Image createButton;
    Image joinButton;
    Image randomButton;
    Image backTitleButton;

    //ボタンのカーソル
    int selectButtonNum = 0;
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

    //ルームバナーリスト
    List<RoomBanner> roomBannerList = new List<RoomBanner>();

    void Start()
    {
        OSCManager.OSCinstance.pSRCB = this;

        for (int i = 0; i < (int)SELECT_ROOM_BUTTON_ID.MAX_NUM; i++) { buttons[i] = transform.GetChild(i).GetComponent<Image>(); }
        buttons[selectButtonNum].color = Color.yellow;

        roomBanners = transform.GetChild(4).gameObject;
        for (int i = 0; i < roomBannerItemNum; i++)
        {
            RoomBanner _room = roomBanners.transform.GetChild(i).GetComponent<RoomBanner>();

            _room.AssignChild();

            //検索した部屋番号を適用
            roomBannerList.Add(_room);
        }

        OSCManager.OSCinstance.SearchRoom(true);
    }

    private void OnDestroy()
    {
        OSCManager.OSCinstance.pSRCB = null;
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
        float value = Input.GetAxis("Vertical");

        //カーソル移動
        if (Mathf.Abs(value) > 0.7f)
        {
            if (isCanSelect)
            {
                buttons[selectButtonNum].color = Color.white;
                if (value < 0) { selectButtonNum = (selectButtonNum + 1) % buttonItemNum; }
                else { selectButtonNum = (selectButtonNum + buttonItemNum - 1) % buttonItemNum; }
                buttons[selectButtonNum].color = Color.yellow;
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

        switch ((SELECT_ROOM_BUTTON_ID)selectButtonNum)
        {
            case SELECT_ROOM_BUTTON_ID.CREATE:
                //ConnectionSceneに移動し、部屋を作成

                OSCManager.OSCinstance.SearchRoom(false);

                //Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
                //Managers.instance.ChangeState(GAME_STATE.CONNECTION);
                break;

            case SELECT_ROOM_BUTTON_ID.JOIN:
                selectJoin = true;
                isCanSelect = true;
                roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.yellow;
                break;

            case SELECT_ROOM_BUTTON_ID.RANDOM:
                //ConnectionSceneに移動し、部屋に参加or作成

                

                //Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
                //Managers.instance.ChangeState(GAME_STATE.CONNECTION);
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
        else if (Mathf.Abs(value.y) > 0.7f)
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

        OSCManager.OSCinstance.startPort = selectRoomBannerNum * 10 + 50000;

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

    public void SetRoomBannerData(AllGameData.AllData _allData, int i)
    {
        roomBannerList[i].SetData(_allData, i);
    }
}
