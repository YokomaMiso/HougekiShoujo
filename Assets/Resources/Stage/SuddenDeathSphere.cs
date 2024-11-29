using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenDeathSphere : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        Player player= other.GetComponent<Player>();
        if (!player) { return; }
        if (player.GetPlayerID() != Managers.instance.playerID) { return; }

        player.SetDead(player.GetPlayerID());
    }
}
