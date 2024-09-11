using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MachingRoomData;

public class RoomManager : MonoBehaviour
{
    /*�����I�ȏ���*/
    public int myNum = -1;
    public readonly int maxCharaCount = 3;
    public readonly int empty = -1;

    void SendDataToServer()
    {
        OSCManager.OSCinstance.SendRoomData();
    }
    public unsafe void Init()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++) { oscRoomData.readyPlayers[i] = false; }
        oscRoomData.gameStart = false;

        OSCManager.OSCinstance.roomData = oscRoomData;
    }

    //�`�[���̐U�蕪�����s���֐�
    public unsafe void PlayerBannerDivider()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        //�ړ��������`�[���ɋ󂫂�����Δԍ���U��
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (oscRoomData.bannerNum[i] == empty)
            {
                oscRoomData.bannerNum[i] = Managers.instance.playerID;
                myNum = i;
                break;
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;

        int myID = Managers.instance.playerID;
        SendDataToServer();
    }
    //�`�[���ړ����s���֐�
    public unsafe void PlayerBannerChanger(int _num)
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        //�����̃`�[�����Ăяo�����Ƃ����瑁�����^�[��
        if (myNum % 2 == _num) { return; }

        bool canMove = false;
        int nextNum = 0;
        //�ړ��������`�[���ɋ󂫂�����Δԍ���U��
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (i % 2 == _num && oscRoomData.bannerNum[i] == empty)
            {
                oscRoomData.bannerNum[i] = Managers.instance.playerID;
                canMove = true;
                nextNum = i;
                break;
            }
        }

        //�ړ����悤�Ƃ����`�[���ɋ󂫂��Ȃ���Ή��������������^�[��
        if (canMove)
        {

            //�`�[���̈ړ��ɐ���������A�O���������̈ʒu���N���A����
            oscRoomData.bannerNum[myNum] = empty;
            //�����̈ʒu�̔ԍ����X�V
            myNum = nextNum;

            while (true)
            {
                //�z��̐�������
                if (!TidyUpPlayerBanner(oscRoomData)) { break; }
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;

        int myID = Managers.instance.playerID;
        SendDataToServer();
    }

    unsafe bool TidyUpPlayerBanner(RoomData _roomData)
    {
        bool isTidied = false;

        for (int i = 0; i < MachingRoomData.bannerMaxCount - 2; i++)
        {
            //���g����Ȃ�
            if (_roomData.bannerNum[i] == empty)
            {
                //�P���̒��g���󂶂�Ȃ��Ȃ�
                if (_roomData.bannerNum[i + 2] != empty)
                {
                    //�P���̏��������̒��g�ɓ���ւ���
                    _roomData.bannerNum[i] = _roomData.bannerNum[i + 2];
                    _roomData.bannerNum[i + 2] = empty;
                    //�����̔ԍ��������ꍇ�A�ԍ����X�V����
                    if (myNum == i) { myNum = i - 2; }

                    isTidied = true;
                }
            }
        }

        return isTidied;
    }

    public unsafe void CharaSelect(int _playerID, int value)
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        int calc = oscRoomData.selectedCharacterID[_playerID];

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        oscRoomData.selectedCharacterID[_playerID] = calc;

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }

    public unsafe void PressSubmit()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        //DEBUG
        //oscRoomData.gameStart = true;
        //return;

        int myID = Managers.instance.playerID;

        //�������z�X�g�Ȃ�
        if (oscRoomData.hostPlayer == myID)
        {
            int readyCount = 0;
            for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
            {
                if (oscRoomData.readyPlayers[i] && i != myID) { readyCount++; }
            }

            if (readyCount >= Managers.instance.gameManager.allPlayerCount)
            {
                oscRoomData.gameStart = true;
            }
        }
        else
        {
            if (!oscRoomData.readyPlayers[myID])
            {
                oscRoomData.readyPlayers[myID] = true;
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }
    public unsafe void PressCancel()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.receiveRoomData;

        int myID = Managers.instance.playerID;

        if (oscRoomData.hostPlayer == myID)
        {

        }
        else
        {
            if (oscRoomData.readyPlayers[myID])
            {
                oscRoomData.readyPlayers[myID] = false;
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }
}
