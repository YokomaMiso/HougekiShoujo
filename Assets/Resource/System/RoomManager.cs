using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class RoomManager : MonoBehaviour
{
    public int[] bannerNum = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };
    public int myNum;
    public int[] selectedCharacterID = new int[6] { 0, 0, 0, 0, 0, 0 };
    public readonly int maxCharaCount = 3;
    public readonly int empty = -1;

    void ReceiveDataFromServer()
    {
        //��M����
    }
    void SendDataToServer(int _playerID, int _bannerID, int _characterID)
    {
        //���M�������s��
    }

    //�`�[���̐U�蕪�����s���֐�
    public void PlayerBannerDivider()
    {
        //�ړ��������`�[���ɋ󂫂�����Δԍ���U��
        for (int i = 0; i < bannerNum.Length; i++)
        {
            if (bannerNum[i] == empty)
            {
                bannerNum[i] = Managers.instance.playerID;
                myNum = i;
                break;
            }
        }

        int myID = Managers.instance.playerID;
        SendDataToServer(myID, myNum, selectedCharacterID[myID]);
    }
    //�`�[���ړ����s���֐�
    public void PlayerBannerChanger(int _num)
    {
        //�����̃`�[�����Ăяo�����Ƃ����瑁�����^�[��
        if (myNum % 2 == _num) { return; }

        bool canMove = false;
        int nextNum = 0;
        //�ړ��������`�[���ɋ󂫂�����Δԍ���U��
        for (int i = 0; i < bannerNum.Length; i++)
        {
            if (i % 2 == _num && bannerNum[i] == empty)
            {
                bannerNum[i] = Managers.instance.playerID;
                canMove = true;
                nextNum = i;
                break;
            }
        }

        //�ړ����悤�Ƃ����`�[���ɋ󂫂��Ȃ���Ή��������������^�[��
        if (!canMove) { return; }

        //�`�[���̈ړ��ɐ���������A�O���������̈ʒu���N���A����
        bannerNum[myNum] = empty;
        //�����̈ʒu�̔ԍ����X�V
        myNum = nextNum;

        while (true)
        {
            //�z��̐�������
            if (!TidyUpPlayerBanner()) { break; }
        }

        int myID = Managers.instance.playerID;
        SendDataToServer(myID, myNum, selectedCharacterID[myID]);
    }

    bool TidyUpPlayerBanner()
    {
        bool isTidied = false;

        for (int i = 0; i < bannerNum.Length - 2; i++)
        {
            //���g����Ȃ�
            if (bannerNum[i] == empty)
            {
                //�P���̒��g���󂶂�Ȃ��Ȃ�
                if (bannerNum[i + 2] != empty)
                {
                    //�P���̏��������̒��g�ɓ���ւ���
                    bannerNum[i] = bannerNum[i + 2];
                    bannerNum[i + 2] = empty;
                    //�����̔ԍ��������ꍇ�A�ԍ����X�V����
                    if (myNum == i) { myNum = i - 2; }

                    isTidied=true;
                }
            }
        }

        return isTidied;
    }

    public void CharaSelect(int _playerID, int value)
    {
        int calc = selectedCharacterID[_playerID];

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        selectedCharacterID[_playerID] = calc;
    }
}
