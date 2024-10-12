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

    //�{�^���̃J�[�\��
    int selectButonNum = 0;
    //�{�^���̍ő吔
    const int buttonItemNum = 4;
    //�J�[�\���ړ��\���ǂ���
    bool isCanSelect = true;

    //joinButton�����������ǂ���
    bool selectJoin;

    //���[��banner�̐e
    GameObject roomBanners;

    //���[���o�i�[�̃J�[�\��
    int selectRoomBannerNum = 0;
    //�{�^���̍ő吔
    const int roomBannerItemNum = 8;

    void Start()
    {
        for (int i = 0; i < (int)SELECT_ROOM_BUTTON_ID.MAX_NUM; i++) { buttons[i] = transform.GetChild(i).GetComponent<Image>(); }
        buttons[selectButonNum].color = Color.yellow;

        roomBanners = transform.GetChild(4).gameObject;
        for (int i = 0; i < roomBannerItemNum; i++)
        {
            //�������������ԍ���K�p
            //roomBanners.GetComponent<RoomBanner>().SetData(/*�����̃f�[�^*/,i);
        }
    }

    void Update()
    {
        //�V�[���`�F���W�̃L�����o�X����������Ă����瑁�����^�[��
        if (Managers.instance.UsingCanvas()) { return; }

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
        float value = Input.GetAxis("Horizontal");

        //�J�[�\���ړ�
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
        //�O�t���[���̏��ۑ�
        else if (Mathf.Abs(value) < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void DecideButtonSelect()
    {
        //���肪������Ă��Ȃ��Ȃ烊�^�[��
        if (!Input.GetButtonDown("Submit")) { return; }

        switch ((SELECT_ROOM_BUTTON_ID)selectButonNum)
        {
            case SELECT_ROOM_BUTTON_ID.CREATE:
                //ConnectionScene�Ɉړ����A�������쐬
                Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
                Managers.instance.ChangeState(GAME_STATE.CONNECTION);
                break;

            case SELECT_ROOM_BUTTON_ID.JOIN:
                selectJoin = true;
                isCanSelect = true;
                break;

            case SELECT_ROOM_BUTTON_ID.RANDOM:
                //ConnectionScene�Ɉړ����A�����ɎQ��or�쐬
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
        //�O�t���[���̏��ۑ�
        else if (value.magnitude < 0.2f)
        {
            isCanSelect = true;
        }
    }

    void DecideRoom()
    {
        //���肪������Ă��Ȃ��Ȃ烊�^�[��
        if (!Input.GetButtonDown("Submit")) { return; }

        //ConnectionScene�Ɉړ����A�I�����������ɎQ��
        Managers.instance.ChangeScene(GAME_STATE.CONNECTION);
        Managers.instance.ChangeState(GAME_STATE.CONNECTION);
    }

    void BackButtonSelect()
    {
        //���肪������Ă�Ȃ烊�^�[��
        if (Input.GetButtonDown("Submit")) { return; }
        //�L�����Z����������Ă��Ȃ��Ȃ烊�^�[��
        if (!Input.GetButtonDown("Cancel")) { return; }

        roomBanners.transform.GetChild(selectRoomBannerNum).GetComponent<Image>().color = Color.white;
        selectJoin = false;
    }
}
