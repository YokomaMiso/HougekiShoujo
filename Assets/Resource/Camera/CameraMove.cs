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
    Player[] teamPlayers;
    int nowPlayer = -1;

    static readonly Vector3 ingameRotation = new Vector3(45, 0, 0);
    float initialTimer;

    float shakeValue;

    float targetFar = 5;
    const float defaultFar = 5;
    float aimTimer;

    void Start()
    {
        audioListenerChild = transform.GetChild(0).gameObject;
        TargetSearch();

        //����A�L�����o�X����������Ă��Ȃ��Ȃ�
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
            if (ownerPlayer.GetAlive()) { Move(ownerPlayer.transform.position); }
            else
            {
                TargetChange();
                if (nowPlayer >= 0 && teamPlayers[nowPlayer] && teamPlayers[nowPlayer].GetAlive())
                {
                    Move(teamPlayers[nowPlayer].transform.position);
                }
            }
        }
    }

    void TargetSearch()
    {
        MachingRoomData.RoomData myRoomData = OSCManager.OSCinstance.roomData;
        MachingRoomData.RoomData hostRoomData = OSCManager.OSCinstance.GetRoomData(0);

        if (myRoomData.myTeamNum == (int)TEAM_NUM.A) { teamPlayers = new Player[hostRoomData.teamACount - 1]; }
        else { teamPlayers = new Player[hostRoomData.teamBCount - 1]; }

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

            teamPlayers[targetIndex] = Managers.instance.gameManager.GetPlayer(i);
            targetIndex++;
        }
    }

    void TargetChange()
    {
        //���������P�l�Ȃ瑁�����^�[��
        if (teamPlayers == null) { return; }

        //�J�����؂�ւ����o���邩�ǂ���
        int canChangeCount = 0;
        for (int i = 0; i < teamPlayers.Length; i++)
        {
            //nowPlayer�Ŏ����Ă���Ȃ�J�E���g�͐i�߂Ȃ�
            if (i == nowPlayer)
            {
                //��������ł�Ȃ�nowPlayer�����ɖ߂�
                if (!teamPlayers[i].GetAlive()) { nowPlayer = -1; }
                continue;
            }
            //�������Ă��閡��������
            if (teamPlayers[i].GetAlive()) { canChangeCount++; }
        }

        //�A�i�E���XUI��\��
        spectateAnnounceInstance.GetComponent<SpectateCameraAnnounce>().Display(canChangeCount > 0);

        //canChangePlayer���P�l�ł�����΁A�J������؂�ւ��邱�Ƃ��o����
        if (canChangeCount > 0)
        {
            //LB�������ꂽ��
            if (Input.GetButtonDown("LB"))
            {
                for (int i = 0; i < teamPlayers.Length; i++)
                {
                    //nowPlayer�Ŏ����Ă���Ȃ�X���[
                    if (i == nowPlayer) { continue; }
                    //nowPlayer�Ɍ��݂̒l�����Ĕ�����
                    nowPlayer = i;
                    break;
                }
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
