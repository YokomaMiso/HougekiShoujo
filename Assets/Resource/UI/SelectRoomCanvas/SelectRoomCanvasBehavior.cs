using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SelectRoomCanvasBehavior : MonoBehaviour
{
    enum SELECT_ROOM_BUTTON_ID { UPDATE = 0, JOIN, RANDOM, BACK_TO_TITLE, MAX_NUM };

    Image[] buttons = new Image[(int)SELECT_ROOM_BUTTON_ID.MAX_NUM];
    Image createButton;
    Image joinButton;
    Image randomButton;
    Image backTitleButton;

    //�{�^���̃J�[�\��
    int selectButtonNum = 0;
    //�{�^���̍ő吔
    const int buttonItemNum = 4;
    //�J�[�\���ړ��\���ǂ���
    bool isCanSelect = true;

    //joinButton�����������ǂ���
    bool selectJoin;

    //���[��banner�̐e
    GameObject roomBanners;

    //���[���o�i�[�̃J�[�\��
    public int selectRoomBannerNum = 0;
    //�{�^���̍ő吔
    const int roomBannerItemNum = 8;

    //���[���o�i�[���X�g
    List<RoomBanner> roomBannerList = new List<RoomBanner>();

    [SerializeField] GameObject connectingWindowPrefab;
    GameObject connectingWindowInstance;

    void Start()
    {
        OSCManager.OSCinstance.startPort = 50000;
        OSCManager.OSCinstance.pSRCB = this;

        for (int i = 0; i < (int)SELECT_ROOM_BUTTON_ID.MAX_NUM; i++) { buttons[i] = transform.GetChild(i).GetComponent<Image>(); }
        buttons[selectButtonNum].color = Color.yellow;

        roomBanners = transform.GetChild(4).gameObject;
        for (int i = 0; i < roomBannerItemNum; i++)
        {
            RoomBanner _room = roomBanners.transform.GetChild(i).GetComponent<RoomBanner>();
            _room.SetParent(this);

            //�������������ԍ���K�p
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
        //�V�[���`�F���W�̃L�����o�X����������Ă����瑁�����^�[��
        if (Managers.instance.UsingCanvas()) { return; }

        //�ʐM���Ȃ烊�^�\��
        if (connectingWindowInstance != null) { return; }

        //Join��������Ă��Ȃ�
        if (!selectJoin)
        {
            ChangeButtonSelectNum();
            DecideButtonSelect();
        }
        //Join�������ꂽ
        else
        {
            ChangeRoomBannerSelectNum();
            DecideRoom();
            BackButtonSelect();
        }
    }

    void ChangeButtonSelectNum()
    {
        Vector2 value = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);

        //�J�[�\���ړ�
        if (Mathf.Abs(value.y) > 0.7f)
        {
            if (isCanSelect)
            {
                buttons[selectButtonNum].color = Color.white;
                if (value.y < 0) { selectButtonNum = (selectButtonNum + 1) % buttonItemNum; }
                else { selectButtonNum = (selectButtonNum + buttonItemNum - 1) % buttonItemNum; }
                buttons[selectButtonNum].color = Color.yellow;
                isCanSelect = false;
            }
        }
        //�O�t���[���̏��ۑ�
        else if (Mathf.Abs(value.y) < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void DecideBehavior()
    {
        switch ((SELECT_ROOM_BUTTON_ID)selectButtonNum)
        {
            case SELECT_ROOM_BUTTON_ID.UPDATE:
                //ConnectionScene�Ɉړ����A�������쐬

                connectingWindowInstance = Instantiate(connectingWindowPrefab, transform);
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
                //ConnectionScene�Ɉړ����A�����ɎQ��or�쐬
                //Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
                //Managers.instance.ChangeState(GAME_STATE.CONNECTION);
                break;

            case SELECT_ROOM_BUTTON_ID.BACK_TO_TITLE:
                Managers.instance.ChangeScene(GAME_STATE.TITLE);
                Managers.instance.ChangeState(GAME_STATE.TITLE);
                break;
        }
    }

    void DecideButtonSelect()
    {
        //���肪������Ă��Ȃ��Ȃ烊�^�[��
        if (!InputManager.GetKeyDown(BoolActions.SouthButton)) { return; }

        DecideBehavior();
    }
    public void DecideButtonSelectFromUI(int _selectButtonNum)
    {
        //�V�[���`�F���W�̃L�����o�X����������Ă����瑁�����^�[��
        if (Managers.instance.UsingCanvas()) { return; }

        //�ʐM���Ȃ烊�^�\��
        if (connectingWindowInstance != null) { return; }

        if (selectJoin) { return; }

        if (selectButtonNum == _selectButtonNum) { DecideBehavior(); }
        else 
        {
            buttons[selectButtonNum].color = Color.white;
            selectButtonNum = _selectButtonNum;
            buttons[selectButtonNum].color = Color.yellow;
        }
    }

    void ChangeRoomBannerSelectNum()
    {
        Vector2 value = InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis);

        //�J�[�\�����ړ�
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
        //�J�[�\���c�ړ�
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
        //�O�t���[���̏��ۑ�
        else if (value.magnitude < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void DecideRoom()
    {
        //���肪������Ă��Ȃ��Ȃ烊�^�[��
        if (!InputManager.GetKeyDown(BoolActions.SouthButton)) { return; }

        OSCManager.OSCinstance.startPort = selectRoomBannerNum * 10 + 50000;

        //ConnectionScene�Ɉړ����A�I�����������ɎQ��
        Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
        Managers.instance.ChangeState(GAME_STATE.CONNECTION);
    }

    public void DecideRoomFromTouch(int _num)
    {
        if (!selectJoin) { return; }

        if (_num == selectRoomBannerNum)
        {
            OSCManager.OSCinstance.startPort = selectRoomBannerNum * 10 + 50000;

            //ConnectionScene�Ɉړ����A�I�����������ɎQ��
            Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
            Managers.instance.ChangeState(GAME_STATE.CONNECTION);
        }
        else
        {
            roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.white;
            selectRoomBannerNum = _num;
            roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.yellow;
        }
    }

    void BackButtonSelect()
    {
        //���肪������Ă�Ȃ烊�^�[��
        if (InputManager.GetKeyDown(BoolActions.SouthButton)) { return; }
        //�L�����Z����������Ă��Ȃ��Ȃ烊�^�[��
        if (!InputManager.GetKeyDown(BoolActions.EastButton)) { return; }

        roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.white;
        selectJoin = false;
    }

    public void SetRoomBannerData(AllGameData.AllData _allData, int i)
    {
        roomBannerList[i].SetData(_allData, i);
    }
}
