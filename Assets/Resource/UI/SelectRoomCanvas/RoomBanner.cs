using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBanner : MonoBehaviour
{
    [SerializeField] Sprite[] bannerSprites;

    public int roomNum;

    Text roomNumText;
    Image isPlaying;
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
        isPlaying = transform.GetChild(1).GetComponent<Image>();
        memberCountText = transform.GetChild(2).GetComponent<Text>();
        hostText = transform.GetChild(3).GetComponent<Text>();
        hostName = transform.GetChild(4).GetComponent<Text>();
    }

    public void InitBannerText()
    {
        //�����ԍ�
        roomNumText.text = "Room ";
        //�v���C�����ǂ���
        isPlaying.color = Color.clear;
        //�����o�[��
        memberCountText.text = "0/6";
        //�z�X�g�A�C�R��
        hostText.color = Color.clear;
        //���O�̓K�p
        hostName.text = "Empty";

        GetComponent<Image>().sprite = bannerSprites[UnityEngine.Random.Range(0, bannerSprites.Length)];
    }

    public void SetData(AllGameData.AllData _data, int _num = 0)
    {
        //�q�I�u�W�F�N�g�̃|�C���^�Ɏ��̂��w��
        AssignChild();

        //�����ԍ�
        roomNum = _num;
        roomNumText.text = (_num + 1).ToString();

        //�v���C���Ȃ�\��
        if (_data.rData.gameStart) { isPlaying.color = Color.white; }
        //�v���C���Ă��Ȃ��Ȃ瓧����
        else { isPlaying.color = Color.clear; }

        //�����o�[��
        memberCountText.text = _data.rData.playerCount.ToString() + "/6";

        //�z�X�g�A�C�R��
        if (_data.rData.playerCount == 0) { hostText.text = ""; }
        else { hostText.text = "Host"; }

        //���O�̓K�p
        if (_data.rData.playerCount == 0) { hostName.text = "Empty"; }
        else { hostName.text = _data.rData.playerName; }
    }

}
