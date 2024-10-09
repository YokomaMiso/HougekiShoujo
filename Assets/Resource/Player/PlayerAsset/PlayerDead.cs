using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    public float deadTimer = 0;
    public readonly float deadBehaviorTime = 3;
    Vector3 deadPos;
    public readonly Vector3 deadTargetPos = new Vector3(0, 90, -9);

    int killPlayerID = -1;
    public void SetKillPlayerID(int _num)
    {
        killPlayerID = _num;
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.killPlayerID = _num;

        SetKDFScore(_num);
    }
    public int GetKillPlayerID() { return killPlayerID; }

    void SetKDFScore(int _num)
    {
        int myID = Managers.instance.playerID;
        int deadID = ownerPlayer.GetPlayerID();
        int killerID = _num;

        //���S�����v���C���[���E�����v���C���[����������Ȃ��Ȃ瑁�����^�[��
        if (myID != deadID && myID != killerID) { return; }

        IngameData.GameData myGameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;

        //���������񂾂Ȃ�f�X�����{�P
        if (deadID == myID) { myGameData.deathCount++; }

        //�������E�����Ȃ�
        if (killerID == myID)
        {
            MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
            MachingRoomData.RoomData otherRoomData = OSCManager.OSCinstance.GetRoomData(deadID);

            //�����`�[���Ȃ�t�����h���[�t�@�C�A�����{�P
            if (otherRoomData.myTeamNum == myRoomData.myTeamNum) { myGameData.friendlyFireCount++; }
            //�Ⴄ�`�[���Ȃ�L�����{�P
            else { myGameData.killCount++; }
        }

        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData = myGameData;
    }

    public void Init()
    {
        deadTimer = 0;
        deadPos = Vector3.zero;
        killPlayerID = -1;
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.killPlayerID = -1;
    }

    public void SetDeadPos(Vector3 _pos) { deadPos = _pos; }

    public void DeadBehavior()
    {
        if (deadTimer > deadBehaviorTime) { return; }

        deadTimer += Managers.instance.timeManager.GetDeltaTime();
        transform.position = Vector3.Lerp(deadPos, deadPos + deadTargetPos, deadTimer / deadBehaviorTime);

    }
}
