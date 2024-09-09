using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PLAYER_STATE { IDLE = 0, RUN, RELOADING, AIMING, ATTACKING }
public enum CANON_STATE { EMPTY = -1, RELOADED = 0 }

public class Player : MonoBehaviour
{
    PlayerData playerData;

    PlayerMove playerMove;
    PlayerReload playerReload;
    PlayerAim playerAim;
    PlayerRecoil playerRecoil;
    PlayerSubAction playerSubAction;
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

    bool IsMine() { return playerID == Managers.instance.playerID; }

    bool alive = true;
    public void SetDead()
    {
        alive = false;
        if (GetComponent<Collider>()) { Destroy(GetComponent<Collider>()); }
        if (GetComponent<Rigidbody>()) { Destroy(GetComponent<Rigidbody>()); }
    }
    public bool GetAlive() { return alive; }

    Vector3 inputVector;
    public Vector3 GetInputVector() { return inputVector; }
    public int AnimNumFromVector()
    {
        Vector3 normalizedVector = Vector3.Normalize(inputVector);
        if (normalizedVector.x < 0) { normalizedVector.x *= -1; }
        float angle = Mathf.Atan2(normalizedVector.x, normalizedVector.z) * Mathf.Rad2Deg;

        if (angle < 22.5f) { return 0; }
        else if (angle < 22.5f + 45) { return 1; }
        else if (angle < 22.5f + 45 * 2) { return 2; }
        else if (angle < 22.5f + 45 * 3) { return 3; }
        else { return 4; }
    }

    void Start()
    {
        if (IsMine())
        {
            playerMove = gameObject.GetComponent<PlayerMove>();
            playerMove.SetPlayer(this);

            playerReload = gameObject.GetComponent<PlayerReload>();
            playerReload.SetPlayer(this);

            playerAim = gameObject.GetComponent<PlayerAim>();
            playerAim.SetPlayer(this, transform.GetChild(2).gameObject, transform.GetChild(1).gameObject);

            playerRecoil = gameObject.GetComponent<PlayerRecoil>();
            playerRecoil.SetPlayer(this);

            playerSubAction = gameObject.GetComponent<PlayerSubAction>();
            playerSubAction.SetPlayer(this);
        }
        playerImage = transform.GetChild(0).GetComponent<PlayerImage>();
        playerImage.SetPlayer(this);
    }

    void Update()
    {
        if (Managers.instance.state != GAME_STATE.IN_GAME) { return; }

        //DEBUG
        //if (Input.GetKeyDown(KeyCode.X)) { TimeManager.slow = !TimeManager.slow; }
        //if (Input.GetKeyDown(KeyCode.X)) { transform.AddComponent<SpeedBuff>().SetRateAndTime(2, 5); }
        //if (Input.GetKeyDown(KeyCode.Z)) { transform.AddComponent<SpeedBuff>().SetRateAndTime(1.0f/2, 5); }

        if (IsMine()) { OwnPlayerBehavior(); }
        else
        {
            transform.position = OSCManager.OSCinstance.receivedData.mainPacketData.inGameData.playerPos;
        }
    }

    void OwnPlayerBehavior()
    {
        int inputNum = InputCheck();

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
                        //�G�C���̈ړ�
                        Vector3 movement;
                        movement = playerAim.AimMove();
                        //�ړ��ɉ����ăL�����O���t�B�b�N�̌����ύX
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

        OSCManager.OSCinstance.myNetData.mainPacketData.inGameData.playerPos = transform.position;
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
        //�ړ��̓K�p
        Vector3 movement;
        movement = playerMove.Move();

        //�ړ��ɉ����ăO���t�B�b�N�̌����ύX
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