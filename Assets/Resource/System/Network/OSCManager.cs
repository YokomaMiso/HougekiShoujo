using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;
using UnityEngine;
using OscCore;
using System.Runtime.InteropServices;

//public class OSCManager : MonoBehaviour
//{
//    //////////////////////////////
//    //////// �{�Ԏg�p�ϐ� ////////
//    //////////////////////////////

//    //���g�̃C���Q�[���f�[�^
//    public IngameData.PlayerNetData myNetIngameData = new IngameData.PlayerNetData();

//    //�����Ă�������̃C���Q�[���f�[�^
//    public IngameData.PlayerNetData receivedIngameData = new IngameData.PlayerNetData();

//    //�n���h�V�F�C�N���Ƀu���[�h�L���X�g�ő��M����f�[�^
//    public HandshakeData.SendUserData firstData = new HandshakeData.SendUserData();

//    //�T�[�o���ɂȂ������̃n���h�V�F�C�N�f�[�^�̎󂯎M
//    public HandshakeData.SendUserData receivedFirstData = new HandshakeData.SendUserData();

//    //�ڑ��m�F���̃T�[�o����̕ԓ��f�[�^
//    public ResponseServerData.ResData resServerData = new ResponseServerData.ResData();

//    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();
//    public MachingRoomData.RoomData receiveRoomData = new MachingRoomData.RoomData();

//    public SendDataCreator netInstance = new SendDataCreator();

//    //�������T�[�o���ǂ���
//    bool isServer = false;
//    bool isServerResponse = false;

//    const float waitHandshakeResponseTime = 4f;

//    ///////// OSCcore���� ////////

//    OscClient Client;
//    OscServer Server;
//    OscServer mainServer;
//    OscServer ingameServer;

//    bool isReceiveIngame = false;

//    //�g�p����|�[�g�ԍ��̎n��
//    const int startPort = 8000;


//    enum NetworkState
//    {
//        HandShaking,
//        Maching,
//        InGame
//    }

//    NetworkState nowNetState = NetworkState.HandShaking;

//    ////////////////////////////////
//    //////// �f�o�b�N�p�ϐ� ////////
//    ////////////////////////////////

//    // �Ƃ肠�����V���O���g���ŉ^�p�i�����ؖ������肪���܂��Ă�����C���j
//    public static OSCManager OSCinstance;

//    [SerializeField]
//    bool isManual = true;

//    [SerializeField]
//    int port = 8000;

//    //[SerializeField]
//    //int otherPort = 8001;

//    int testNum = 0;

//    byte[] handshakeBytes = new byte[0];
//    byte[] roomDataBytes = new byte[0];
//    int receiveLong = 0;

//    bool handshaked = false;

//    //////////////////////
//    //////// �֐� ////////
//    //////////////////////

//    // Start is called before the first frame update
//    void Start()
//    {
//        /////////////////////////////
//        //////////�����ʐM����///////
//        /////////////////////////////

//        roomData = initRoomData(roomData);
//        receiveRoomData = initRoomData(receiveRoomData);

//        handshakeBytes = netInstance.StructToByte(firstData);
//        roomDataBytes = netInstance.StructToByte(roomData);

//        //�T�[�o�������ꍇ�̃f�[�^�Z�b�g
//        myNetIngameData.mainPacketData.comData.targetIP = "255.255.255.255";
//        myNetIngameData.mainPacketData.comData.receiveAddress = "/example";

//        //��U���g��IP���u���[�h�L���X�g�A�h���X��
//        firstData.IP = "255.255.255.255";

//        //�ꎞ�I�ɃT�[�o����̕ԓ����󂯕t����|�[�g�ԍ��擾
//        firstData.tempPort = GetRandomTempPort();

//        if (Client == null)
//        {
//            //8000�ԁi�T�[�o���̃|�[�g�ԍ��j�ֈ��Ă��N���C�A���g���쐬
//            Client = new OscClient(myNetIngameData.mainPacketData.comData.targetIP, startPort);

