using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReloadBeconBehavior : InstallationBehavior
{
    bool[] hitedPlayer = new bool[8];
    float reloadSpeedRate;
    float buffLifeTime;

    protected override void Start()
    {
        base.Start();
        reloadSpeedRate = ownerPlayer.GetPlayerData().GetSubWeapon().GetSpeedRate();
        buffLifeTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetLifeTime() - 2.0f;
        changeToLoopAnimTime = 1.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (applyLoop) { return; }
        if (timer > changeToLoopAnimTime)
        {
            imageAnimator.runtimeAnimatorController = loopAnim;
            transform.GetChild(1).gameObject.SetActive(true);
            applyLoop = true;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (timer < changeToLoopAnimTime) { return; }

        Player player = other.GetComponent<Player>();
        if (!player) { return; }

        int id = player.GetPlayerID();
        if (hitedPlayer[id]) { return; }

        hitedPlayer[id] = true;

        player.AddComponent<ReloadBuff>().SetRateAndTime(reloadSpeedRate, buffLifeTime);
    }
}
