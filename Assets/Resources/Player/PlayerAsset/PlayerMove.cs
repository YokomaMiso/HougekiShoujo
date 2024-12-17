using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    [SerializeField] BuffEffectBehavior buffVfxBehavior;
    [SerializeField] SlipDeBuffEffectBehavior debuffVfxBehavior;

    //移動速度
    float speed = 5;
    //バフデバフ用の変数
    float[] speedRate = new float[6] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    SpeedBuff[] buffs = new SpeedBuff[6] { null, null, null, null, null, null };
    const float defaultSpeedRate = 1.0f;
    //スリップデバフ用の変数
    float slipTimer = 0;
    Vector3 slipVector;

    private void Start()
    {
        speed = ownerPlayer.GetPlayerData().GetMoveSpeed();
    }

    public void Init()
    {
        for (int i = 0; i < buffs.Length; i++)
        {
            if (buffs[i] != null) { buffs[i].ResetBuff(); }
        }
        slipTimer = 0;
        buffVfxBehavior.DisplayBuff(1.0f);
        debuffVfxBehavior.DisplayBuff(1.0f);
    }

    public void MoveForOther() { NowSpeedRate(); }

    public Vector3 Move()
    {
        if (!ownerPlayer.IsMine()) { return Vector3.zero; }

        Vector3 movement = Vector3.zero;
        movement += Vector3.right * InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis).x;
        movement += Vector3.forward * InputManager.GetAxis<Vector2>(Vec2AxisActions.LStickAxis).y;
        if (movement == Vector3.zero)
        {
            movement += Vector3.right * ownerPlayer.GetInputVectorFromUI().x;
            movement += Vector3.forward * ownerPlayer.GetInputVectorFromUI().y;
        }
        movement = movement.normalized;

        float currentSpeed = speed * NowSpeedRate() * Managers.instance.timeManager.TimeRate();

        if (slipTimer > 0)
        {
            ownerPlayer.GetComponent<Rigidbody>().velocity = SlipUpdate(movement * currentSpeed);
        }
        else
        {
            ownerPlayer.GetComponent<Rigidbody>().velocity = movement * currentSpeed;
        }

        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerStickValue = movement;

        return movement;
    }

    public void MoveStop()
    {
        if (slipTimer > 0)
        {
            ownerPlayer.GetComponent<Rigidbody>().velocity = SlipUpdate(Vector3.zero);
        }
        else
        {
            ownerPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public int AddSpeedRate(SpeedBuff _buff, float _rate)
    {
        int emptyNum = -1;
        for (int i = 0; i < speedRate.Length; i++)
        {
            if (speedRate[i] == defaultSpeedRate)
            {
                emptyNum = i;
                speedRate[i] = _rate;
                buffs[i] = _buff;
                break;
            }
        }

        return emptyNum;
    }
    public void ResetSpeedRate(int _num) { speedRate[_num] = defaultSpeedRate; }
    float NowSpeedRate()
    {
        float multiplyAllRate = 1.0f;
        for (int i = 0; i < speedRate.Length; i++) { multiplyAllRate *= speedRate[i]; }
        buffVfxBehavior.DisplayBuff(multiplyAllRate);
        return multiplyAllRate;
    }

    Vector3 SlipUpdate(Vector3 _movement)
    {
        float deltaTime = Managers.instance.timeManager.GetDeltaTime();
        slipTimer -= deltaTime;
        float value = Mathf.Clamp01(1.0f - slipTimer);
        debuffVfxBehavior.DisplayBuff(value);

        slipVector = Vector3.Lerp(slipVector, _movement * 2, deltaTime / 2);
        return slipVector;
    }

    public void ReceiveSlip(float _timer, Vector3 _vector = default(Vector3))
    {
        if (slipTimer < _timer) { slipTimer = _timer; }
        Vector3 addVector;
        if (_vector == Vector3.zero) { addVector = ownerPlayer.GetComponent<Rigidbody>().velocity; }
        else { addVector = _vector; }
        slipVector = addVector;
    }
}
