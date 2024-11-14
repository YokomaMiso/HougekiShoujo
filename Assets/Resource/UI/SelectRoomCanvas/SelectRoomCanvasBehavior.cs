using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoomCanvasBehavior : MonoBehaviour
{
    enum SELECT_ROOM_BUTTON_ID { UPDATE = 0, JOIN, RANDOM, BACK_TO_TITLE, MAX_NUM };

    [SerializeField] SelectRoomCursorBehavior cursor;

    [SerializeField] RectTransform refreshButton;
    [SerializeField] RectTransform randomButton;
    [SerializeField] RectTransform backTitleButton;

    //カーソル
    public int selectRoomNum = 0;
    //ルームの最大数
    public readonly int maxRoomNum = 8;
    //上方向のボタンの数
    public readonly int headerButonNum = 2;
    //下方向のボタンの数
    const int footerButonNum = 1;

    //ルームbannerの親
    [SerializeField] GameObject roomBanners;

    //ルームバナーリスト
    public List<RoomBanner> roomBannerList = new List<RoomBanner>();

    [SerializeField] GameObject connectingWindowPrefab;
    GameObject connectingWindowInstance;

    void Start()
    {
        OSCManager.OSCinstance.startPort = 50000;
        OSCManager.OSCinstance.pSRCB = this;

        for (int i = 0; i < maxRoomNum; i++)
        {
            RoomBanner _room = roomBanners.transform.GetChild(i).GetComponent<RoomBanner>();
            _room.SetParent(this);

            //検索した部屋番号を適用
            roomBannerList.Add(_room);
        }

        connectingWindowInstance = Instantiate(connectingWindowPrefab, transform);
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

        //通信中ならリタ―ン
        if (connectingWindowInstance != null) { return; }

        ChangeButtonSelectNum();
        DecideButtonSelect();
        BackButtonSelect();
        PressLeftShoulder();
    }

    void CursorUpdate()
    {
        //カーソル座標の変更
        //カーソルがルーム内に居る時の挙動
        if (selectRoomNum < maxRoomNum)
        {
            cursor.SetPosition(roomBannerList[selectRoomNum].GetComponent<RectTransform>());
        }
        //カーソルが上方向のボタンに居る時
        else if (selectRoomNum < maxRoomNum + headerButonNum)
        {
            if (selectRoomNum == maxRoomNum) { cursor.SetPosition(backTitleButton); }
            else { cursor.SetPosition(refreshButton); }
        }
        //カーソルが下方向のボタンに居る時
        else
        {
            cursor.SetPosition(randomButton);
        }
    }

    void ChangeButtonSelectNum()
    {
        const float border = 0.9f;

        Vector2 value = InputManager.GetAxisDelay<Vector2>(Vec2AxisActions.LStickAxis, 0.3f);
        if (value.magnitude < border) { return; }

        //カーソルがルーム上段に居る時の挙動
        if (selectRoomNum < maxRoomNum / 2)
        {
            int maxNum = (maxRoomNum / 2);

            if (Mathf.Abs(value.x) > border)
            {
                if (value.x > 0) { selectRoomNum = (selectRoomNum + 1) % maxNum; }
                else { selectRoomNum = (selectRoomNum + maxNum - 1) % maxNum; }
            }

            if (Mathf.Abs(value.y) > border)
            {
                //上方向のボタンに移動
                if (value.y > 0) { selectRoomNum = maxRoomNum + selectRoomNum / 2; }
                //ルームの下段に移動
                else { selectRoomNum += maxNum; }
            }
        }
        //カーソルがルーム下段に居る時の挙動
        else if (selectRoomNum < maxRoomNum)
        {
            int maxNum = (maxRoomNum / 2);

            if (Mathf.Abs(value.x) > border)
            {
                if (value.x > 0) { selectRoomNum = (selectRoomNum + 1) % maxNum + 4; }
                else { selectRoomNum = (selectRoomNum + maxNum - 1) % maxNum + 4; }
            }

            if (Mathf.Abs(value.y) > border)
            {
                //ルームの上段に移動
                if (value.y > 0) { selectRoomNum -= maxNum; }
                //下方向のボタンに移動
                else { selectRoomNum = maxRoomNum + headerButonNum; }
            }
        }
        //カーソルが上方向のボタンに居る時
        else if (selectRoomNum < maxRoomNum + headerButonNum)
        {
            if (Mathf.Abs(value.x) > border)
            {
                if (value.x > 0) { selectRoomNum = maxRoomNum + 1; }
                else { selectRoomNum = maxRoomNum; }
            }

            if (Mathf.Abs(value.y) > border)
            {
                //上を押されても何もしない
                if (value.y > 0) { }
                //ルームの上段に移動
                else { selectRoomNum = (selectRoomNum % 2) * (maxRoomNum / 2 - 1); }
            }
        }
        //カーソルが下方向のボタンに居る時
        else
        {
            if (Mathf.Abs(value.y) > border)
            {
                //ルームの下段に移動
                if (value.y > 0) { selectRoomNum = maxRoomNum - 1; }
            }
        }


        CursorUpdate();
    }

    void DecideButtonSelect()
    {
        //決定が押されていないならリターン
        if (!InputManager.GetKeyDown(BoolActions.SouthButton)) { return; }

        DecideBehavior();
    }

    void DecideBehavior()
    {
        //カーソルがルーム内に居る時の挙動
        if (selectRoomNum < maxRoomNum)
        {
            DecideRoom();
        }
        //カーソルが上方向のボタンに居る時
        else if (selectRoomNum < maxRoomNum + headerButonNum)
        {
            //タイトルに戻る
            if (selectRoomNum == maxRoomNum)
            {
                Managers.instance.ChangeScene(GAME_STATE.TITLE);
                Managers.instance.ChangeState(GAME_STATE.TITLE);
            }
            //ルーム情報の更新
            else
            {
                connectingWindowInstance = Instantiate(connectingWindowPrefab, transform);
                OSCManager.OSCinstance.SearchRoom(false);
            }
        }
        //カーソルが下方向のボタンに居る時
        else
        {
            selectRoomNum = Random.Range(0, maxRoomNum);
            DecideRoom();
        }
    }

    public void DecideButtonSelectFromUI(int _selectRoomNum)
    {
        //シーンチェンジのキャンバスが生成されていたら早期リターン
        if (Managers.instance.UsingCanvas()) { return; }

        //通信中ならリタ―ン
        if (connectingWindowInstance != null) { return; }


        if (selectRoomNum == _selectRoomNum) { DecideBehavior(); }
        else 
        {
            selectRoomNum = _selectRoomNum;
            CursorUpdate();
        }
    }

    void DecideRoom()
    {
        OSCManager.OSCinstance.startPort = selectRoomNum * 10 + 50000;

        //ConnectionSceneに移動し、選択した部屋に参加
        Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
        Managers.instance.ChangeState(GAME_STATE.CONNECTION);
    }

    void BackButtonSelect()
    {
        //決定が押されてるならリターン
        if (InputManager.GetKeyDown(BoolActions.SouthButton)) { return; }
        //キャンセルが押されていないならリターン
        if (!InputManager.GetKeyDown(BoolActions.EastButton)) { return; }

        Managers.instance.ChangeScene(GAME_STATE.TITLE);
        Managers.instance.ChangeState(GAME_STATE.TITLE);
    }
    void PressLeftShoulder()
    {
        //LBが押されてないならリターン
        if (!InputManager.GetKeyDown(BoolActions.LeftShoulder)) { return; }

        connectingWindowInstance = Instantiate(connectingWindowPrefab, transform);
        OSCManager.OSCinstance.SearchRoom(false);
    }

    public void SetRoomBannerData(AllGameData.AllData _allData, int i)
    {
        roomBannerList[i].SetData(_allData, i);
    }
}
