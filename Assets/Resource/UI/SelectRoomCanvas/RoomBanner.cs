using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomBanner : MonoBehaviour
{
    Text roomNumText;
    Text isPlayingText;
    Text memberCountText;
    Image hostIcon;
    Text memberText;

    void AssignChild()
    {
        roomNumText = transform.GetChild(0).GetComponent<Text>();
        isPlayingText = transform.GetChild(1).GetComponent<Text>();
        memberCountText = transform.GetChild(3).GetComponent<Text>();
        hostIcon = transform.GetChild(4).GetComponent<Image>();
        memberText = transform.GetChild(5).GetComponent<Text>(); 
    }

    public void InitBannerText()
    {
        //�����ԍ�
        roomNumText.text = "Room ";
        //�v���C�����ǂ���
        isPlayingText.color=Color.clear;
        //�����o�[��
        memberCountText.text = "0/6";
        //�z�X�g�A�C�R��
        hostIcon.color = Color.clear;
        //���O�̓K�p
        memberText.text = "Empty";
    }

    public void SetData(AllGameData.AllData _data, int _num = 0)
    {
        //�q�I�u�W�F�N�g�̃|�C���^�Ɏ��̂��w��
        AssignChild();

        //�����ԍ�
        roomNumText.text = "Room " + (_num + 1).ToString();

        //�v���C���Ȃ�\��
        if (_data.rData.gameStart) { isPlayingText.color = Color.red; }
        //�v���C���Ă��Ȃ��Ȃ瓧����
        else { isPlayingText.color = Color.clear; }

        //�����o�[��
        memberCountText.text = _data.rData.playerCount.ToString() + "/6";

        //�z�X�g�A�C�R��
        if (_data.rData.playerCount == 0) { hostIcon.color = Color.clear; }
        else { hostIcon.color = Color.white; }

        //���O�̓K�p
        if (_data.rData.playerCount == 0) { memberText.text = "Empty"; }
        else { memberText.text = _data.rData.playerName; }
    }

}