//        }

//        if (Server == null)
//        {
//            //�ꎞ�|�[�g�ԍ��ō쐬
//            Server = new OscServer(firstData.tempPort);
//        }

//        Server.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);

//        Debug.Log("�n���h�V�F�C�N�J�n");

//        //1�b���ƂɃn���h�V�F�C�N�̃f�[�^���M�����݂�
//        InvokeRepeating("SendFirstHandshake", 0f, 1f);

//        //��̃n���h�V�F�C�N���w�莞�Ԏ�������^�C���A�E�g������K�v�����邽�߂��̒��ŏ㏈�����~�߂�
//        //�����ԓ����Ȃ���΂��̒��ŃT�[�o���쐬����
//        StartCoroutine(CheckForResponse());

//        /////////////////////////////
//        ////////OSCcore������////////
//        /////////////////////////////

//        OSCinstance = this;

//        //myNetIngameData = default;
//        //myNetIngameData.mainPacketData = default;
//        //myNetIngameData.byteData = null;
//        //
//        //receivedIngameData = default;
//        //receivedIngameData.byteData = new byte[0];
//        //
//        //if(isManual)
//        //{
//        //    myNetIngameData.mainPacketData.comData.myPort = port;
//        //}
//        //else
//        //{
//        //    //myNetIngameData.mainPacketData.comData.myPort = PortAssignor();
//        //}
//        //
//        //
//        //Managers.instance.playerID = myNetIngameData.mainPacketData.comData.myPort - 8000;
//        //
//        //Debug.Log("���̃|�[�g�ԍ��� : " + myNetIngameData.mainPacketData.comData.myPort);
//        //Debug.Log("���̃v���C���[�ԍ��� : " + Managers.instance.playerID);
//        //
//        ////myNetData.mainPacketData.comData.myIP = "255.255.255.255";
//        ////myNetData.mainPacketData.comData.targetIP = "255.255.255.255";
//        ////myNetData.mainPacketData.comData.targetPort = otherPort;
//        //
//        //myNetIngameData.mainPacketData.comData.sendAddress = "/example";
//        //
//        //if(myNetIngameData.mainPacketData.comData.myPort == startPort)
//        //{
//        //    myNetIngameData.mainPacketData.comData.targetPort = 8001;
//        //}
//        //else
//        //{
//        //    myNetIngameData.mainPacketData.comData.targetPort = 8000;
//        //}
//        //
//        //
//        //if (mainClient == null)
//        //{
//        //    mainClient = new OscClient(myNetIngameData.mainPacketData.comData.targetIP, myNetIngameData.mainPacketData.comData.targetPort);
//        //
//        //}
//        //
//        //if (mainServer == null)
//        //{
//        //    //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
//        //    mainServer = new OscServer(myNetIngameData.mainPacketData.comData.myPort);
//        //}
//        //
//        //mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReadIngameValue);


//    }

//    // Update is called once per frame
//    void Update()
//    {
//        ////�f�o�b�N�p�ŔC�ӂ̃^�C�~���O�ő����悤�ɂ��Ă���
//        //if(Input.GetKeyDown(KeyCode.Space))
//        //{
//        //    SendValue();
//        //}

//        //if(Input.GetKeyDown(KeyCode.M))
//        //{
//        //    myNetData.mainPacketData.inGameData.num += 1;
//        //    myNetData.mainPacketData.inGameData.test = "HelloWorld!";
//        //    myNetData.mainPacketData.inGameData.vc3 += new Vector3(1.0f, 0.5f, 0.1f);



//        //}

//        //if(Input.GetKeyDown(KeyCode.P))
//        //{
//        //    Debug.Log(receivedData.mainPacketData.inGameData.num);
//        //    Debug.Log(receivedData.mainPacketData.inGameData.test);
//        //    Debug.Log(receivedData.mainPacketData.inGameData.vc3);
//        //}

//        Debug.Log(testNum);


