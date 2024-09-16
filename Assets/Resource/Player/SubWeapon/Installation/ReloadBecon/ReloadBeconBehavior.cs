using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReloadBeconBehavior : InstallationBehavior
{
    bool[] hitedPlayer = new bool[8];
    float reloadSpeedRate;
    float buffLifeTime;

    void Start()
    {
        lifeTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetLifeTime();
        reloadSpeedRate = ownerPlayer.GetPlayerData().GetSubWeapon().GetSpeedRate();
        buffLifeTime = ownerPlayer.GetPlayerData().GetSubWeapon().GetLifeTime() - 2.0f;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) { return; }

        int id = player.GetPlayerID();
        if (hitedPlayer[id]) { return; }

        hitedPlayer[id] = true;

        player.AddComponent<ReloadBuff>().SetRateAndTime(reloadSpeedRate, buffLifeTime);
    }
}
