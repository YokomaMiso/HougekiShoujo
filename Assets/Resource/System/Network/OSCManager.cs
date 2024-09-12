using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;
using UnityEngine;
using OscCore;
using System.Runtime.InteropServices;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////

    //���g�̃C���Q�[���f�[�^
    public IngameData.PlayerNetData myNetIngameData = new IngameData.PlayerNetData();

    //�����Ă�������̃C���Q�[���f�[�^
    public IngameData.PlayerNetData receivedIngameData = new IngameData.PlayerNetData();

    //�n���h�V�F�C�N���Ƀu���[�h�L���X�g�ő��M����f�[�^
    public HandshakeData.SendUserData firstData = new HandshakeData.SendUserData();

    //�T�[�o���ɂȂ������̃n���h�V�F�C�N�f�[�^�̎󂯎M
    public HandshakeData.SendUserData receivedFirstData = new HandshakeData.SendUserData();

    //�ڑ��m�F���̃T�[�o����̕ԓ��f�[�^
    public ResponseServerData.ResData resServerData = new ResponseServerData.ResData();

    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();
    public MachingRoomData.RoomData receiveRoomData = new MachingRoomData.RoomData();

    public SendDataCreator netInstance = new SendDataCreator();

    //�������T�[�o���ǂ���
    bool isServer = false;
    bool isServerResponse = false;

    const float waitHandshakeResponseTime = 4f;

    ///////// OSCcore���� ////////

    OscClient tempClient;
    OscServer tempServer;

    OscClient mainClient;
    OscServer mainServer;

    //�g�p����|�[�g�ԍ��̎n��
    const int startPort = 8000;


    enum NetworkState
    {
        HandShaking,
        Maching,
        InGame
    }

    NetworkState nowNetState = NetworkState.HandShaking;

    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    // �Ƃ肠�����V���O���g���ŉ^�p�i�����ؖ������肪���܂��Ă�����C���j
    public static OSCManager OSCinstance;

    [SerializeField]
    bool isManual = true;

    [SerializeField]
    int port = 8000;

    //[SerializeField]
    //int otherPort = 8001;

    int testNum = 0;

    byte[] handshakeBytes = new byte[0];
    byte[] roomDataBytes = new byte[0];
    int receiveLong = 0;

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        /////////////////////////////
        //////////�����ʐM����///////
        /////////////////////////////

        roomData = initRoomData(roomData);
        receiveRoomData = initRoomData(receiveRoomData);

        handshakeBytes = netInstance.StructToByte(firstData);
        roomDataBytes = netInstance.StructToByte(roomData);

        //�T�[�o�������ꍇ�̃f�[�^�Z�b�g
        myNetIngameData.mainPacketData.comData.targetIP = "255.255.255.255";
        myNetIngameData.mainPacketData.comData.receiveAddress = "/example";

        //��U���g��IP���u���[�h�L���X�g�A�h���X��
        firstData.IP = "255.255.255.255";

        //�ꎞ�I�ɃT�[�o����̕ԓ����󂯕t����|�[�g�ԍ��擾
        firstData.tempPort = GetRandomTempPort();

        if (tempClient == null)
        {
            //8000�ԁi�T�[�o���̃|�[�g�ԍ��j�ֈ��Ă��N���C�A���g���쐬
            tempClient = new OscClient(myNetIngameData.mainPacketData.comData.targetIP, startPort);

        }

        if (tempServer == null)
        {
            //�ꎞ�|�[�g�ԍ��ō쐬
            tempServer = new OscServer(firstData.tempPort);
        }

        tempServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);

        Debug.Log("�n���h�V�F�C�N�J�n");

        //1�b���ƂɃn���h�V�F�C�N�̃f�[�^���M�����݂�
        InvokeRepeating("SendFirstHandshake", 0f, 1f);

        //��̃n���h�V�F�C�N���w�莞�Ԏ�������^�C���A�E�g������K�v�����邽�߂��̒��ŏ㏈�����~�߂�
        //�����ԓ����Ȃ���΂��̒��ŃT�[�o���쐬����
        StartCoroutine(CheckForResponse());

        /////////////////////////////
        ////////OSCcore������////////
        /////////////////////////////

        OSCinstance = this;

        //myNetIngameData = default;
        //myNetIngameData.mainPacketData = default;
        //myNetIngameData.byteData = null;
        //
        //receivedIngameData = default;
        //receivedIngameData.byteData = new byte[0];
        //
        //if(isManual)
        //{
        //    myNetIngameData.mainPacketData.comData.myPort = port;
        //}
        //else
        //{
        //    //myNetIngameData.mainPacketData.comData.myPort = PortAssignor();
        //}
        //
        //
        //Managers.instance.playerID = myNetIngameData.mainPacketData.comData.myPort - 8000;
        //
        //Debug.Log("���̃|�[�g�ԍ��� : " + myNetIngameData.mainPacketData.comData.myPort);
        //Debug.Log("���̃v���C���[�ԍ��� : " + Managers.instance.playerID);
        //
        ////myNetData.mainPacketData.comData.myIP = "255.255.255.255";
        ////myNetData.mainPacketData.comData.targetIP = "255.255.255.255";
        ////myNetData.mainPacketData.comData.targetPort = otherPort;
        //
        //myNetIngameData.mainPacketData.comData.sendAddress = "/example";
        //
        //if(myNetIngameData.mainPacketData.comData.myPort == startPort)
        //{
        //    myNetIngameData.mainPacketData.comData.targetPort = 8001;
        //}
        //else
        //{
        //    myNetIngameData.mainPacketData.comData.targetPort = 8000;
        //}
        //
        //
        //if (mainClient == null)
        //{
        //    mainClient = new OscClient(myNetIngameData.mainPacketData.comData.targetIP, myNetIngameData.mainPacketData.comData.targetPort);
        //
        //}
        //
        //if (mainServer == null)
        //{
        //    //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
        //    mainServer = new OscServer(myNetIngameData.mainPacketData.comData.myPort);
        //}
        //
        //mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReadIngameValue);


    }

    // Update is called once per frame
    void Update()
    {
        ////�f�o�b�N�p�ŔC�ӂ̃^�C�~���O�ő����悤�ɂ��Ă���
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    SendValue();
        //}

        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    myNetData.mainPacketData.inGameData.num += 1;
        //    myNetData.mainPacketData.inGameData.test = "HelloWorld!";
        //    myNetData.mainPacketData.inGameData.vc3 += new Vector3(1.0f, 0.5f, 0.1f);



        //}

        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    Debug.Log(receivedData.mainPacketData.inGameData.num);
        //    Debug.Log(receivedData.mainPacketData.inGameData.test);
        //    Debug.Log(receivedData.mainPacketData.inGameData.vc3);
        //}

        //Debug.Log(testNum);


        //Debug.Log("�n���h�V�F�C�N�p�f�[�^" + handshakeBytes.Length);
        //Debug.Log("�������" + roomDataBytes.Length);
        //Debug.Log("��M�o�C�g�T�C�Y" + receiveLong);

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("�z�X�g�v���C���[ID : " + roomData.hostPlayer);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SendMachingData();
        }
    }

    private void LateUpdate()
    {
        if (Managers.instance.state == GAME_STATE.IN_GAME)
        {
            SendIngameValue();
        }
    }

    private void OnEnable()
    {




    }

    private void OnDisable()
    {
        if (mainServer != null)
        {
            mainServer.Dispose();
        }

        if (tempServer != null)
        {
            tempServer.Dispose();
        }

    }

    MachingRoomData.RoomData initRoomData(MachingRoomData.RoomData _roomData)
    {
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetBannerNum(i, MachingRoomData.bannerEmpty); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetSelectedCharacterID(i, 0); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetReadyPlayers(i, false); }
        _roomData.hostPlayer = 0;
        _roomData.gameStart = false;


        return _roomData;
    }

    //////////////////////////
    //////�C���Q�[���֐�//////
    //////////////////////////

    /// <summary>
    /// �f�[�^���M
    /// </summary>
    private void SendIngameValue()
    {
        //���M�f�[�^�̃o�C�g�z��
        myNetIngameData.byteData = netInstance.StructToByte(myNetIngameData.mainPacketData);

        //�f�[�^�̑��M
        mainClient.Send(myNetIngameData.mainPacketData.comData.receiveAddress, myNetIngameData.byteData, myNetIngameData.byteData.Length);
    }

    /// <summary>
    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
    /// </summary>
    /// <param name="values">��M�����f�[�^</param>
    /// <remarks>�T�u�X���b�h����̂���Unity�p�̃��\�b�h�͓��삵�܂���I�I�I</remarks>
    private void ReadIngameValue(OscMessageValues values)
    {
        //��M�f�[�^�̃R�s�[
        values.ReadBlobElement(0, ref receivedIngameData.byteData);

        //�f�[�^�̍\���̉�
        receivedIngameData.mainPacketData = netInstance.ByteToStruct<IngameData.PacketDataForPerFrame>(receivedIngameData.byteData);

        return;
    }

    //////////////////////////////
    ///////////�����ʐM�֐�///////
    //////////////////////////////

    int GetRandomTempPort()
    {
        return Random.Range(8006, 9000);
    }

    //�n���h�V�F�C�N���M�֐�
    private void SendFirstHandshake()
    {
        byte[] bytes = netInstance.StructToByte(firstData);

        tempClient.Send(myNetIngameData.mainPacketData.comData.receiveAddress, bytes, bytes.Length);

        Debug.Log("1way���M");

        return;
    }

    /// <summary>
    /// �u���[�h�L���X�g���M���̃T�[�o����̕ԓ��p
    /// </summary>
    /// <param name="value"></param>
    private void ReceiveHandshakeForClient(OscMessageValues value)
    {
        byte[] _bytes = new byte[0];

        value.ReadBlobElement(0, ref _bytes);

        resServerData = netInstance.ByteToStruct<ResponseServerData.ResData>(_bytes);

        //���̃��\�b�h���Ăяo����Ă��鎞�_�ŃT�[�o����̕ԓ������Ă��邽��true�ɂ���
        isServerResponse = true;

        return;
    }

    /// <summary>
    /// �T�[�o���ɂȂ����ۂ̃N���C�A���g�n���h�V�F�C�N��M�p
    /// </summary>
    /// <param name="value"></param>
    private void ReceiveMachingServer(OscMessageValues value)
    {
        byte[] _receiveBytes = new byte[0];
        byte[] _sendBytes = new byte[0];

        resServerData.toClientPort = 8001;
        resServerData.serverIP = "255.255.255.255";


        value.ReadBlobElement(0, ref _receiveBytes);

        receiveLong = _receiveBytes.Length;

        //���M����Ă����o�C�g�z��T�C�Y���n���h�V�F�C�N�p�Ɠ����Ȃ�n���h�V�F�C�N�f�[�^�Ƃ��Ď�舵��
        if (_receiveBytes.Length == 1048)
        {
            testNum = 1;
            receivedFirstData = netInstance.ByteToStruct<HandshakeData.SendUserData>(_receiveBytes);

            if (mainClient == null)
            {
                mainClient = new OscClient(receivedFirstData.IP, receivedFirstData.tempPort);
            }

            _sendBytes = netInstance.StructToByte(resServerData);

            mainClient.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
        }
        else
        {
            testNum = 2;
            roomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_receiveBytes);

            //�K���n���h�V�F�C�N���ɏ�̏�����ʂ�̂ł����������삷��ꍇ�o�O
            if (mainClient == null)
            {
                mainClient = new OscClient(receivedFirstData.IP, receivedFirstData.tempPort);
            }

            _sendBytes = netInstance.StructToByte(roomData);

            mainClient.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
        }

        return;
    }

    private void ReceiveMachingClient(OscMessageValues value)
    {
        byte[] _recieveByte = new byte[0];

        value.ReadBlobElement(0, ref _recieveByte);

        receiveRoomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_recieveByte);

        return;
    }

    /// <summary>
    /// �n���h�V�F�C�N���M���̔����m�F�p
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckForResponse()
    {
        yield return new WaitForSeconds(waitHandshakeResponseTime);

        Debug.Log("�R���[�`���쓮");

        if (!isServerResponse)
        {
            //�n���h�V�F�C�N�m�F�p�p�P�b�g�j���O�ɃT�[�o���Ȃ��Ȃ�ƃo�O�邽�߂����ɋL�q
            CancelInvoke("SendFirstHandshake");
            Debug.Log("�T�[�o����̕ԓ�������܂���A�T�[�o�����ֈڍs");
            StartServer();
        }
        else
        {
            CancelInvoke("SendFirstHandshake");
            Debug.Log("�T�[�o�����݂��܂����A�N���C�A���g�����ֈڍs");
            StartClient();
        }
    }

    //�T�[�o�����݂������ߎ�M�f�[�^���܂Ƃ߂�
    private void StartClient()
    {
        tempServer.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);
        tempServer.Dispose();

        if (mainServer == null)
        {
            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
            mainServer = new OscServer(resServerData.toClientPort);
        }

        mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingClient);

        return;
    }

    //�T�[�o�s�ݎ��Ɏ������T�[�o�ɂȂ鏈��
    void StartServer()
    {
        isServer = true;
        isServerResponse = true;

        tempServer.RemoveMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveHandshakeForClient);
        tempServer.Dispose();

        if (mainServer == null)
        {
            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
            mainServer = new OscServer(startPort);
        }

        mainServer.TryAddMethod(myNetIngameData.mainPacketData.comData.receiveAddress, ReceiveMachingServer);

        return;
    }

    void SendMachingData()
    {
        byte[] _sendBytes = new byte[0];

        _sendBytes = netInstance.StructToByte(roomData);

        if (isServer)
        {
            mainClient.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
        }
        else
        {
            tempClient.Send(myNetIngameData.mainPacketData.comData.receiveAddress, _sendBytes, _sendBytes.Length);
        }


        return;
    }

    public void SendRoomData()
    {
        SendMachingData();

        return;
    }
}