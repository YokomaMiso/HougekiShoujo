using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBuff : MonoBehaviour
{
    PlayerReload playerReload;
    int buffNum;

    float speedRate = 2.0f;
    float lifeTime = 3.0f;

    float timer = 0;

    public void SetRateAndTime(float _rate, float _time)
    {
        speedRate = _rate;
        lifeTime = _time;
    }

    void Start()
    {
        if (GetComponent<PlayerReload>())
        {
            playerReload = GetComponent<PlayerReload>();
            buffNum = playerReload.AddSpeedRate(speedRate);
            //PlayerReload�Ƀo�t�𑗂ꂽ�烊�^�[��
            if (buffNum >= 0) { return; }
        }

        //����̂Ɏ��s�����炱�̃N���X��j��
        Destroy(this);
    }

    void Update()
    {
        timer += Managers.instance.timeManager.GetDeltaTime();
        if (timer > lifeTime)
        {
            playerReload.ResetSpeedRate(buffNum);
            Destroy(this);
        }
    }
}
