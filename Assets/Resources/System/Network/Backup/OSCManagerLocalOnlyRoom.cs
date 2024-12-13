using OscCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class OSCManagerLocalOnlyRoom : MonoBehaviour
{
    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////

    //���M�f�[�^�\����
    public AllGameData.AllData allData;

    //���g�̃��[�J���C���Q�[���f�[�^
    public IngameData.PlayerNetData myNetIngameData;

    //���g�̃��[�J���}�b�`���O�V�[���f�[�^
    public MachingRoomData.RoomData roomData;

    //�\���̕ϊ����������邽�ߐ���
    SendDataCreator netInstance = new SendDataCreator();

    //�ő�v���C���[�l��
    const int maxPlayer = 6;

    public const string broadcastAddress = "255.255.255.255";

    public const int startPort = 50000;

    int tempPort;

    [SerializeField]
    float sendPerSecond = 60.0f;

    string address = "/main";

    ///////// OSCcore���� ////////

    //���M��ۑ����X�g
    //�N���C�A���g�Ȃ�z�X�g���Ă�1�A�z�X�g�Ȃ�N���C�A���g5�l��������
    //�n���h�V�F�C�N���̓z�X�g����̉������m�F���邽�ߕK�����������
    List<OscClientData> clientList = new List<OscClientData>();
    List<float> connectTimeList = new List<float>();

    OscServer tempServer;
    OscServer mainServer;


    //�������T�[�o���ǂ���
    bool isServer = false;
    bool isServerResponse = false;

    bool isFinishHandshake = false;

    const float waitHandshakeResponseTime = 2f;

    const float timeoutSec = 5f;


    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    // �Ƃ肠�����V���O���g���ŉ^�p�i�����ؖ������肪���܂��Ă�����C���j
    //public static OSCManager OSCinstance;

    [SerializeField]
    int myPort = 8000;

    int testNum = 0;

    //�Q�[�����ŕK�v�ȑ���M�f�[�^���X�g
    //�@�v�f�����v���C���[ID
    public List<AllGameData.AllData> playerDataList = new List<AllGameData.AllData>();

    string testS;

    bool cutSend = false;

    bool isOutServer = false;

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //�����̃C���X�^���X
        //OSCinstance = this;

        //CreateTempNet();
    }

    // Update is called once per frame
    //�C���Q�[���f�[�^�������ɑ��M������܂����̂�Update�͊�{�s�g�p
    void Update()
    {
        if (isOutServer)
        {
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            Managers.instance.ChangeState(GAME_STATE.TITLE);
            Managers.instance.roomManager.Init();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            cutSend = true;
            //Debug.Log("���M�����J�b�g��");
        }
        else
        {
            cutSend = false;
        }


        //Debug.Log("PlayerID is " + Managers.instance.playerID);
        //Debug.Log("IPAddress is " + GetLocalIPAddress());
        //Debug.Log(testS);
        
        //Debug.Log(testNum);

        if (isFinishHandshake)
        {
            TimeoutChecker();
        }
    }

    float sendDataTimer;
    float sendStartTimer;

    private void LateUpdate()
    {
        if (isOutServer)
        {
            Managers.instance.ChangeScene(GAME_STATE.TITLE);
            Managers.instance.ChangeState(GAME_STATE.TITLE);
            Managers.instance.roomManager.Init();
        }

        if (playerDataList.Count == 0)
        {
            //Debug.Log("null�ł�");
            return;
        }

        allData.rData = roomData;
        allData.pData = myNetIngameData;
        playerDataList[Managers.instance.playerID] = allData;

        float fixedDeltaTime = 1.0f / sendPerSecond;

        sendDataTimer += Time.deltaTime;

        if (!isFinishHandshake) { return; }

        if (sendStartTimer <= 2f)
        {
            sendStartTimer += Time.deltaTime;
            return;
        }

        if (sendDataTimer >= fixedDeltaTime)
        {
            //�n���h�V�F�C�N���������Ă���Ζ��t���[���C���Q�[���f�[�^�𑗐M����
            if (isFinishHandshake)
            {
                if (isServer)
                {
                    //���M�p�f�[�^���X�g�ɂ��镪���M�����݂�
                    for (int i = 0; i < playerDataList.Count; i++)
                    {
                        //���[���f�[�^�͏��������s���Ă��Ȃ��ƎQ�ƃG���[���N���邽�߉��C���X�^���X���쐬�����
                        AllGameData.AllData _data = new AllGameData.AllData();
                        _data.rData = initRoomData(_data.rData);

                        _data = playerDataList[i];

                        if (_data.rData.myID != -1)
                        {
                            //Debug.Log("ID " + i + " �փf�[�^���M");
                            SendValue(_data);
                        }
                    }
                }
                else
                {
                    AllGameData.AllData _data = new AllGameData.AllData();
                    _data.rData = initRoomData(_data.rData);
                    _data = playerDataList[Managers.instance.playerID];

                    if (cutSend)
                    {
                        return;
                    }

                    if (_data.rData.myID != -1)
                    {
                        //Debug.Log("�C���Q�[���f�[�^���M(�N���C�A���g)");
                        SendValue(_data);
                    }
                }
            }

            sendDataTimer = 0;
        }

    }

    private void OnDisable()
    {
        DisPacket();
    }

    private void DisPacket()
    {
        if (tempServer != null)
        {
            tempServer.Dispose();
        }

        if (mainServer != null)
        {
            mainServer.Dispose();
        }

        if (clientList.Count > 0)
        {
            foreach (OscClientData _client in clientList)
            {
                _client.Release();
            }

            clientList.Clear();
        }
    }

    ////////////////////////////////////////////////////
    ///////////////�@�n���h�V�F�C�N�p�֐��@/////////////
    ////////////////////////////////////////////////////

    private string GetLocalIPAddress()
    {
        IPAddress ipv4Address = null;

        // �C�[�T�l�b�g��IPv4�A�h���X��T��
        ipv4Address = GetIPv4AddressByType(NetworkInterfaceType.Ethernet);
        if (ipv4Address != null)
        {
            return ipv4Address.ToString();
        }

        // Wi-Fi��IPv4�A�h���X��T��
        ipv4Address = GetIPv4AddressByType(NetworkInterfaceType.Wireless80211);
        if (ipv4Address != null)
        {
            return ipv4Address.ToString();
        }

        // �������݂��Ȃ��ꍇ�̓u���[�h�L���X�g�A�h���X��Ԃ�
        return "255.255.255.255";
    }

    private IPAddress GetIPv4AddressByType(NetworkInterfaceType type)
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == type && ni.OperationalStatus == OperationalStatus.Up)
            {
                var ipProperties = ni.GetIPProperties();
                var ipv4Address = ipProperties.UnicastAddresses
                    .Where(ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(ua => ua.Address)
                    .FirstOrDefault();

                if (ipv4Address != null)
                {
                    return ipv4Address;
                }
            }
        }
        return null;
    }

    public void CreateTempNet()
    {
        DisPacket();

        AllGameData.AllData allData = new AllGameData.AllData();
        myNetIngameData = new IngameData.PlayerNetData();
        roomData = new MachingRoomData.RoomData();

        playerDataList.Clear();
        clientList.Clear();
        connectTimeList.Clear();

        isServer = false;
        isServerResponse = false;
        isFinishHandshake = false;
        isOutServer = false;

        //�C���Q�[���p�f�[�^�̏��������
        allData.pData = new IngameData.PlayerNetData();
        allData.pData = default;
        allData.pData.mainPacketData.inGameData = initIngameData(allData.pData.mainPacketData.inGameData);

        //���[���f�[�^�̏�����
        allData.rData = initRoomData(allData.rData);

        //���|�J���p�����l��
        roomData = default;
        roomData = initRoomData(roomData);

        myNetIngameData = default;
        myNetIngameData.mainPacketData.inGameData = initIngameData(myNetIngameData.mainPacketData.inGameData);


        //�ő�l�����̃f�[�^���쐬
        for (int i = 0; i < maxPlayer; i++)
        {
            //�S�ď����l�ōő�l�����̃f�[�^���Z�b�g����
            allData.pData.PlayerID = i;
            allData.rData = initRoomData(allData.rData);
            allData.rData.isInData = false;
            playerDataList.Add(allData);

            //�l������M���Ԃ����
            connectTimeList.Add(0.0f);

            //�l�����̃N���C�A���g
            clientList.Add(new OscClientData());
        }

        //�T�[�o�����邩�ǂ����������m�F���邽�߂̃N���C�A���g���쐬����
        clientList[0].Assign(broadcastAddress, startPort);

        //�ꎞ�|�[�g�ԍ��ŃT�[�o����̉�����ҋ@
        tempPort = GetRandomTempPort();

        tempServer = new OscServer(tempPort);

        tempServer.TryAddMethod(address, ReadValue);

        //1�b���ƂɃn���h�V�F�C�N�̃f�[�^���M�����݂�
        InvokeRepeating("SendFirstHandshake", 0f, 1f);

        //��̃n���h�V�F�C�N���w�莞�Ԏ�������^�C���A�E�g������K�v�����邽�߂��̒��ŏ㏈�����~�߂�
        //�����ԓ����Ȃ���΂��̒��ŃT�[�o���쐬����
        StartCoroutine(CheckForResponse());

        return;
    }

    private void SendFirstHandshake()
    {
        AllGameData.AllData _data = new AllGameData.AllData();
        _data.rData = initRoomData(_data.rData);

        _data.pData.mainPacketData.comData.myIP = GetLocalIPAddress();
        _data.pData.mainPacketData.comData.myPort = tempPort;
        _data.rData.myID = -1;


        if (roomData.isHandshaking == true)
        {
            //Debug.Log("�n���h�V�F�C�N�̑��M");
            SendValue(_data);
        }

        return;
    }

    IEnumerator CheckForResponse()
    {
        yield return new WaitForSeconds(waitHandshakeResponseTime);

        //Debug.Log("�R���[�`���쓮");

        if (roomData.isHandshaking)
        {
            //�n���h�V�F�C�N�m�F�p�p�P�b�g�j���O�ɃT�[�o���Ȃ��Ȃ�ƃo�O�邽�߂����ɋL�q
            CancelInvoke("SendFirstHandshake");
            //Debug.Log("�T�[�o����̕ԓ�������܂���A�T�[�o�����ֈڍs");

            Managers.instance.playerID = 0;
            myNetIngameData.PlayerID = Managers.instance.playerID;
            roomData.myID = Managers.instance.playerID;

            allData.pData = myNetIngameData;
            allData.rData = roomData;

            playerDataList[0] = allData;

            clientList[0].Release();

            tempServer.Dispose();

            mainServer = new OscServer(startPort);

            isServer = true;

            isFinishHandshake = true;
            roomData.playerName = Managers.instance.optionData.playerName;

            mainServer.TryAddMethod(address, ReadValue);
        }
        else
        {
            CancelInvoke("SendFirstHandshake");
            //Debug.Log("�T�[�o�����݂��܂����A�N���C�A���g�����ֈڍs");

            tempServer.Dispose();

            mainServer = new OscServer(myNetIngameData.mainPacketData.comData.myPort);

            isServer = false;

            isFinishHandshake = true;
            roomData.playerName = Managers.instance.optionData.playerName;

            mainServer.TryAddMethod(address, ReadValue);
        }
    }

    private int GetRandomTempPort()
    {
        return UnityEngine.Random.Range(50006, 51000);
    }

    ////////////////////////////////////////////////////
    /////////////////�@�ڑ������p�֐��@///////////////
    ////////////////////////////////////////////////////

    /// <summary>
    /// �f�[�^���M�֐�
    /// </summary>
    /// <typeparam name="T">�\���̋y�ђl�^�̃f�[�^</typeparam>
    /// <param name="_struct">���ۂɑ��M�������\���̃f�[�^</param>
    private void SendValue<T>(T _struct) where T : struct
    {
        byte[] _sendBytes = new byte[0];

        //���M�f�[�^�̃o�C�g�z��
        _sendBytes = netInstance.StructToByte(_struct);

        int i = 0;

        //�T�[�o�Ȃ�܉�A�N���C�A���g�Ȃ���
        foreach (OscClientData _clientData in clientList)
        {
            if (_clientData.IsUsing())
            {
                //Debug.Log(i + " �փf�[�^���M");

                //�f�[�^�̑��M
                _clientData.client.Send(address, _sendBytes, _sendBytes.Length);
            }
            i++;
        }
    }

    private void SendValueTarget<T>(T _struct, OscClient _client) where T : struct
    {
        byte[] _sendBytes = new byte[0];

        //���M�f�[�^�̃o�C�g�z��
        _sendBytes = netInstance.StructToByte(_struct);


        //�f�[�^�̑��M
        _client.Send(address, _sendBytes, _sendBytes.Length);
    }

    /// <summary>
    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
    /// </summary>
    /// <param name="values">��M�����f�[�^</param>
    /// <remarks>�T�u�X���b�h����̂���Unity�p�̃��\�b�h�͓��삵�܂���I�I�I</remarks>
    private void ReadValue(OscMessageValues values)
    {
        byte[] _receiveBytes = new byte[0];

        //��M�f�[�^�̃R�s�[
        values.ReadBlobElement(0, ref _receiveBytes);

        //�f�[�^�̍\���̉�
        AllGameData.AllData _allData = new AllGameData.AllData();
        _allData.rData = initRoomData(_allData.rData);
        _allData = netInstance.ByteToStruct<AllGameData.AllData>(_receiveBytes);


        if (isServer)
        {
            //testS = "�T�[�o";

            //��M�����v���C���[�f�[�^���Q�[�����ɑ��݂���ꍇ�f�[�^���X�g�ɃZ�b�g����
            if (!_allData.rData.isHandshaking)
            {
                if (_allData.rData.myID == -1)
                {
                    _allData.rData = initRoomData(_allData.rData);
                    _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

                    playerDataList[_allData.pData.PlayerID] = _allData;

                    connectTimeList[_allData.pData.PlayerID] = 0.0f;

                    clientList[_allData.pData.PlayerID].Release();

                    return;
                }

                testS = "�T�[�o�Ƃ��ăC���Q�[����M";
                playerDataList[_allData.pData.PlayerID] = _allData;

                //��M�J�E���g�����Z�b�g����
                connectTimeList[_allData.pData.PlayerID] = 0f;
            }
            else if (_allData.rData.myID == -1 && _allData.rData.isHandshaking)
            {
                testS = "�T�[�o�Ƃ��ăR�l�N�V������M";


                testNum++;

                for (int i = 1; i < playerDataList.Count; i++)
                {
                    if (playerDataList[i].rData.myID == -1)
                    {
                        AllGameData.AllData _handshakeAllData = new AllGameData.AllData();
                        _handshakeAllData.rData = initRoomData(_handshakeAllData.rData);

                        _handshakeAllData.pData.mainPacketData.comData.myIP = GetLocalIPAddress();
                        _handshakeAllData.pData.mainPacketData.comData.myPort = startPort + i;
                        _handshakeAllData.pData.PlayerID = i;
                        _handshakeAllData.rData.myID = i;
                        _handshakeAllData.rData.isHandshaking = false;

                        playerDataList[i] = _handshakeAllData;

                        OscClient _tempClient = new OscClient(_allData.pData.mainPacketData.comData.myIP, _allData.pData.mainPacketData.comData.myPort);

                        SendValueTarget(_handshakeAllData, _tempClient);

                        clientList[i].Assign(_allData.pData.mainPacketData.comData.myIP, startPort + i);

                        break;
                    }
                }
            }
        }
        else
        {
            if (!isFinishHandshake)
            {
                //testS = "�N���C�A���g�Ƃ��ăn���h�V�F�C�N��M";
                Managers.instance.playerID = _allData.rData.myID;
                roomData.myID = _allData.rData.myID;

                myNetIngameData = _allData.pData;
                roomData = _allData.rData;

                allData.pData = myNetIngameData;
                allData.rData = roomData;

                playerDataList[Managers.instance.playerID] = allData;

                clientList[0].Release();

                clientList[0].Assign(_allData.pData.mainPacketData.comData.myIP, startPort);

            }
            else
            {
                //testS = "�N���C�A���g�Ƃ��ăC���Q�[����M";

                if (_allData.rData.myID == -1 && _allData.pData.PlayerID == 0)
                {
                    testS = "�T�[�o�������܂���";

                    _allData.rData = initRoomData(_allData.rData);
                    _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

                    playerDataList[0] = _allData;

                    connectTimeList[0] = 0.0f;

                    clientList[0].Release();

                    InitNetworkData();

                    DisPacket();

                    isOutServer = true;

                    return;
                }

                playerDataList[_allData.pData.PlayerID] = _allData;

                //��M�J�E���g�����Z�b�g����
                connectTimeList[0] = 0f;
            }
        }
    }

    /// <summary>
    /// ���[���f�[�^�̏���������
    /// </summary>
    /// <param name="_roomData">���������������[���f�[�^</param>
    /// <returns>���[���f�[�^�̏������l</returns>
    MachingRoomData.RoomData initRoomData(MachingRoomData.RoomData _roomData)
    {
        _roomData.myID = -1;
        _roomData.myTeamNum = -1;
        _roomData.selectedCharacterID = 0;
        _roomData.ready = false;
        _roomData.gameStart = false;
        _roomData.isInData = true;
        _roomData.isHandshaking = true;

        return _roomData;
    }

    /// <summary>
    /// �C���Q�[���f�[�^�̏���������
    /// </summary>
    /// <param name="_ingameData">�������������C���Q�[���f�[�^</param>
    /// <returns>�C���Q�[���f�[�^�̏������l</returns>
    IngameData.GameData initIngameData(IngameData.GameData _ingameData)
    {
        _ingameData.play = false;
        _ingameData.start = false;
        _ingameData.end = false;
        _ingameData.startTimer = 0;
        _ingameData.endTimer = 0;

        _ingameData.roundCount = 1;
        _ingameData.roundTimer = 60;
        _ingameData.alivePlayerCountTeamA = 0;
        _ingameData.alivePlayerCountTeamB = 0;
        _ingameData.winCountTeamA = 0;
        _ingameData.winCountTeamB = 0;
        _ingameData.winner = -1;

        return _ingameData;
    }

    /// <summary>
    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
    /// </summary>
    /// <remarks>���C���X���b�h����̂���Unity�p�̃��\�b�h������</remarks>
    private void MainThreadMethod()
    {

    }

    private void TimeoutChecker()
    {
        if (Managers.instance.state >= GAME_STATE.ROOM && Managers.instance.state <= GAME_STATE.IN_GAME)
        {
            if (isServer)
            {
                for (int i = 1; i < maxPlayer; i++)
                {
                    //�����v���C���[�����݂��A�^�C���A�E�g���ԂɒB���Ă���΂��̃v���C���[������������
                    if (playerDataList[i].rData.myID != -1 && connectTimeList[i] > timeoutSec)
                    {
                        AllGameData.AllData _allData = new AllGameData.AllData();

                        _allData.rData = initRoomData(_allData.rData);
                        _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

                        playerDataList[i] = _allData;

                        connectTimeList[i] = 0.0f;

                        clientList[i].Release();
                    }
                    else if (playerDataList[i].rData.myID != -1)
                    {
                        connectTimeList[i] += Time.deltaTime;
                    }
                }
            }
            else
            {
                //�^�C���A�E�g���ԂɒB���Ă���Ύ��g�������������ă^�C�g���܂Ŗ߂�
                if (connectTimeList[0] > timeoutSec)
                {
                    InitNetworkData();

                    //Debug.Log("�ڑ����^�C���A�E�g���܂���");

                    DisPacket();

                    Managers.instance.ChangeScene(GAME_STATE.TITLE);
                    Managers.instance.ChangeState(GAME_STATE.TITLE);
                }
                else
                {
                    connectTimeList[0] += Time.deltaTime;
                }
            }
        }

        return;
    }

    /// <summary>
    /// �N���C�A���g�̃l�b�g���[�N����������
    /// </summary>
    private void InitNetworkData()
    {

        if (isServer)
        {
            AllGameData.AllData _allData = new AllGameData.AllData();

            _allData.rData = initRoomData(_allData.rData);
            _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

            playerDataList[0] = _allData;

            myNetIngameData.mainPacketData.inGameData = initIngameData(myNetIngameData.mainPacketData.inGameData);
            roomData = initRoomData(roomData);

            sendStartTimer = 0f;

            isFinishHandshake = false;

            foreach (OscClientData _client in clientList)
            {
                _client.Release();
            }

            mainServer.Dispose();
        }
        else
        {
            AllGameData.AllData _allData = new AllGameData.AllData();

            _allData.rData = initRoomData(_allData.rData);
            _allData.pData.mainPacketData.inGameData = initIngameData(_allData.pData.mainPacketData.inGameData);

            playerDataList[0] = _allData;

            myNetIngameData.mainPacketData.inGameData = initIngameData(myNetIngameData.mainPacketData.inGameData);
            roomData = initRoomData(roomData);

            sendStartTimer = 0f;

            isFinishHandshake = false;

            mainServer.Dispose();
        }

        return;
    }

    /// <summary>
    /// ���[���f�[�^�̃Q�b�^�[
    /// </summary>
    /// <param name="_num">�擾�������v���C���[ID</param>
    /// <returns>�w�肵���v���C���[ID�̃��[���f�[�^</returns>
    public MachingRoomData.RoomData GetRoomData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata.rData;
    }

    /// <summary>
    /// �C���Q�[���f�[�^�̃Q�b�^�[
    /// </summary>
    /// <param name="_num">�擾�������v���C���[ID</param>
    /// <returns>�w�肵���v���C���[ID�̃C���Q�[���f�[�^</returns>
    public IngameData.PlayerNetData GetIngameData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata.pData;
    }

    /// <summary>
    /// �S�ʐM�f�[�^�̃Q�b�^�[
    /// </summary>
    /// <param name="_num">�擾�������v���C���[ID</param>
    /// <returns>�w�肵���v���C���[ID�̑S�f�[�^</returns>
    public AllGameData.AllData GetAllData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata;
    }

    /// <summary>
    /// �w�肵���v���C���[ID�̎擾
    /// </summary>
    /// <param name="_num">�擾�������v���C���[ID</param>
    /// <returns>�v���C���[ID</returns>
    public int GetPlayerID(int _num)
    {
        return playerDataList[_num].pData.PlayerID;
    }

    public bool GetIsFinishedHandshake()
    {
        return isFinishHandshake;
    }

    /// <summary>
    /// ���[�����甲������
    /// </summary>
    public void ExitToRoom()
    {
        if (isServer)
        {
            AllGameData.AllData _data = new AllGameData.AllData();
            _data.rData = initRoomData(_data.rData);
            _data = playerDataList[Managers.instance.playerID];

            _data.rData.myID = -1;

            for (int i = 0; i < playerDataList.Count; i++)
            {
                if (playerDataList[i].rData.myID != -1)
                {
                    SendValue(_data);
                }
            }

            InitNetworkData();
        }
        else
        {
            AllGameData.AllData _data = new AllGameData.AllData();
            _data.rData = initRoomData(_data.rData);
            _data = playerDataList[Managers.instance.playerID];

            _data.rData.myID = -1;

            SendValue(_data);

            InitNetworkData();
        }

        DisPacket();

        return;
    }
}