//        //Debug.Log("�n���h�V�F�C�N�p�f�[�^" + handshakeBytes.Length);
//        //Debug.Log("�������" + roomDataBytes.Length);
//        //Debug.Log("��M�o�C�g�T�C�Y" + receiveLong);

//        if (Input.GetKeyDown(KeyCode.P))
//        {
//            Debug.Log("�z�X�g�v���C���[ID : " + roomData.hostPlayer);
//        }

//        if (Input.GetKeyDown(KeyCode.M))
//        {
//            Debug.Log(Client.Destination);
//            SendMachingData();
//        }

//        if (Managers.instance.gameManager.play && isReceiveIngame == false)
//        {
//            isReceiveIngame = true;
//            if (Managers.instance.playerID == 0)
//            {
//                mainServer.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingServer);
//                mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReadIngameValue);
//            }
//            else
//            {
//                mainServer.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingClient);
//                mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReadIngameValue);
//            }
//        }
//    }

//    private void LateUpdate()
//    {
//        if (Managers.instance.state == GAME_STATE.IN_GAME)
//        {
//            SendIngameValue();
//        }
//    }

//    private void OnEnable()
//    {




//    }

//    private void OnDisable()
//    {

//        if (Server != null)
//        {
//            Server.Dispose();
//        }

//    }

//    MachingRoomData.RoomData initRoomData(MachingRoomData.RoomData _roomData)
//    {
//        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetBannerNum(i, MachingRoomData.bannerEmpty); }
//        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetSelectedCharacterID(i, 0); }
//        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetReadyPlayers(i, false); }
//        _roomData.hostPlayer = 0;
//        _roomData.gameStart = false;


//        return _roomData;
//    }

//    //////////////////////////
//    //////�C���Q�[���֐�//////
//    //////////////////////////

//    /// <summary>
//    /// �f�[�^���M
//    /// </summary>
//    private void SendIngameValue()
//    {
//        //���M�f�[�^�̃o�C�g�z��
//        myNetIngameData.byteData = netInstance.StructToByte(myNetIngameData.mainPacketData);

//        //�f�[�^�̑��M
//        Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, myNetIngameData.byteData, myNetIngameData.byteData.Length);
//    }

//    /// <summary>
//    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
//    /// </summary>
//    /// <param name="values">��M�����f�[�^</param>
//    /// <remarks>�T�u�X���b�h����̂���Unity�p�̃��\�b�h�͓��삵�܂���I�I�I</remarks>
//    private void ReadIngameValue(OscMessageValues values)
//    {
//        //��M�f�[�^�̃R�s�[
//        values.ReadBlobElement(0, ref receivedIngameData.byteData);

//        //�f�[�^�̍\���̉�
//        receivedIngameData.mainPacketData = netInstance.ByteToStruct<IngameData.PacketDataForPerFrame>(receivedIngameData.byteData);

//        return;
//    }

//    //////////////////////////////
//    ///////////�����ʐM�֐�///////
//    //////////////////////////////

//    int GetRandomTempPort()
//    {
//        return Random.Range(8006, 9000);
//    }

//    //�n���h�V�F�C�N���M�֐�
//    private void SendFirstHandshake()
//    {
//        byte[] bytes = netInstance.StructToByte(firstData);

//        Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, bytes, bytes.Length);

//        Debug.Log("1way���M");

//        return;
//    }

//    /// <summary>
//    /// �u���[�h�L���X�g���M���̃T�[�o����̕ԓ��p
//    /// </summary>
//    /// <param name="value"></param>
//    private void ReceiveHandshakeForClient(OscMessageValues value)
//    {
//        byte[] _bytes = new byte[0];

//        value.ReadBlobElement(0, ref _bytes);

//        resServerData = netInstance.ByteToStruct<ResponseServerData.ResData>(_bytes);

//        receiveRoomData = resServerData.serverRoomData;

