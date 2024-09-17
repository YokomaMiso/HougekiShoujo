using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum PLAYER_STATE { IDLE = 0, RUN, RELOADING, AIMING, ATTACKING, DEAD }
public enum CANON_STATE { EMPTY = -1, RELOADED = 0 }

public class Player : MonoBehaviour
{
    PlayerData playerData;

    PlayerMove playerMove;
    PlayerReload playerReload;
    PlayerAim playerAim;
    PlayerRecoil playerRecoil;
    PlayerSubAction playerSubAction;
    PlayerDead playerDead;
    PlayerImage playerImage;

    int playerID;
    public void SetPlayerID(int _id) { playerID = _id; }
    public int GetPlayerID() { return playerID; }

    public void SetPlayerData(PlayerData _playerData) { playerData = _playerData; }
    public PlayerData GetPlayerData() { return playerData; }

    public PLAYER_STATE playerState = PLAYER_STATE.IDLE;
    public CANON_STATE canonState = CANON_STATE.EMPTY;
    public int GetCanonState() { return (int)canonState; }
    public float GetSubWeaponReload() { return playerSubAction.ReloadTime(); }

    public bool IsMine() { return playerID == Managers.instance.playerID; }

    bool alive = true;
    public void SetDead()
    {
        alive = false;
        playerState = PLAYER_STATE.DEAD;
        playerDead.SetDeadPos(transform.position);
        if (GetComponent<Collider>()) { Destroy(GetComponent<Collider>()); }
        if (GetComponent<Rigidbody>()) { Destroy(GetComponent<Rigidbody>()); }
    }
    public float GetDeadTimer() { return playerDead.deadTimer; }
    public bool GetAlive() { return alive; }

    Vector3 inputVector;
    public Vector3 GetInputVector() { return inputVector; }
    public int AnimNumFromVector()
    {
        Vector3 normalizedVector = Vector3.Normalize(inputVector);
        if (normalizedVector.x < 0) { normalizedVector.x *= -1; }
        float angle = Mathf.Atan2(normalizedVector.x, normalizedVector.z) * Mathf.Rad2Deg;

        const float borderAngle = 45f;  //45度ずつで返却する番号を変える
        const float defaultAngle = borderAngle / 2; //22.5度からスタート

        //22.5fまで…0, 67.5fまで…1, 112.5fまで…2, 157.5fまで…3, それ以上…4 
        for (int i = 0; i < 4; i++)
        {
            if (angle < defaultAngle + borderAngle * i) { return i; }
        }
        return 4;
    }

    public float GetReloadAnimSpeedRate() { return playerReload.NowSpeedRate(); }

    public void RoundInit()
    {
        playerState = PLAYER_STATE.IDLE;
        canonState = CANON_STATE.EMPTY;
        inputVector = Vector3.zero;
        alive = true;

        //playerMove.Init();
        playerReload.Init();
        playerAim.Init();
        playerRecoil.Init();
        playerSubAction.Init();
        playerDead.Init();
        //playerImage;
    }

    void Start()
    {
        playerMove = gameObject.GetComponent<PlayerMove>();
        playerMove.SetPlayer(this);

        playerAim = gameObject.GetComponent<PlayerAim>();

        if (IsMine())
        {
            playerAim.SetPlayer(this, transform.GetChild(2).gameObject, transform.GetChild(1).gameObject);
        }
        else
        {
            playerAim.SetPlayer(this, null, null);
        }

        playerReload = gameObject.GetComponent<PlayerReload>();
        playerReload.SetPlayer(this);

        playerRecoil = gameObject.GetComponent<PlayerRecoil>();
        playerRecoil.SetPlayer(this);

        playerSubAction = gameObject.GetComponent<PlayerSubAction>();
        playerSubAction.SetPlayer(this);

        playerDead = gameObject.GetComponent<PlayerDead>();
        playerDead.SetPlayer(this);

        playerImage = transform.GetChild(0).GetComponent<PlayerImage>();
        playerImage.SetPlayer(this);
    }

    void Update()
    {
        if (Managers.instance.gameManager.play)
        {
            if (IsMine())
            {
                //if (Managers.instance.state != GAME_STATE.IN_GAME) { return; }
                OwnPlayerBehavior();
            }
            else { OtherPlayerBehavior(); }
        }
        else
        {
            if (!alive) { playerDead.DeadBehavior(); }
        }
    }

    void OwnPlayerBehavior()
    {
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.fire = false;
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.useSub = false;

        int inputNum = InputCheck();

        if (alive)
        {
            if (InAction())
            {
                playerMove.MoveStop();

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
                        else if (inputNum == 1)
                        {
                            playerState = PLAYER_STATE.IDLE;
                            Camera.main.GetComponent<CameraMove>().ResetCameraFar();
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
                                playerAim.AimStart();
                                playerState = PLAYER_STATE.AIMING;
                            }
                        }
                        break;
                    case 1:
                        playerSubAction.UseSubWeapon();
                        break;
                }
            }
        }
        else
        {
            playerDead.DeadBehavior();
        }
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerPos = transform.position;
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerState = playerState;
    }

    void OtherPlayerBehavior()
    {
        playerState = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.playerState;
        transform.position = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.playerPos;
        Vector3 stickValue = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.playerStickValue;

        bool fire = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.fire;
        bool useSub = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.useSub;

        if (alive)
        {
            switch (playerState)
            {
                case PLAYER_STATE.RUN:
                case PLAYER_STATE.AIMING:
                    //移動に応じてキャラグラフィックの向き変更
                    DirectionChange(stickValue);
                    break;
            }
        }
        else
        {
            playerDead.DeadBehavior();
        }
        if (fire) { playerAim.Fire(transform.localScale); }
        if (useSub) { playerSubAction.UseSubWeapon(); }

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

        inputVector = _movement;

        Vector3 imageScale = Vector3.one;
        if (_movement.x < 0) { imageScale.x *= -1; }
        playerImage.transform.localScale = imageScale;
    }
}