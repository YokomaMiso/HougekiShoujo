using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EMPExplosionBehavior : ExplosionBehavior
{
    bool[] hitedPlayer = new bool[8];

    protected override void Start()
    {
        lifeTime = 0.3f;
    }

    protected override void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) { return; }
        int id = player.GetPlayerID();
        if (hitedPlayer[id]) { return; }

        hitedPlayer[id] = true;
        other.AddComponent<SpeedBuff>().SetRateAndTime(0.0f, 2.0f);

        //トラップ被弾ボイスを鳴らす
        player.PlayVoice(player.GetPlayerData().GetPlayerVoiceData().GetDamageTrap(), Camera.main.transform);
    }
}
