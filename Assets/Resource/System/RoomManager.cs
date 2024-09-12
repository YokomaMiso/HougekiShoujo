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
        if (Managers.instance.onDebug) { return; }
        OSCManager.OSCinstance.SendRoomData();
    }
    public void Init()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++) { oscRoomData.SetReadyPlayers(i, false); }
        oscRoomData.gameStart = false;

        OSCManager.OSCinstance.roomData = oscRoomData;
    }

    //�`�[���̐U�蕪�����s���֐�
    public void PlayerBannerDivider()
    {
        RoomData oscRoomData;
        int id = Managers.instance.playerID;
        if (id == 0) { oscRoomData = OSCManager.OSCinstance.roomData; }
        else { oscRoomData = OSCManager.OSCinstance.receiveRoomData; }

        //�ړ��������`�[���ɋ󂫂�����Δԍ���U��
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (oscRoomData.GetBannerNum(i) == empty)
            {
                oscRoomData.SetBannerNum(i, Managers.instance.playerID);
                myNum = i;
                break;
            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;

        int myID = Managers.instance.playerID;
        SendDataToServer();
    }
    //�`�[���ړ����s���֐�
    public void PlayerBannerChanger(int _num)
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;

        //�����̃`�[�����Ăяo�����Ƃ����瑁�����^�[��
        if (myNum % 2 == _num) { return; }

        bool canMove = false;
        int nextNum = 0;
        //�ړ��������`�[���ɋ󂫂�����Δԍ���U��
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++)
        {
            if (i % 2 == _num && oscRoomData.GetBannerNum(i) == empty)
            {
                oscRoomData.SetBannerNum(i, Managers.instance.playerID);
                canMove = true;
                nextNum = i;
                break;
            }
        }

        //�ړ����悤�Ƃ����`�[���ɋ󂫂��Ȃ���Ή��������������^�[��
        if (canMove)
        {

            //�`�[���̈ړ��ɐ���������A�O���������̈ʒu���N���A����
            oscRoomData.SetBannerNum(myNum, MachingRoomData.bannerEmpty);
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

    bool TidyUpPlayerBanner(RoomData _roomData)
    {
        bool isTidied = false;

        for (int i = 0; i < MachingRoomData.bannerMaxCount - 2; i++)
        {
            //���g����Ȃ�
            if (_roomData.GetBannerNum(i) == empty)
            {
                //�P���̒��g���󂶂�Ȃ��Ȃ�
                if (_roomData.GetBannerNum(i + 2) != empty)
                {
                    //�P���̏��������̒��g�ɓ���ւ���
                    _roomData.SetBannerNum(i, _roomData.GetBannerNum(i + 2));
                    _roomData.SetBannerNum(i + 2, MachingRoomData.bannerEmpty);
                    //�����̔ԍ��������ꍇ�A�ԍ����X�V����
                    if (myNum == i) { myNum = i - 2; }

                    isTidied = true;
                }
            }
        }

        return isTidied;
    }

    public void CharaSelect(int _playerID, int value)
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        int calc = oscRoomData.GetSelectedCharacterID(_playerID);

        if (value > 0) { calc = (calc + 1) % maxCharaCount; }
        else { calc = (calc + (maxCharaCount - 1)) % maxCharaCount; }

        oscRoomData.SetSelectedCharacterID(_playerID, calc);

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }

    public void PressSubmit()
    {
        //DEBUG
        if (Managers.instance.onDebug)
        {
            OSCManager.OSCinstance.roomData.gameStart = true;
            OSCManager.OSCinstance.receiveRoomData.gameStart = true;
            return;
        }

        RoomData oscRoomData;
        int myID = Managers.instance.playerID;

        //�������z�X�g�Ȃ�
        if (0 == myID)
        {
            oscRoomData = OSCManager.OSCinstance.receiveRoomData;

            int readyCount = 0;
            for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
            {
                if (oscRoomData.GetReadyPlayers(i)) { readyCount++; }
            }

            Debug.Log(readyCount);
            
            if (readyCount >= Managers.instance.gameManager.allPlayerCount - 1)
            {
                OSCManager.OSCinstance.roomData.gameStart = true;
                Debug.Log("�V�[���ς����");
            }
        }
        else
        {
            oscRoomData = OSCManager.OSCinstance.roomData;

            if (!oscRoomData.GetReadyPlayers(myID))
            {
                oscRoomData.SetReadyPlayers(myID, true);
                OSCManager.OSCinstance.roomData = oscRoomData;
            }
        }

        SendDataToServer();
    }
    public void PressCancel()
    {
        RoomData oscRoomData = OSCManager.OSCinstance.roomData;

        int myID = Managers.instance.playerID;

        if (oscRoomData.hostPlayer == myID)
        {

        }
        else
        {
            if (oscRoomData.GetReadyPlayers(myID))
            {
                oscRoomData.SetReadyPlayers(myID, false);

            }
        }

        OSCManager.OSCinstance.roomData = oscRoomData;
        SendDataToServer();
    }
}
