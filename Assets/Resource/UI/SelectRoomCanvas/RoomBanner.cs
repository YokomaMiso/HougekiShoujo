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

    public void AssignChild()
    {
        roomNumText = transform.GetChild(0).GetComponent<Text>();
        isPlayingText = transform.GetChild(1).GetComponent<Text>();
        memberCountText = transform.GetChild(3).GetComponent<Text>();
        hostIcon = transform.GetChild(4).GetComponent<Image>();
        memberText = transform.GetChild(5).GetComponent<Text>(); 
    }

    private void Awake()
    {
        //AssignChild();
    }

    public void SetData(AllGameData.AllData _data, int _num = 0)
    {
        //�q�I�u�W�F�N�g�̃|�C���^�Ɏ��̂��w��
        //AssignChild();

        //�����ԍ�
        roomNumText.text = "Room " + (_num + 1).ToString();

        //�v���C���Ȃ�\��
        if (_data.rData.gameStart) { }
        //�v���C���Ă��Ȃ��Ȃ瓧����
        else { isPlayingText.color = Color.clear; }

        //�����o�[��
        memberCountText.text = _data.rData.playerCount.ToString() + "/6";

        //�z�X�g�A�C�R��
        if (_data.rData.playerCount == 0) { hostIcon.color = Color.clear; }

        //���O�̓K�p
        memberText.text = _data.rData.playerName;

        //�����o�[�̖��O
        //for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        //{
        //    //���Ȃ���Γ����ɂ���
        //    if (_data[i].rData.myID == -1)
        //    {
        //        memberTexts[i].color = Color.clear;
        //        continue;
        //    }

        //    //���O�̓K�p
        //    memberTexts[i].text = _data[i].rData.playerName;
        //}
    }

}
