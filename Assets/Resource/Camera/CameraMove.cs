using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] GameObject spectateAnnouncePrefab;
    GameObject spectateAnnounceInstance;

    GameObject audioListenerChild;

    const float distance = 10;
    Player ownerPlayer;
    int nowPlayerID;

    static readonly Vector3 ingameRotation = new Vector3(45, 0, 0);
    float initialTimer;

    float shakeValue;

    float targetFar = 5;
    const float defaultFar = 5;
    float aimTimer;

    void Start()
    {
        nowPlayerID = Managers.instance.playerID;

        audioListenerChild = transform.GetChild(0).gameObject;

        //初回、キャンバスが生成されていないなら
        if (spectateAnnounceInstance == null)
        {
            spectateAnnounceInstance = Instantiate(spectateAnnouncePrefab);
            spectateAnnounceInstance.GetComponent<SpectateCameraAnnounce>().Display(false);
        }
    }

    public void SetPlayer(Player _player) { ownerPlayer = _player; }

    void Update()
    {
        if (Managers.instance.state != GAME_STATE.IN_GAME) { return; }
        if (initialTimer < 1) { InitialAngle(); }
        else
        {
            CameraShakeUpdate();
            FarUpdate();
            Player nowPlayer;

            spectateAnnounceInstance.GetComponent<SpectateCameraAnnounce>().Display(!ownerPlayer.GetAlive());
            if (ownerPlayer.GetAlive())
            {
                nowPlayer = ownerPlayer;
                nowPlayerID = Managers.instance.playerID;
            }
            else
            {
                TargetChange();
                nowPlayer = Managers.instance.gameManager.GetPlayer(nowPlayerID);
            }

            if (nowPlayerID >= 0 && nowPlayer.GetAlive())
            {
                Move(nowPlayer.transform.position);
            }

        }
    }

    void TargetChange()
    {
        //canChangePlayerが１人でも入れば、カメラを切り替えることが出来る
        //LBが押されたら
        if (InputManager.GetKeyDown(BoolActions.LeftShoulder))
        {
            int maxNum = Managers.instance.gameManager.GetPlayerCount();

            for (int i = 1; i < maxNum; i++)
            {
                int nowID = (i + nowPlayerID) % maxNum;

                //プレイヤーが居ない or 他チームなら早期リターン
                MachingRoomData.RoomData roomData = OSCManager.OSCinstance.GetRoomData(nowID);
                MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
                if (roomData.myID == -1 || roomData.myTeamNum != myRoomData.myTeamNum) { continue; }
                
                //nowPlayerに現在の値を入れて抜ける
                nowPlayerID = nowID;
                break;
            }
        }
    }
    void InitialAngle()
    {
        initialTimer += Time.deltaTime;
        if (initialTimer >= 1) { initialTimer = 1; }

        Vector3 angle = Vector3.Lerp(Vector3.zero, ingameRotation, initialTimer);
        transform.rotation = Quaternion.Euler(angle);


        Vector3 playerPos = ownerPlayer.transform.position;
        audioListenerChild.transform.position = playerPos;
        Vector3 pos = Vector3.Lerp(new Vector3(1.0f, 1.5f, -2.0f), playerPos + new Vector3(0, distance, -distance), initialTimer);
        transform.position = pos;
    }

    void CameraShakeUpdate()
    {
        if (shakeValue == 0) { return; }

        shakeValue -= Managers.instance.timeManager.GetDeltaTime();
        if (shakeValue < 0) { shakeValue = 0; }
    }
    void FarUpdate()
    {
        Camera camera = GetComponent<Camera>();
        if (camera.orthographicSize == targetFar) { return; }

        aimTimer += Time.deltaTime;
        if (aimTimer > 1) { aimTimer = 1; }
        float nowFar = Mathf.MoveTowards(camera.orthographicSize, targetFar, aimTimer);
        camera.orthographicSize = nowFar;
    }

    public void Move(Vector3 _pos)
    {
        Vector3 shakeVector3 = Random.insideUnitCircle * shakeValue;
        transform.position = _pos + new Vector3(0, distance, -distance) + shakeVector3;
        audioListenerChild.transform.position = _pos;
    }
    public void SetCameraShake(float _shakeValue)
    {
        if (!Managers.instance.GetOptionData().cameraShakeOn) { return; }

        if (_shakeValue <= shakeValue) { return; }

        shakeValue = _shakeValue;
    }
    public void SetCameraFar(float _far) { targetFar = _far; }
    public void ResetCameraFar() { targetFar = defaultFar; aimTimer = 0; }

}
