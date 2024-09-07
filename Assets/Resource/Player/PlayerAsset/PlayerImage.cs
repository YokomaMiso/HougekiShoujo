using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class PlayerImage : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    SpriteRenderer charaSprite;
    CharacterAnimData animData;

    Material MaterialData;

    int[] animSpeed = new int[5] { 1, 1, 1, 1, 1 };


    void Start()
    {
        animData = ownerPlayer.GetPlayerData().GetCharacterAnimData();
        charaSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        MaterialData = ownerPlayer.GetPlayerData().GetMaterial();

        if (charaSprite != null)
        {
            charaSprite.shadowCastingMode = ShadowCastingMode.On;
            charaSprite.material = MaterialData;
        }

    }

    void Update()
    {
        int num = (int)ownerPlayer.playerState;

        RuntimeAnimatorController applyController;// = ownerPlayer.GetPlayerData().GetCharacterAnimData().GetIdleAnim();
        switch (ownerPlayer.playerState)
        {
            default://case PLAYER_STATE.IDLE:
                applyController = animData.GetIdleAnim();
                break;
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
