using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceReset : MonoBehaviour
{
    Player ownerPlayer;
    public void SetOwnerPlayer(Player _player) { ownerPlayer = _player; }

    private void OnDestroy()
    {
        if (ownerPlayer != null) { ownerPlayer.ResetVoiceSoundID(); }
    }
}