//        //���̃��\�b�h���Ăяo����Ă��鎞�_�ŃT�[�o����̕ԓ������Ă��邽��true�ɂ���
//        isServerResponse = true;

//        return;
//    }

//    /// <summary>
//    /// �n���h�V�F�C�N���M���̔����m�F�p
//    /// </summary>
//    /// <returns></returns>
//    IEnumerator CheckForResponse()
//    {
//        yield return new WaitForSeconds(waitHandshakeResponseTime);

//        Debug.Log("�R���[�`���쓮");

//        if (!isServerResponse)
//        {
//            //�n���h�V�F�C�N�m�F�p�p�P�b�g�j���O�ɃT�[�o���Ȃ��Ȃ�ƃo�O�邽�߂����ɋL�q
//            CancelInvoke("SendFirstHandshake");
//            Debug.Log("�T�[�o����̕ԓ�������܂���A�T�[�o�����ֈڍs");

//            Managers.instance.playerID = 0;
//            Client = new OscClient("255.255.255.255", 8001);
//            StartServer();
//        }
//        else
//        {
//            CancelInvoke("SendFirstHandshake");
//            Debug.Log("�T�[�o�����݂��܂����A�N���C�A���g�����ֈڍs");

//            Managers.instance.playerID = 1;
//            Client = new OscClient("255.255.255.255", 8000);
//            StartClient();
//        }
//    }

//    //�T�[�o�����݂������ߎ�M�f�[�^���܂Ƃ߂�
//    private void StartClient()
//    {
//        Server.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);
//        Server.Dispose();

//        if (mainServer == null)
//        {
//            mainServer = new OscServer(8001);
//        }

//        mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingClient);

//        return;
//    }

//    private void ReceiveMachingClient(OscMessageValues value)
//    {
//        byte[] _recieveByte = new byte[0];

//        value.ReadBlobElement(0, ref _recieveByte);

//        receiveRoomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_recieveByte);

//        return;
//    }

//    //�T�[�o�s�ݎ��Ɏ������T�[�o�ɂȂ鏈��
//    void StartServer()
//    {
//        isServer = true;
//        isServerResponse = true;

//        Server.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);
//        Server.Dispose();

//        if (mainServer == null)
//        {
//            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
//            mainServer = new OscServer(startPort);
//        }

//        mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingServer);

//        return;
//    }

//    /// <summary>
//    /// �T�[�o���ɂȂ����ۂ̃N���C�A���g�n���h�V�F�C�N��M�p
//    /// </summary>
//    /// <param name="value"></param>
//    private void ReceiveMachingServer(OscMessageValues value)
//    {
//        byte[] _receiveBytes = new byte[0];
//        byte[] _sendBytes = new byte[0];

//        resServerData.toClientPort = 8001;
//        resServerData.serverIP = "255.255.255.255";

//        value.ReadBlobElement(0, ref _receiveBytes);

//        receiveLong = _receiveBytes.Length;

//        //���M����Ă����o�C�g�z��T�C�Y���n���h�V�F�C�N�p�Ɠ����Ȃ�n���h�V�F�C�N�f�[�^�Ƃ��Ď�舵��
//        if (handshaked == false) //_receiveBytes.Length == 1048
//        {
//            handshaked = true;

//            testNum = 1;
//            receivedFirstData = netInstance.ByteToStruct<HandshakeData.SendUserData>(_receiveBytes);

//            Client = new OscClient(receivedFirstData.IP, receivedFirstData.tempPort);

//            resServerData.serverRoomData = roomData;

//            _sendBytes = netInstance.StructToByte(resServerData);

//            Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);

//            //3way���ł����炻����ֈڍs
//            Client = new OscClient("255.255.255.255", 8001);
//        }
//        else //if(_receiveBytes.Length == 1112)
//        {
//            testNum = 2;
//            receiveRoomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_receiveBytes);

//            //Client = new OscClient("255.255.255.255", 8001);

//            _sendBytes = netInstance.StructToByte(roomData);

//            Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
//        }

