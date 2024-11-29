using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBanner : MonoBehaviour
{
    [SerializeField] Sprite[] bannerSprites;

    public int roomNum;
    public int memberCount;
    public bool isPlaying;

    Text roomNumText;
    Image isPlayingIcon;
    Text memberCountText;
    Text hostText;
    Text hostName;

    SelectRoomCanvasBehavior parent;
    public void SetParent(SelectRoomCanvasBehavior _parent) { parent = _parent; }

    public void PressThisBanner()
    {
        parent.DecideButtonSelectFromUI(roomNum);
    }

    void AssignChild()
    {
        roomNumText = transform.GetChild(0).GetComponent<Text>();
        isPlayingIcon = transform.GetChild(1).GetComponent<Image>();
        memberCountText = transform.GetChild(2).GetComponent<Text>();
        hostText = transform.GetChild(3).GetComponent<Text>();
        hostName = transform.GetChild(4).GetComponent<Text>();
    }

    public void InitBannerText()
    {
        //�����ԍ�
        roomNumText.text = "Room ";
        //�v���C�����ǂ���
        isPlayingIcon.color = Color.clear;
        //�����o�[��
        memberCountText.text = "0/6";
        //�z�X�g�A�C�R��
        hostText.color = Color.clear;
        //���O�̓K�p
        hostName.text = "Empty";

        GetComponent<Image>().sprite = bannerSprites[0];
    }

    public void SetData(AllGameData.AllData _data, int _num = 0)
    {
        //�q�I�u�W�F�N�g�̃|�C���^�Ɏ��̂��w��
        AssignChild();

        //�����ԍ�
        roomNum = _num;
        roomNumText.text = (_num + 1).ToString();

        //�v���C���Ȃ�\��
        isPlaying = _data.rData.gameStart;
        if (_data.rData.gameStart) { isPlayingIcon.color = Color.white; }
        //�v���C���Ă��Ȃ��Ȃ瓧����
        else { isPlayingIcon.color = Color.clear; }

        //�����o�[��
        memberCount = _data.rData.playerCount;
        memberCountText.text = memberCount.ToString() + "/6";

        //�z�X�g�A�C�R��
        if (_data.rData.playerCount == 0) { hostText.text = ""; }
        else { hostText.text = "Host"; }

        //���O�̓K�p
        if (_data.rData.playerCount == 0) { hostName.text = "Empty"; }
        else { hostName.text = _data.rData.playerName; }

        int spriteNum;
        if (isPlaying) { spriteNum = 2; }
        else if (memberCount >= 6) { spriteNum = 2; }
        else if (memberCount == 0) { spriteNum = 0; }
        else { spriteNum = 1; }

        GetComponent<Image>().sprite = bannerSprites[spriteNum];
    }

}