using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscCore;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// �{�Ԏg�p�ϐ� ////////
    //////////////////////////////

    //���g�̃l�b�g���[�N�f�[�^
    SendDataCreator.PlayerNetData myNetData = default;

    SendDataCreator netInstance = new SendDataCreator();

    //1�t���[���O�̑S�v���C���[�f�[�^�i�[�p
    List<SendDataCreator.PlayerNetData> beforeNetData = new List<SendDataCreator.PlayerNetData>();


    ///////// OSCcore���� ////////

    OscClient client;
    OscServer server;


    ////////////////////////////////
    //////// �f�o�b�N�p�ϐ� ////////
    ////////////////////////////////

    [SerializeField]
    int port = 8000;

    [SerializeField]
    int otherPort = 8001;


    //���t���[���̑S�v���C���[�f�[�^�i�[�p
    //List<Player> nowFramePlayerData = new List<Player>();

    int receiveNum = 0;

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //netData[0].mainPacketData


        myNetData.mainPacketData.comData.myIP = "255.255.255.255";
        myNetData.mainPacketData.comData.targetIP = "255.255.255.255";
        myNetData.mainPacketData.comData.myPort = port;
        myNetData.mainPacketData.comData.targetPort = otherPort;
        myNetData.mainPacketData.comData.receiveAddress = "/example";
        myNetData.mainPacketData.comData.sendAddress = "/example";


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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            client.Send(myNetData.mainPacketData.comData.receiveAddress, 1);
            //SendValue();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(receiveNum);
        }
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

    private void ReadValue(OscMessageValues values)
    {
        //values.ReadBlobElement(0, ref myNetData.receiveData);
        receiveNum += values.ReadIntElement(0);
    }

    private void SendValue()
    {
        myNetData.sendData = netInstance.StructToByte(myNetData);
        client.Send(myNetData.mainPacketData.comData.receiveAddress, myNetData.sendData, myNetData.sendData.Length);
    }

    private void MainThreadMethod()
    {
        Debug.Log($"OscCore received = " + receiveNum);
    }
}
