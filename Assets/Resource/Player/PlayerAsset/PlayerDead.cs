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
    public readonly Vector3 deadTargetPos = new Vector3(0, 50, -9);

    int killPlayerID = -1;
    public void SetKillPlayerID(int _num) 
    {
        killPlayerID = _num; 
       OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.killPlayerID = _num;
    }
    public int GetKillPlayerID() {  return killPlayerID; }

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
