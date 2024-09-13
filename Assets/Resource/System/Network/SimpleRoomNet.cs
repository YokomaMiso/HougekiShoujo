using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscCore;


//public class OSCManager : MonoBehaviour
//{
//    //////////////////////////////
//    //////// �{�Ԏg�p�ϐ� ////////
//    //////////////////////////////

//    //���g�̃l�b�g���[�N�f�[�^
//    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();

//    //�����Ă�������̃f�[�^
//    public MachingRoomData.RoomData receiveRoomData = new MachingRoomData.RoomData();

//    SendDataCreator netInstance = new SendDataCreator();

//    string broadcastAddress = "255.255.255.255";

//    string address = "/example";

//    ///////// OSCcore���� ////////

//    OscClient client;
//    OscServer server;


//    ////////////////////////////////
//    //////// �f�o�b�N�p�ϐ� ////////
//    ////////////////////////////////

//    // �Ƃ肠�����V���O���g���ŉ^�p�i�����ؖ������肪���܂��Ă�����C���j
//    public static OSCManager OSCinstance;

//    [SerializeField]
//    int myPort = 8000;

//    [SerializeField]
//    int otherPort = 8001;

//    //////////////////////
//    //////// �֐� ////////
//    //////////////////////

//    // Start is called before the first frame update
//    void Start()
//    {
//        roomData = default;
//        receiveRoomData = default;

//        if (client == null)
//        {
//            client = new OscClient(broadcastAddress, otherPort);

//        }

//        if (server == null)
//        {
//            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
//            server = new OscServer(myPort);
//        }

//        server.TryAddMethodPair(broadcastAddress, ReadValue, MainThreadMethod);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //�f�o�b�N�p�ŔC�ӂ̃^�C�~���O�ő����悤�ɂ��Ă���
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            SendValue();
//        }
        
//    }

//    private void LateUpdate()
//    {
//        SendValue();
//    }

//    private void OnDisable()
//    {
//        if (server != null)
//        {
//            server.Dispose();
//        }

//    }

//    /// <summary>
//    /// �f�[�^���M
//    /// </summary>
//    private void SendValue()
//    {
//        byte[] _sendBytes = new byte[0];

//        //���M�f�[�^�̃o�C�g�z��
//        _sendBytes = netInstance.StructToByte(roomData);

//        //�f�[�^�̑��M
//        client.Send(address, _sendBytes, _sendBytes.Length);
//    }

//    /// <summary>
//    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
//    /// </summary>
//    /// <param name="values">��M�����f�[�^</param>
//    /// <remarks>�T�u�X���b�h����̂���Unity�p�̃��\�b�h�͓��삵�܂���I�I�I</remarks>
//    private void ReadValue(OscMessageValues values)
//    {
//        byte[] _receiveBytes = new byte[0];

//        //��M�f�[�^�̃R�s�[
//        values.ReadBlobElement(0, ref _receiveBytes);

//        //�f�[�^�̍\���̉�
//        receiveRoomData = netInstance.ByteToStruct<MachingRoomData.RoomData>(_receiveBytes);
//    }

//    /// <summary>
//    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
//    /// </summary>
//    /// <remarks>���C���X���b�h����̂���Unity�p�̃��\�b�h������</remarks>
//    private void MainThreadMethod()
//    {

//    }
//}