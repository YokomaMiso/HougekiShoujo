using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;
using UnityEngine;
using OscCore;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////

    //���g�̃l�b�g���[�N�f�[�^
    public SendDataCreator.PlayerNetData myNetData = new SendDataCreator.PlayerNetData();
    
    //�����Ă�������̃f�[�^
    public SendDataCreator.PlayerNetData receivedData = new SendDataCreator.PlayerNetData();
    
    SendDataCreator netInstance = new SendDataCreator();




    ///////// OSCcore���� ////////

    OscClient client;
    OscServer server;

    //�g�p����|�[�g�ԍ��̎n��
    const int startPort = 8000;


    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    // �Ƃ肠�����V���O���g���ŉ^�p�i�����ؖ������肪���܂��Ă�����C���j
    public static OSCManager OSCinstance;

    //[SerializeField]
    //int port = 8000;

    //[SerializeField]
    //int otherPort = 8001;

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        OSCinstance = this;

        myNetData = default;
        myNetData.mainPacketData = default;
        myNetData.byteData = null;

        receivedData = default;
        receivedData.byteData = new byte[0];

        myNetData.mainPacketData.comData.myPort = PortAssignor();
        Managers.instance.playerID = myNetData.mainPacketData.comData.myPort - 8000;
        
        Debug.Log("���̃|�[�g�ԍ��� : " + myNetData.mainPacketData.comData.myPort);
        Debug.Log("���̃v���C���[�ԍ��� : " + Managers.instance.playerID);

        myNetData.mainPacketData.comData.myIP = "255.255.255.255";
        myNetData.mainPacketData.comData.targetIP = "255.255.255.255";
        //myNetData.mainPacketData.comData.targetPort = otherPort;
        myNetData.mainPacketData.comData.receiveAddress = "/example";
        myNetData.mainPacketData.comData.sendAddress = "/example";

        if(myNetData.mainPacketData.comData.myPort == startPort)
        {
            myNetData.mainPacketData.comData.targetPort = 8001;
        }
        else
        {
            myNetData.mainPacketData.comData.targetPort = 8000;
        }


        if (client == null)
        {
            client = new OscClient(myNetData.mainPacketData.comData.targetIP, myNetData.mainPacketData.comData.targetPort);
        
        }

        if (server == null)
        {
            //server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
            server = new OscServer(myNetData.mainPacketData.comData.myPort);
        }
        
        server.TryAddMethodPair(myNetData.mainPacketData.comData.receiveAddress, ReadValue, MainThreadMethod);
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
    }
    
    private void LateUpdate()
    {
        SendValue();
    }

    private void OnEnable()
    {

        
        

    }

    private void OnDisable()
    {
        if(server != null)
        {
            server.Dispose();
        }
        
    }

    /// <summary>
    /// �f�[�^���M
    /// </summary>
    private void SendValue()
    {
        //���M�f�[�^�̃o�C�g�z��
        myNetData.byteData = netInstance.StructToByte(myNetData.mainPacketData);

        //�f�[�^�̑��M
        client.Send(myNetData.mainPacketData.comData.receiveAddress, myNetData.byteData, myNetData.byteData.Length);
    }

    /// <summary>
    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
    /// </summary>
    /// <param name="values">��M�����f�[�^</param>
    /// <remarks>�T�u�X���b�h����̂���Unity�p�̃��\�b�h�͓��삵�܂���I�I�I</remarks>
    private void ReadValue(OscMessageValues values)
    {
        //��M�f�[�^�̃R�s�[
        values.ReadBlobElement(0, ref receivedData.byteData);

        //�f�[�^�̍\���̉�
        receivedData.mainPacketData = netInstance.ByteToStruct<SendDataCreator.PacketDataForPerFrame>(receivedData.byteData);
    }

    /// <summary>
    /// �T�[�o���Ńf�[�^���L���b�`����ΌĂяo����܂�
    /// </summary>
    /// <remarks>���C���X���b�h����̂���Unity�p�̃��\�b�h������</remarks>
    private void MainThreadMethod()
    {

    }

    /// <summary>
    /// 8000�ȍ~�̃|�[�g�ԍ����m�F���g�p����Ă��Ȃ��|�[�g�����g�Ɋ��蓖�Ă܂�
    /// </summary>
    private int PortAssignor()
    {
        IPGlobalProperties ipPropertie = IPGlobalProperties.GetIPGlobalProperties();

        IEnumerable<System.Net.IPEndPoint> connection = ipPropertie.GetActiveTcpConnections().Select(x => x.LocalEndPoint);
        System.Net.IPEndPoint[] tcpListener = ipPropertie.GetActiveTcpListeners();
        System.Net.IPEndPoint[] udpListener = ipPropertie.GetActiveUdpListeners();

        HashSet<int> activePorts = new HashSet<int>(connection
        .Concat(udpListener)
        .Where(x => x.Port >= startPort)
        .Select(x => x.Port));

        for (int port = startPort; port <= 65535; port++)
        {
            if (!activePorts.Contains(port))
            {
                return port;
            }
        }

        return -1;
    }
}                         