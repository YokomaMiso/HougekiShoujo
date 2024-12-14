using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenDeathSphere : MonoBehaviour
{
    [SerializeField] AudioClip damageSFX;

    void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) { return; }
        if (player.GetPlayerID() != Managers.instance.playerID) { return; }

        player.SetDead(player.GetPlayerID());
        SoundManager.PlaySFX(damageSFX, player.transform);
    }
}
