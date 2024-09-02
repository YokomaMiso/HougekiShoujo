using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImage : MonoBehaviour
{
    Player ownerPlayer;
    public void SetPlayer(Player _player) { ownerPlayer = _player; }


    [SerializeField] SpriteRenderer charaSprite;
    [SerializeField] RuntimeAnimatorController idleAnimationController;
    [SerializeField] RuntimeAnimatorController runAnimationController;
    [SerializeField] RuntimeAnimatorController reloadAnimationController;
    [SerializeField] RuntimeAnimatorController aimAnimationController;
    [SerializeField] RuntimeAnimatorController recoilAnimationController;

    int[] animSpeed = new int[5] { 1, 2, 1, 1, 1 };


    void Start()
    {

    }

    void Update()
    {
        int num = (int)ownerPlayer.playerState;

        RuntimeAnimatorController applyController = idleAnimationController;
        switch (ownerPlayer.playerState)
        {
            case PLAYER_STATE.IDLE:
                applyController = idleAnimationController;
                break;
            case PLAYER_STATE.RUN:
                applyController = runAnimationController;
                break;
            case PLAYER_STATE.RELOADING:
                applyController = reloadAnimationController;
                break;
            case PLAYER_STATE.AIMING:
                applyController = aimAnimationController;
                break;
            case PLAYER_STATE.ATTACKING:
                applyController = recoilAnimationController;
                break;
        }
        charaSprite.GetComponent<Animator>().runtimeAnimatorController = applyController;
        charaSprite.GetComponent<Animator>().speed = animSpeed[num] * ownerPlayer.managerMaster.timeManager.GetDeltaTime();
    }
}
