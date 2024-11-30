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

    public GameObject GetCharaImageObject() { return charaSprite.gameObject; }

    void Start()
    {
        animData = ownerPlayer.GetPlayerData().GetCharacterAnimData();
        charaSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (charaSprite != null)
        {
            charaSprite.shadowCastingMode = ShadowCastingMode.On;
            charaSprite.material = ownerPlayer.GetOutLineMat();
        }
    }

    public void Init()
    {
        charaSprite.color = Color.white;
    }

    void Update()
    {
        int num = (int)ownerPlayer.playerState;
        float animSpeedRate = 1.0f;

        RuntimeAnimatorController applyController;
        switch (ownerPlayer.playerState)
        {
            default://case PLAYER_STATE.IDLE:
                applyController = animData.GetIdleAnim((CANON_STATE)ownerPlayer.GetCanonState(), ownerPlayer.NowDirection());
                break;
            case PLAYER_STATE.RUN:
                applyController = animData.GetRunAnim(0, (CANON_STATE)ownerPlayer.GetCanonState(), ownerPlayer.NowDirection());
                break;
            case PLAYER_STATE.RELOADING:
                applyController = animData.GetReloadAnim(ownerPlayer.NowDirection());
                break;
            case PLAYER_STATE.AIMING:
                float time = charaSprite.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
                applyController = animData.GetAimAnim(ownerPlayer.AnimNumFromVector(), ownerPlayer.NowDirection());
                charaSprite.GetComponent<Animator>().ForceStateNormalizedTime(time + Managers.instance.timeManager.GetDeltaTime());
                //charaSprite.GetComponent<Animator>().Play(0, -1, time);
                break;
            case PLAYER_STATE.ATTACKING:
                applyController = animData.GetRecoilAnim(ownerPlayer.AnimNumFromVector(), ownerPlayer.NowDirection());
                break;
            case PLAYER_STATE.DEAD:
                applyController = animData.GetDeadAnim();
                if (ownerPlayer.GetDeadTimer() >= 3) { charaSprite.color = Color.clear; }
                break;
        }

        charaSprite.GetComponent<Animator>().runtimeAnimatorController = applyController;
        charaSprite.GetComponent<Animator>().speed = animSpeedRate * Managers.instance.timeManager.TimeRate();
    }
}
