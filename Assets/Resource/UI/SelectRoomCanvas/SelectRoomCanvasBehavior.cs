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

    //�J�[�\��
    public int selectRoomNum = 0;
    //���[���̍ő吔
    public readonly int maxRoomNum = 8;
    //������̃{�^���̐�
    public readonly int headerButonNum = 2;
    //�������̃{�^���̐�
    const int footerButonNum = 1;

    //���[��banner�̐e
    [SerializeField] GameObject roomBanners;

    //���[���o�i�[���X�g
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

        ChangeButtonSelectNum();
        DecideButtonSelect();
        BackButtonSelect();
        PressLeftShoulder();
    }

    void CursorUpdate()
    {
        //�J�[�\�����W�̕ύX
        //�J�[�\�������[�����ɋ��鎞�̋���
        if (selectRoomNum < maxRoomNum)
        {
            cursor.SetPosition(roomBannerList[selectRoomNum].GetComponent<RectTransform>());
        }
        //�J�[�\����������̃{�^���ɋ��鎞
        else if (selectRoomNum < maxRoomNum + headerButonNum)
        {
            if (selectRoomNum == maxRoomNum) { cursor.SetPosition(backTitleButton); }
            else { cursor.SetPosition(refreshButton); }
        }
        //�J�[�\�����������̃{�^���ɋ��鎞
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

        //�J�[�\�������[����i�ɋ��鎞�̋���
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
                //������̃{�^���Ɉړ�
                if (value.y > 0) { selectRoomNum = maxRoomNum + selectRoomNum / 2; }
                //���[���̉��i�Ɉړ�
                else { selectRoomNum += maxNum; }
            }
        }
        //�J�[�\�������[�����i�ɋ��鎞�̋���
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
                //���[���̏�i�Ɉړ�
                if (value.y > 0) { selectRoomNum -= maxNum; }
                //�������̃{�^���Ɉړ�
                else { selectRoomNum = maxRoomNum + headerButonNum; }
            }
        }
        //�J�[�\����������̃{�^���ɋ��鎞
        else if (selectRoomNum < maxRoomNum + headerButonNum)
        {
            if (Mathf.Abs(value.x) > border)
            {
                if (value.x > 0) { selectRoomNum = maxRoomNum + 1; }
                else { selectRoomNum = maxRoomNum; }
            }

            if (Mathf.Abs(value.y) > border)
            {
                //���������Ă��������Ȃ�
                if (value.y > 0) { }
                //���[���̏�i�Ɉړ�
                else { selectRoomNum = (selectRoomNum % 2) * (maxRoomNum / 2 - 1); }
            }
        }
        //�J�[�\�����������̃{�^���ɋ��鎞
        else
        {
            if (Mathf.Abs(value.y) > border)
            {
                //���[���̉��i�Ɉړ�
                if (value.y > 0) { selectRoomNum = maxRoomNum - 1; }
            }
        }


        CursorUpdate();
    }

    void DecideButtonSelect()
    {
        //���肪������Ă��Ȃ��Ȃ烊�^�[��
        if (!InputManager.GetKeyDown(BoolActions.SouthButton)) { return; }

        DecideBehavior();
    }

    void DecideBehavior()
    {
        //�J�[�\�������[�����ɋ��鎞�̋���
        if (selectRoomNum < maxRoomNum)
        {
            DecideRoom();
        }
        //�J�[�\����������̃{�^���ɋ��鎞
        else if (selectRoomNum < maxRoomNum + headerButonNum)
        {
            //�^�C�g���ɖ߂�
            if (selectRoomNum == maxRoomNum)
            {
                Managers.instance.ChangeScene(GAME_STATE.TITLE);
                Managers.instance.ChangeState(GAME_STATE.TITLE);
            }
            //���[�����̍X�V
            else
            {
                connectingWindowInstance = Instantiate(connectingWindowPrefab, transform);
                OSCManager.OSCinstance.SearchRoom(false);
            }
        }
        //�J�[�\�����������̃{�^���ɋ��鎞
        else
        {
            selectRoomNum = Random.Range(0, maxRoomNum);
            DecideRoom();
        }
    }

    public void DecideButtonSelectFromUI(int _selectRoomNum)
    {
        //�V�[���`�F���W�̃L�����o�X����������Ă����瑁�����^�[��
        if (Managers.instance.UsingCanvas()) { return; }

        //�ʐM���Ȃ烊�^�\��
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

        //ConnectionScene�Ɉړ����A�I�����������ɎQ��
        Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
        Managers.instance.ChangeState(GAME_STATE.CONNECTION);
    }

    void BackButtonSelect()
    {
        //���肪������Ă�Ȃ烊�^�[��
        if (InputManager.GetKeyDown(BoolActions.SouthButton)) { return; }
        //�L�����Z����������Ă��Ȃ��Ȃ烊�^�[��
        if (!InputManager.GetKeyDown(BoolActions.EastButton)) { return; }

        Managers.instance.ChangeScene(GAME_STATE.TITLE);
        Managers.instance.ChangeState(GAME_STATE.TITLE);
    }
    void PressLeftShoulder()
    {
        //LB��������ĂȂ��Ȃ烊�^�[��
        if (!InputManager.GetKeyDown(BoolActions.LeftShoulder)) { return; }

        connectingWindowInstance = Instantiate(connectingWindowPrefab, transform);
        OSCManager.OSCinstance.SearchRoom(false);
    }

    public void SetRoomBannerData(AllGameData.AllData _allData, int i)
    {
        roomBannerList[i].SetData(_allData, i);
    }
}
