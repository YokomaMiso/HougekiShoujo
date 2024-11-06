using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunDamageArea : MonoBehaviour
{
    protected Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    private void OnTriggerStay(Collider other)
    {
        //当たったオブジェクトからPlayer型を取得
        Player player = other.GetComponent<Player>();

        //Player型でなければ早期リターン
        if (!player) { return; }

        //自分のキャラクターじゃなければ早期リターン
        if (player && player.GetPlayerID() != Managers.instance.playerID) { return; }

        //自分のキャラクターの死亡判定を行う
        other.GetComponent<Player>().SetDead(ownerPlayer.GetPlayerID());
    }
}
