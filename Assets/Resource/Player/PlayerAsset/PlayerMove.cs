using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    [SerializeField] BuffEffectBehavior vfxBehavior;

    //移動速度
    float speed = 5;
    //バフデバフ用の変数
    float[] speedRate = new float[8] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    const float defaultSpeedRate = 1.0f;

    private void Start()
    {
        speed = ownerPlayer.GetPlayerData().GetMoveSpeed();
    }

    public Vector3 Move()
    {
        if (!ownerPlayer.IsMine()) { return Vector3.zero; }

        Vector3 movement = Vector3.zero;
        movement += Vector3.right * Input.GetAxis("Horizontal");
        movement += Vector3.forward * Input.GetAxis("Vertical");

        float currentSpeed = speed * NowSpeedRate() * Managers.instance.timeManager.TimeRate();
        ownerPlayer.GetComponent<Rigidbody>().velocity = movement * currentSpeed;

        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerStickValue = movement;

        return movement;
    }

    public void MoveStop() { ownerPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero; }

    public int AddSpeedRate(float _rate)
    {
        int emptyNum = -1;
        for (int i = 0; i < speedRate.Length; i++)
        {
            if (speedRate[i] == defaultSpeedRate)
            {
                emptyNum = i;
                speedRate[i] = _rate;
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
        vfxBehavior.DisplayBuff(multiplyAllRate);
        return multiplyAllRate;
    }
}
