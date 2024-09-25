using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float distance = 10;
    [SerializeField] float asistValue = 2;
    Player ownerPlayer;
    Player nowPlayer;

    static readonly Vector3 ingameRotation = new Vector3(45, 0, 0);
    float initialTimer;

    float shakeValue;

    float targetFar = 5;
    const float defaultFar = 5;
    float aimTimer;

    void Start()
    {
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
            if (ownerPlayer.GetAlive()) { Move(ownerPlayer.transform.position); }
            else
            {
                TargetChange();
                if (nowPlayer) { Move(nowPlayer.transform.position); }
            }
        }
    }

    void TargetChange()
    {
        if (nowPlayer != null && !nowPlayer.GetAlive()) { nowPlayer = null; }

        int changeNum = -1;
        if (Input.GetButtonDown("RB")) { changeNum = 0; }
        else if (Input.GetButtonDown("LB")) { changeNum = 1; }

        if (changeNum == -1) { return; }

        MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;

        int[] targetID = new int[2] { -1, -1 };
        int targetIndex = 0;
        for (int i = 0; i < MachingRoomData.playerMaxCount; i++)
        {
            //if it is myData, return
            if (i == Managers.instance.playerID) { continue; }

            //if it is empty, return
            AllGameData.AllData oscAllData = OSCManager.OSCinstance.GetAllData(i);
            if (oscAllData.rData.myID == MachingRoomData.bannerEmpty) { continue; }

            //if another team, return
            if (myRoomData.myTeamNum != oscAllData.rData.myTeamNum) { continue; }

            //if it player is dead, return
            if (!oscAllData.pData.mainPacketData.inGameData.alive) { continue; }

            targetID[targetIndex] = i;
            targetIndex++;
        }

        if (targetID[changeNum] != -1) { nowPlayer = Managers.instance.gameManager.GetPlayer(targetID[changeNum]); }
    }

    void InitialAngle()
    {
        initialTimer += Time.deltaTime;
        if (initialTimer >= 1) { initialTimer = 1; }

        Vector3 angle = Vector3.Lerp(Vector3.zero, ingameRotation, initialTimer);
        transform.rotation = Quaternion.Euler(angle);


        Vector3 playerPos = ownerPlayer.transform.position;
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
