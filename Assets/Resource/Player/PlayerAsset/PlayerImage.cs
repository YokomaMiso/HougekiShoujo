using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImage : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    SpriteRenderer charaSprite;
    CharacterAnimData animData;
    int[] animSpeed = new int[5] { 1, 2, 1, 1, 1 };

    void Start()
    {
        charaSprite=transform.GetChild(0).GetComponent<SpriteRenderer>();
        animData = ownerPlayer.GetPlayerData().GetCharacterAnimData();
    }

    void Update()
    {
        int num = (int)ownerPlayer.playerState;

        RuntimeAnimatorController applyController = ownerPlayer.GetPlayerData().GetCharacterAnimData().GetIdleAnim();
        switch (ownerPlayer.playerState)
        {
            /*
            case PLAYER_STATE.IDLE:
                applyController = animData.GetIdleAnim();
                break;
            */
            case PLAYER_STATE.RUN:
                applyController = animData.GetRunAnim(0);
                break;
            case PLAYER_STATE.RELOADING:
                applyController = animData.GetReloadAnim();
                break;
            case PLAYER_STATE.AIMING:
                applyController = animData.GetAimAnim(0);
                break;
            case PLAYER_STATE.ATTACKING:
                applyController = animData.GetRecoilAnim(0);
                break;
        }

        charaSprite.GetComponent<Animator>().runtimeAnimatorController = applyController;
        charaSprite.GetComponent<Animator>().speed = animSpeed[num] * Managers.instance.timeManager.TimeRate();
    }
}
