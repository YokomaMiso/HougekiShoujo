using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MachingRoomData;

public class KeyAnnounceInSelectRoom : MonoBehaviour
{
    [SerializeField] SelectRoomCanvasBehavior srcb;

    [SerializeField] GameObject horizon;
    [SerializeField] GameObject vertical;
    [SerializeField] GameObject submit;
    [SerializeField] GameObject cancel;
    [SerializeField] GameObject leftShoulder;
    GameObject[] keys;
    const int maxKeyCount = 5;

    void Start()
    {
        keys = new GameObject[maxKeyCount];
        keys[0] = horizon;
        keys[1] = vertical;
        keys[2] = submit;
        keys[3] = cancel;
        keys[4] = leftShoulder;

        keys[1].transform.GetChild(1).GetComponent<Text>().text = "�J�[�\���ړ�";
        keys[2].transform.GetChild(1).GetComponent<Text>().text = "�������쐬";
        keys[3].transform.GetChild(1).GetComponent<Text>().text = "�^�C�g���ɖ߂�";
        keys[4].transform.GetChild(1).GetComponent<Text>().text = "�����̍X�V";

        ChangeDisplayButtons();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }

        //�J�[�\�������[�����ɋ��鎞�̋���
        if (srcb.selectRoomNum < srcb.maxRoomNum)
        {
            //0�l�Ȃ�
            if (srcb.roomBannerList[srcb.selectRoomNum].memberCount == 0)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "�������쐬";
            }
            else
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "�����ɎQ��";
            }
        }
        //�J�[�\����������̃{�^���ɋ��鎞
        else if (srcb.selectRoomNum < srcb.maxRoomNum + srcb.headerButonNum)
        {
            //�^�C�g���ɖ߂�
            if (srcb.selectRoomNum == srcb.maxRoomNum)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "�^�C�g���ɖ߂�";
            }
            //���[�����̍X�V
            else
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "�����̍X�V";
            }
        }
        //�J�[�\�����������̃{�^���ɋ��鎞
        else
        {
            keys[2].transform.GetChild(1).GetComponent<Text>().text = "�����_���}�b�`";
        }

        MachingRoomData.RoomData roomData = OSCManager.OSCinstance.roomData;
    }

    void ChangeDisplayButtons()
    {
        Sprite[] applySprites = InputManager.nowButtonSpriteData.GetAllSprites();
        for (int i = 0; i < maxKeyCount; i++) { keys[i].transform.GetChild(0).GetComponent<Image>().sprite = applySprites[i]; }
    }

    bool ReadyChecker(MachingRoomData.RoomData _myData)
    {
        int readyCount = 0;
        for (int i = 1; i < MachingRoomData.playerMaxCount; i++)
        {
            RoomData otherData = OSCManager.OSCinstance.GetRoomData(i);
            if (otherData.ready) { readyCount++; }
        }

        //�����ȊO�̑S�v���C���[��READY���Ȃ�
        if (readyCount >= _myData.playerCount - 1)
        {
#if UNITY_EDITOR
            return true;
#else
            if(readyCount <= 0){ return false; }
            else{ return true; }
#endif
        }

        return false;
    }
}
