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
    PlayerRadioChat playerRadioChat;
    PlayerDead playerDead;
    PlayerImage playerImage;
    PlayerNameCanvas playerNameCanvas;

    Collider myCollider;

    [SerializeField] GameObject blindCanvas;
    [SerializeField] GameObject nameCanvas;

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
    public void SetDead(int _num)
    {
        if (!alive) { return; }

        if (IsMine())
        {
            IngameData.GameData hostIngameData;
            if (Managers.instance.playerID == 0) { hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData; }
            else { hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData; }
            if (hostIngameData.end) { return; }

            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.alive = false;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.deadTime = hostIngameData.roundTimer;
            Camera.main.GetComponent<CameraMove>().ResetCameraFar();
        }
        alive = false;
        playerState = PLAYER_STATE.DEAD;
        playerDead.SetDeadPos(transform.position);
        playerDead.SetKillPlayerID(_num);
        if (myCollider) { myCollider.enabled = false; }
        Managers.instance.gameManager.AddKillLog(this);
    }
    public float GetDeadTimer() { return playerDead.deadTimer; }

    public GameObject GetPlayerImageObject() { return playerImage.GetCharaImageObject(); }

    public int GetKiller() { return playerDead.GetKillPlayerID(); }
    public void SetAlive() { alive = true; playerState = PLAYER_STATE.IDLE; }
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


    public void PlayEmote(RADIO_CHAT_ID _ID) { playerRadioChat.DisplayEmote(_ID); }

    public void ChangeShellIconColor(int _num)
    {
        if (playerNameCanvas == null) { return; }
        playerNameCanvas.ChangeShellIconColor(_num);
    }

    //For Other
    bool fire;
    bool useSub;
    PLAYER_STATE prevPlayerState;

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
        playerImage.Init();
        ChangeShellIconColor(0);

        fire = false;
        useSub = false;

        if (IsMine())
        {
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.fire = false;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.useSub = false;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.alive = true;
            OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.deadTime = 0;
        }

        myCollider.enabled = true;

        MachingRoomData.RoomData roomData;
        if (IsMine()) { roomData = OSCManager.OSCinstance.roomData; }
        else { roomData = OSCManager.OSCinstance.GetRoomData(playerID); }
        if (roomData.myTeamNum == (int)TEAM_NUM.B) { DirectionChange(Vector3.left); }
    }

    Material outLine;
    public void SetOutLineMat(Material _mat) { outLine = _mat; }
    public Material GetOutLineMat() { return outLine; }


    GameObject voiceSoundObject;
    int voiceSoundID = 0;
    public void PlayVoice(AudioClip _clip, Transform _transform = null, int _num = 0)
    {
        if (_num >= voiceSoundID)
        {
            if (voiceSoundObject != null) { Destroy(voiceSoundObject); }
            voiceSoundID = _num;
        }

        if (_transform == null) { _transform = transform; }

        voiceSoundObject = SoundManager.PlayVoice(_clip, _transform);
        voiceSoundObject.AddComponent<PlayerVoiceReset>().SetOwnerPlayer(this);
    }
    public void ResetVoiceSoundID() { voiceSoundID = 0; }

    void Start()
    {
        playerMove = gameObject.GetComponent<PlayerMove>();
        playerMove.SetPlayer(this);

        playerAim = gameObject.GetComponent<PlayerAim>();

        if (IsMine())
        {
            playerAim.SetPlayer(this, transform.GetChild(2).gameObject, transform.GetChild(1).gameObject);
            this.PlayVoice(playerData.GetPlayerVoiceData().GetGameStart(), Camera.main.transform);
        }
        else
        {
            playerAim.SetPlayer(this, null, null);
            fire = OSCManager.OSCinstance.GetIngameData(playerID).mainPacketData.inGameData.fire;
            useSub = OSCManager.OSCinstance.GetIngameData(playerID).mainPacketData.inGameData.useSub;
        }

        playerReload = gameObject.GetComponent<PlayerReload>();
        playerReload.SetPlayer(this);

        playerRecoil = gameObject.GetComponent<PlayerRecoil>();
        playerRecoil.SetPlayer(this);

        playerSubAction = gameObject.GetComponent<PlayerSubAction>();
        playerSubAction.SetPlayer(this);

        playerRadioChat = gameObject.GetComponent<PlayerRadioChat>();
        playerRadioChat.SetPlayer(this);

        playerDead = gameObject.GetComponent<PlayerDead>();
        playerDead.SetPlayer(this);

        playerImage = transform.GetChild(0).GetComponent<PlayerImage>();
        playerImage.SetPlayer(this);

        myCollider = GetComponent<Collider>();

        MachingRoomData.RoomData roomData;
        if (IsMine()) { roomData = OSCManager.OSCinstance.roomData; }
        else { roomData = OSCManager.OSCinstance.GetRoomData(playerID); }
        if (roomData.myTeamNum == (int)TEAM_NUM.B) { DirectionChange(Vector3.left); }

        if (Managers.instance.unlockFlag[(int)UNLOCK_ITEM.UI_DELETE])
        {
            Destroy(nameCanvas);
        }
        else
        {
            playerNameCanvas = nameCanvas.GetComponent<PlayerNameCanvas>();
            playerNameCanvas.SetPlayer(this);
            playerNameCanvas.SetName(roomData.playerName, roomData.myTeamNum);
        }
    }

    void Update()
    {
        IngameData.GameData hostIngameData;
        if (Managers.instance.playerID == 0) { hostIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData; }
        else { hostIngameData = OSCManager.OSCinstance.GetIngameData(0).mainPacketData.inGameData; }

        if (IsMine())
        {
            if (hostIngameData.play)
            {
                OwnPlayerBehavior();
            }
            else
            {
                playerState = PLAYER_STATE.IDLE;
                playerMove.Init();
                if (alive) { playerMove.MoveStop(); }
            }

            if (!alive) { playerDead.DeadBehavior(); }

            SetNetPos();
        }
        else
        {
            GetNetPosForOtherPlayer();
            IngameData.GameData myIngameData = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData;
            if (hostIngameData.start && alive && !myIngameData.alive) { SetDead(myIngameData.killPlayerID); }

            if (hostIngameData.play) { OtherPlayerBehavior(); }
            else { playerMove.Init(); }
            if (!alive) { playerDead.DeadBehavior(); }
        }
    }

    void OwnPlayerBehavior()
    {
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
                            if (playerData.GetShell().GetShellType() != SHELL_TYPE.SPECIAL) { canonState = (CANON_STATE)reloadNum; }
                            else { playerSubAction.CutOffReloadTime(5.0f); }
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
                            playerAim.ResetAim();
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
    }

    void SetNetPos()
    {
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerPos = transform.position;
        OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerState = playerState;
    }
    void GetNetPosForOtherPlayer()
    {
        playerState = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.playerState;
        transform.position = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.playerPos;
    }

    void OtherPlayerBehavior()
    {
        IngameData.GameData myIngameData = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData;

        Vector3 stickValue = myIngameData.playerStickValue;

        //if (alive && !myIngameData.alive) { SetDead(myIngameData.killPlayerID); }
        bool nowFire = myIngameData.fire;
        bool nowSub = myIngameData.useSub;

        playerMove.MoveForOther();

        if (OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.alive)
        {
            switch (playerState)
            {
                case PLAYER_STATE.RUN:
                case PLAYER_STATE.AIMING:
                    //移動に応じてキャラグラフィックの向き変更
                    DirectionChange(stickValue);
                    break;
                case PLAYER_STATE.RELOADING:
                    if (canonState != CANON_STATE.EMPTY) { break; }
                    if (!playerReload.reloadFlagForOther) { playerReload.Reload(0); }
                    break;
            }
        }

        if (fire != nowFire)
        {
            playerAim.Fire(transform.localScale);
            fire = nowFire;
            canonState = CANON_STATE.EMPTY;
        }
        if (useSub != nowSub)
        {
            playerSubAction.UseSubWeapon();
            useSub = nowSub;
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

    int prevInputFromUI = -1;
    public void SetInputFromUI(int _num) { prevInputFromUI = _num; }

    int InputCheck()
    {
        int reloadValue = -1;

        if (prevInputFromUI != -1)
        {
            reloadValue = prevInputFromUI;
            prevInputFromUI = -1;
            return reloadValue;
        }

        //if (Input.GetButtonDown("X")) { reloadValue = 0; }
        //if (Input.GetButtonDown("Y")) { reloadValue = 0; }
        if (InputManager.GetKeyDown(BoolActions.SouthButton)) { reloadValue = 0; }
        if (InputManager.GetKeyDown(BoolActions.EastButton)) { reloadValue = 1; }

        //if (Input.GetButtonUp("X")) { reloadValue = 3; }
        //if (Input.GetButtonUp("Y")) { reloadValue = 2; }
        if (InputManager.GetKeyUp(BoolActions.SouthButton)) { reloadValue = 2; }
        if (InputManager.GetKeyUp(BoolActions.EastButton)) { reloadValue = 3; }

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
        inputVector = _movement;

        if (_movement.x == 0) { return; }
        Vector3 imageScale = Vector3.one;
        if (_movement.x < 0) { imageScale.x *= -1; }
        playerImage.transform.localScale = imageScale;
    }

    public float NowDirection()
    {
        return playerImage.transform.localScale.x;
    }
}