//        return;
//    }

//    void SendMachingData()
//    {
//        byte[] _sendBytes = new byte[0];

//        _sendBytes = netInstance.StructToByte(roomData);

//        if (isServer)
//        {
//            Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
//        }
//        else
//        {
//            Client.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
//        }


//        return;
//    }

//    public void SendRoomData()
//    {
//        SendMachingData();

//        return;
//    }
//}

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////

    //���g�̃C���Q�[���f�[�^
    public IngameData.PlayerNetData myNetIngameData = new IngameData.PlayerNetData();

    //�����Ă�������̃C���Q�[���f�[�^
    public IngameData.PlayerNetData receivedIngameData = new IngameData.PlayerNetData();

    //���g�̃l�b�g���[�N�f�[�^
    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();

    //�����Ă�������̃f�[�^
    public MachingRoomData.RoomData receiveRoomData = new MachingRoomData.RoomData();

    SendDataCreator netInstance = new SendDataCreator();

    string broadcastAddress = "255.255.255.255";

    string address = "/example";

    ///////// OSCcore���� ////////

    OscClient client;
    OscServer server;


    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    // �Ƃ肠�����V���O���g���ŉ^�p�i�����ؖ������肪���܂��Ă�����C���j
    public static OSCManager OSCinstance;

    [SerializeField]
    int myPort = 8000;

    [SerializeField]
    int otherPort = 8001;

    int testNum = 0;

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        OSCinstance = this;

        roomData = default;
        receiveRoomData = default;

        roomData = initRoomData(roomData);
        receiveRoomData = initRoomData(receiveRoomData);

        if (client == null)
        {
            client = new OscClient(broadcastAddress, otherPort);

        }

        if (server == null)
        {
            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
            server = new OscServer(myPort);
        }

        if(myPort == 8000)
        {
            Managers.instance.playerID = 0;
        }
        else
        {
            Managers.instance.playerID = 1;
        }

        server.TryAddMethodPair(broadcastAddress, ReadValue, MainThreadMethod);
    }

    // Update is called once per frame
    void Update()
    {
        //�f�o�b�N�p�ŔC�ӂ̃^�C�~���O�ő����悤�ɂ��Ă���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendValue();
        }

        Debug.Log(testNum);

    }

    private void LateUpdate()
    {
        //SendValue();
    }

    private void OnDisable()
    {
        if (server != null)
        {
            server.Dispose();
        }

    }

    /// <summary>
    /// �f�[�^���M
    /// </summary>
    private void SendValue()
    {
        byte[] _sendBytes = new byte[0];

        //���M�f�[�^�̃o�C�g�z��
        _sendBytes = netInstance.StructToByte(roomData);

        //�f�[�^�̑��M
        client.Send(address, _sendBytes, _sendBytes.Length);
    }

    /// <summary>
    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
    /// </summary>
    /// <param name="values">��M�����f�[�^</param>
    /// <remarks>�T�u�X���b�h����̂���Unity�p�̃��\�b�h�͓��삵�܂���I�I�I</remarks>
    private void ReadValue(OscMessageValues values)
    {
        byte[] _receiveBytes = new byte[0];
        testNum++;

        //��M�f�[�^�̃R�s�[
        values.ReadBlobElement(0, ref _receiveBytes);

        //�f�[�^�̍\���̉�
        receiveRoomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_receiveBytes);
    }

    MachingRoomData.RoomData initRoomData(MachingRoomData.RoomData _roomData)
    {
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetBannerNum(i, MachingRoomData.bannerEmpty); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetSelectedCharacterID(i, 0); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetReadyPlayers(i, false); }
        _roomData.hostPlayer = 0;
        _roomData.gameStart = false;
        _roomData.isInData = true;

        return _roomData;
    }

    /// <summary>
    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
    /// </summary>
    /// <remarks>���C���X���b�h����̂���Unity�p�̃��\�b�h������</remarks>
    private void MainThreadMethod()
    {

    }
}