using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE = 0, RUN, RELOADING, AIMING, ATTACKING }
public enum CANON_STATE { EMPTY = -1, RELOADED=0 }

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData playerData;

    PlayerMove playerMove;
    PlayerReload playerReload;
    PlayerAim playerAim;
    PlayerRecoil playerRecoil;
    PlayerImage playerImage;

    int playerID = 0;
    public int SetPlayerID(int _id) { playerID = _id;  return playerID; }
    public int GetPlayerID() { return playerID; }
    public PlayerData GetPlayerData() { return playerData; }

    public PLAYER_STATE playerState = PLAYER_STATE.IDLE;
    public CANON_STATE canonState = CANON_STATE.EMPTY;
    public int GetCanonState() { return (int)canonState; }

    void Start()
    {
        playerMove = gameObject.GetComponent<PlayerMove>();
        playerMove.SetPlayer(this);

        playerReload = gameObject.GetComponent<PlayerReload>();
        playerReload.SetPlayer(this);

        playerAim = gameObject.GetComponent<PlayerAim>();
        playerAim.SetPlayer(this, transform.GetChild(2).gameObject, transform.GetChild(1).gameObject);

        playerRecoil = gameObject.GetComponent<PlayerRecoil>();
        playerRecoil.SetPlayer(this);

        playerImage = transform.GetChild(0).GetComponent<PlayerImage>();
        playerImage.SetPlayer(this);

        //プレイヤーが自分のキャラクターなら
        if (playerID == 0)
        {
            Camera.main.GetComponent<CameraMove>().SetPlayer(this);
        }
    }

    void Update()
    {
        if (Managers.instance.state != GAME_STATE.IN_GAME) { return; }

        //DEBUG
        //if (Input.GetKeyDown(KeyCode.X)) { TimeManager.slow = !TimeManager.slow; }

        int inputNum = InputCheck();

        if (InAction())
        {
            switch (playerState)
            {
                case PLAYER_STATE.RELOADING:
                    int reloadNum = playerReload.ReloadBehavior();
                    if (reloadNum == -1) { playerState = PLAYER_STATE.RELOADING; }
                    else
                    {
                        canonState = (CANON_STATE)reloadNum;
                        playerState = PLAYER_STATE.IDLE;
                    }
                    break;
                case PLAYER_STATE.AIMING:
                    if (inputNum == -1)
                    {
                        //エイムの移動
                        Vector3 movement;
                        movement = playerAim.AimMove();
                        //移動に応じてキャラグラフィックの向き変更
                        DirectionChange(movement);
                    }
                    else if (inputNum - 2 == GetCanonState())
                    {
                        playerAim.Fire(playerImage.transform.localScale);
                        playerState = PLAYER_STATE.ATTACKING;
                        playerRecoil.SetRecoil();
                        canonState = CANON_STATE.EMPTY;
                    }
                    break;
                case PLAYER_STATE.ATTACKING:
                    if (!playerRecoil.GetIsRecoil()) { playerState = PLAYER_STATE.IDLE; }
                    break;
            }
        }
        else
        {
            switch (inputNum)
            {
                case -1:
                    MoveBehavior();
                    break;
                case 0:
                    if (canonState == CANON_STATE.EMPTY)
                    {
                        playerReload.Reload(inputNum);
                        playerState = PLAYER_STATE.RELOADING;
                    }
                    else
                    {
                        if (GetCanonState() == inputNum)
                        {
                            playerAim.AimStart(playerData.GetShell());
                            playerState = PLAYER_STATE.AIMING;
                        }
                    }
                    break;
                case 1:
  
                    break;
            }


        }
    }

    bool InAction()
    {
        switch (playerState)
        {
            case PLAYER_STATE.IDLE:
                return false;
            case PLAYER_STATE.RUN:
                return false;
            case PLAYER_STATE.RELOADING:
                return true;
            case PLAYER_STATE.AIMING:
                return true;
            case PLAYER_STATE.ATTACKING:
                return true;
        }
        return false;
    }

    int InputCheck()
    {
        int reloadValue = -1;
        //if (Input.GetButtonDown("X")) { reloadValue = 0; }
        if (Input.GetButtonDown("Y")) { reloadValue = 0; }
        if (Input.GetButtonDown("Cancel")) { reloadValue = 1; }

        //if (Input.GetButtonUp("X")) { reloadValue = 3; }
        if (Input.GetButtonUp("Y")) { reloadValue = 2; }
        if (Input.GetButtonUp("Cancel")) { reloadValue = 3; }

        return reloadValue;
    }
    void MoveBehavior()
    {
        //移動の適用
        Vector3 movement;
        movement = playerMove.Move();

        //移動に応じてグラフィックの向き変更
        DirectionChange(movement);

        if (movement == Vector3.zero) { playerState = PLAYER_STATE.IDLE; }
        else { playerState = PLAYER_STATE.RUN; }
    }

    void DirectionChange(Vector3 _movement)
    {
        if (_movement.x == 0) { return; }

        Vector3 imageScale = Vector3.one;
        if (_movement.x < 0) { imageScale.x *= -1; }
        playerImage.transform.localScale = imageScale;
    }
}