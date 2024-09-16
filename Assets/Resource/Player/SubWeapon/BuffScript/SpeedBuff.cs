using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    PlayerMove playerMove;
    int buffNum;

    float speedRate = 1.0f;
    float lifeTime = 3.0f;

    float timer = 0;

    public void SetRateAndTime(float _rate, float _time)
    {
        speedRate = _rate;
        lifeTime = _time;
    }

    void Start()
    {
        if (GetComponent<PlayerMove>())
        {
            playerMove = GetComponent<PlayerMove>();
            buffNum = playerMove.AddSpeedRate(speedRate);
            //PlayerMoveにバフを送れたらリターン
            if (buffNum >= 0) { return; }
        }

        //送るのに失敗したらこのクラスを破棄
        Destroy(this);
    }

    void Update()
    {
        timer += Managers.instance.timeManager.GetDeltaTime();
        if (timer > lifeTime)
        {
            playerMove.ResetSpeedRate(buffNum);
            Destroy(this);
        }
    }
}
