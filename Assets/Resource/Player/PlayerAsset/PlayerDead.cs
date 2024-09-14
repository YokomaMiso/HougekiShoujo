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
    public readonly Vector3 deadTargetPos = new Vector3(0, 150, -9);

    public void Init()
    {
        deadTimer = 0;
        deadPos = Vector3.zero;
    }

    public void SetDeadPos(Vector3 _pos) { deadPos = _pos; }

    public void DeadBehavior()
    {
        if (deadTimer > deadBehaviorTime) { return; }

        deadTimer += Managers.instance.timeManager.GetDeltaTime();
        transform.position = Vector3.Lerp(deadPos, deadTargetPos, deadTimer / deadBehaviorTime);
    }
}
