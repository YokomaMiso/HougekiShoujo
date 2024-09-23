using OscCore;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////

    //���M�f�[�^�\����
    public AllGameData.AllData allData = new AllGameData.AllData();

    //���g�̃��[�J���C���Q�[���f�[�^
    public IngameData.PlayerNetData myNetIngameData = new IngameData.PlayerNetData();

    //���g�̃��[�J���}�b�`���O�V�[���f�[�^
    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();

    //�\���̕ϊ����������邽�ߐ���
    SendDataCreator netInstance = new SendDataCreator();

    //�ő�v���C���[�l��
    const int maxPlayer = 6;

    const string broadcastAddress = "255.255.255.255";

    int tempPort;

    const int startPort = 8000;

    [SerializeField]
    float sendPerSecond = 60.0f;

    string address = "/main";

    ///////// OSCcore���� ////////

    //���M��ۑ����X�g
    //�N���C�A���g�Ȃ�z�X�g���Ă�1�A�z�X�g�Ȃ�N���C�A���g5�l��������
    //�n���h�V�F�C�N���̓z�X�g����̉������m�F���邽�ߕK�����������
    List<OscClient> clientList = new List<OscClient>();


    OscServer tempServer;
    OscServer mainServer;

    //�������T�[�o���ǂ���
    bool isServer = false;
    bool isServerResponse = false;

    bool isFinishHandshake = false;

    const float waitHandshakeResponseTime = 2f;


    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    // �Ƃ肠�����V���O���g���ŉ^�p�i�����ؖ������肪���܂��Ă�����C���j
    public static OSCManager OSCinstance;

    [SerializeField]
    int myPort = 8000;

    int testNum = 0;

    //�Q�[�����ŕK�v�ȑ���M�f�[�^���X�g
    //�@�v�f�����v���C���[ID
    public List<AllGameData.AllData> playerDataList = new List<AllGameData.AllData>();

    string testS;

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //�����̃C���X�^���X
        OSCinstance = this;

        //�C���Q�[���p�f�[�^�̏��������
        allData.pData = new IngameData.PlayerNetData();
        allData.pData = default;

        //���[���f�[�^�̏�����
        allData.rData = initRoomData(allData.rData);

        //���|�J���p�����l��
        roomData = default;
        roomData = initRoomData(roomData);


        //�����̃f�[�^�������������|�[�g�ԍ�������
        for (int i = 0; i < maxPlayer; i++)
        {
            //�S�ď����l�ōő�l�����̃f�[�^���Z�b�g����
            allData.pData.PlayerID = i;
            allData.rData = initRoomData(allData.rData);
            allData.rData.isInData = false;
            playerDataList.Add(allData);
        }

        CreateTempNet();
    }

    // Update is called once per frame
    //�C���Q�[���f�[�^�������ɑ��M������܂����̂�Update�͊�{�s�g�p
    void Update()
    {
        //�f�o�b�N�p�ŔC�ӂ̃^�C�~���O�ő����悤�ɂ��Ă���
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        Debug.Log("PlayerID is " + Managers.instance.playerID);
        Debug.Log("IPAddress is " + GetLocalIPAddress());

        foreach (AllGameData.AllData data in playerDataList)
        {
            Debug.Log(data.rData.myID);
        }

        Debug.Log(testNum);

    }


    float sendDataTimer;
    float sendStartTimer;

    private void LateUpdate()
    {
        allData.rData = roomData;
        allData.pData = myNetIngameData;
        playerDataList[Managers.instance.playerID] = allData;

        float fixedDeltaTime = 1.0f / sendPerSecond;

        sendDataTimer += Time.deltaTime;

        if (sendStartTimer >= 2f)
        {
            if (sendDataTimer >= fixedDeltaTime)
            {
                Debug.Log("�C���Q�[���f�[�^���M");
                sendDataTimer += Time.deltaTime;

                //�n���h�V�F�C�N���������Ă���Ζ��t���[���C���Q�[���f�[�^�𑗐M����
                if (isFinishHandshake)
                {
                    //���[���f�[�^�͏��������s���Ă��Ȃ��ƎQ�ƃG���[���N���邽�߉��C���X�^���X���쐬�����
                    AllGameData.AllData _data = new AllGameData.AllData();
                    _data.rData = initRoomData(_data.rData);
                    Debug.Log("�C���Q�[���f�[�^���M");

                    _data = playerDataList[i];
                    //���M�p�f�[�^���X�g�ɂ��镪���M�����݂�
                    for (int i = 0; i < playerDataList.Count; i++)
                    {
                        //���[���f�[�^�͏��������s���Ă��Ȃ��ƎQ�ƃG���[���N���邽�߉��C���X�^���X���쐬�����
                        AllGameData.AllData _data = new AllGameData.AllData();
                        _data.rData = initRoomData(_data.rData);

                        SendValue(_data);
                        _data = playerDataList[i];

                        if (roomData.myID != -1)
                        {
                            SendValue(_data);
                        }
                    }
                }

                sendDataTimer = 0;
            }

        }
        else
        {
            sendStartTimer += Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        if (tempServer != null)
        {
            tempServer.Dispose();
        }

        if (mainServer != null)
        {
            mainServer.Dispose();
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

    private void CreateTempNet()
    {
        //�T�[�o�����邩�ǂ����������m�F���邽�߂̃N���C�A���g���쐬����
        OscClient _client = new OscClient(broadcastAddress, startPort);

        clientList.Add(_client);

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

        Debug.Log("�n���h�V�F�C�N�̑��M");


        if (roomData.isHandshaking == true)
        {
            Debug.Log("�n���h�V�F�C�N�̑��M");
            SendValue(_data);
        }

        return;
    }

    IEnumerator CheckForResponse()
    {
        yield return new WaitForSeconds(waitHandshakeResponseTime);

        Debug.Log("�R���[�`���쓮");

        if (roomData.isHandshaking)
        {
            //�n���h�V�F�C�N�m�F�p�p�P�b�g�j���O�ɃT�[�o���Ȃ��Ȃ�ƃo�O�邽�߂����ɋL�q
            CancelInvoke("SendFirstHandshake");
            Debug.Log("�T�[�o����̕ԓ�������܂���A�T�[�o�����ֈڍs");

            Managers.instance.playerID = 0;
            myNetIngameData.PlayerID = Managers.instance.playerID;
            roomData.myID = Managers.instance.playerID;

            allData.pData = myNetIngameData;
            allData.rData = roomData;

            playerDataList[0] = allData;

            clientList.Clear();

            tempServer.Dispose();

            mainServer = new OscServer(startPort);

            mainServer.TryAddMethod(address, ReadValue);

            isServer = true;

            isFinishHandshake = true;

            mainServer.TryAddMethod(address, ReadValue);
        }
        else
        {
            CancelInvoke("SendFirstHandshake");
            Debug.Log("�T�[�o�����݂��܂����A�N���C�A���g�����ֈڍs");

            tempServer.Dispose();

            mainServer = new OscServer(myNetIngameData.mainPacketData.comData.myPort);

            mainServer.TryAddMethod(address, ReadValue);


            isServer = false;

            isFinishHandshake = true;

            mainServer.TryAddMethod(address, ReadValue);
        }
    }

    private int GetRandomTempPort()
    {
        return UnityEngine.Random.Range(8006, 9000);
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


        //�T�[�o�Ȃ�܉�A�N���C�A���g�Ȃ���
        foreach (OscClient _client in clientList)
        {
            //�f�[�^�̑��M
            _client.Send(address, _sendBytes, _sendBytes.Length);
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
                testS = "�T�[�o�Ƃ��ăC���Q�[����M";
                playerDataList[_allData.pData.PlayerID] = _allData;
            }
            else if (_allData.rData.myID == -1 && _allData.rData.isHandshaking)
            {
                //testS = "�T�[�o�Ƃ��ăR�l�N�V������M";


                testNum++;

                for (int i = 0; i < playerDataList.Count; i++)
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

                        testNum = _allData.pData.mainPacketData.comData.myPort;

                        playerDataList[i] = _handshakeAllData;

                        OscClient _tempClient = new OscClient(broadcastAddress, testNum);
                        OscClient _tempClient = new OscClient(broadcastAddress, _allData.pData.mainPacketData.comData.myPort);

                        SendValueTarget(_handshakeAllData, _tempClient);

                        OscClient _client = new OscClient(broadcastAddress, startPort + i);
                        clientList.Add(_client);

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

                clientList.Clear();

                OscClient _client = new OscClient(_allData.pData.mainPacketData.comData.myIP, startPort);

                clientList.Add(_client);

            }
            else
            {
                testS = "�N���C�A���g�Ƃ��ăC���Q�[����M";

                playerDataList[_allData.pData.PlayerID] = _allData;
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
        _roomData.myBannerNum = -1;
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetSelectedCharacterID(i, 0); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetReadyPlayers(i, false); }
        _roomData.hostPlayer = 0;
        _roomData.gameStart = false;
        _roomData.isInData = true;
        _roomData.isHandshaking = true;
        _roomData.myID = -1;

        return _roomData;
    }

    /// <summary>
    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
    /// </summary>
    /// <remarks>���C���X���b�h����̂���Unity�p�̃��\�b�h������</remarks>
    private void MainThreadMethod()
    {

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
}