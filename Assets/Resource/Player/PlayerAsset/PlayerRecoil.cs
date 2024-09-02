using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoil : MonoBehaviour
{
    const float recoilTime = 1;
    float timer;
    bool isRecoil;

    public void SetRecoil() { isRecoil = true; }
    public bool GetIsRecoil() { return isRecoil; }

    void Update()
    {
        if (isRecoil)
        {
            timer += TimeManager.GetDeltaTime();
            if (timer > recoilTime)
            {
                isRecoil = false;
                timer = 0;
            }
        }
    }
}
