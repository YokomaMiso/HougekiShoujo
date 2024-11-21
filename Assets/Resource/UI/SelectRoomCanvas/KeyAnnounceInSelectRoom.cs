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

    readonly string[] cursorMoveText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "�J�[�\���ړ�",
        "Cursor Move",
        "",
        "",
    };
    readonly string[] createRoomText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "�������쐬",
        "Create Room",
        "",
        "",
    };
    readonly string[] backToTitleText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "�^�C�g���ɖ߂�",
        "Back to Title",
        "",
        "",
    };
    readonly string[] refreshText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "�����̍X�V",
        "Refresh Room",
        "",
        "",
    };
    readonly string[] joinToRoomText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "�����ɎQ��",
        "Join to Room",
        "",
        "",
    };
    readonly string[] randomMatchText = new string[(int)LANGUAGE_NUM.MAX_NUM]
    {
        "�����_���}�b�`",
        "Random Match",
        "",
        "",
    };

    void Start()
    {
        keys = new GameObject[maxKeyCount];
        keys[0] = horizon;
        keys[1] = vertical;
        keys[2] = submit;
        keys[3] = cancel;
        keys[4] = leftShoulder;

        keys[1].transform.GetChild(1).GetComponent<Text>().text = cursorMoveText[(int)Managers.instance.nowLanguage];
        keys[2].transform.GetChild(1).GetComponent<Text>().text = createRoomText[(int)Managers.instance.nowLanguage];
        keys[3].transform.GetChild(1).GetComponent<Text>().text = backToTitleText[(int)Managers.instance.nowLanguage];
        keys[4].transform.GetChild(1).GetComponent<Text>().text = refreshText[(int)Managers.instance.nowLanguage];

        ChangeDisplayButtons();
    }

    void Update()
    {
        if (InputManager.isChangedController) { ChangeDisplayButtons(); }

        //�J�[�\�������[�����ɋ��鎞�̋���
        if (srcb.selectRoomNum < srcb.maxRoomNum)
        {
            if (srcb.roomBannerList[srcb.selectRoomNum].memberCount >= 6)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            else if (srcb.roomBannerList[srcb.selectRoomNum].isPlaying)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            //0�l�Ȃ�
            else if (srcb.roomBannerList[srcb.selectRoomNum].memberCount == 0)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = createRoomText[(int)Managers.instance.nowLanguage];
            }
            else
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = joinToRoomText[(int)Managers.instance.nowLanguage]; ;
            }
        }
        //�J�[�\����������̃{�^���ɋ��鎞
        else if (srcb.selectRoomNum < srcb.maxRoomNum + srcb.headerButonNum)
        {
            //�^�C�g���ɖ߂�
            if (srcb.selectRoomNum == srcb.maxRoomNum)
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = backToTitleText[(int)Managers.instance.nowLanguage]; ;
            }
            //���[�����̍X�V
            else
            {
                keys[2].transform.GetChild(1).GetComponent<Text>().text = refreshText[(int)Managers.instance.nowLanguage]; ;
            }
        }
        //�J�[�\�����������̃{�^���ɋ��鎞
        else
        {
            keys[2].transform.GetChild(1).GetComponent<Text>().text = randomMatchText[(int)Managers.instance.nowLanguage]; ;
        }
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
