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

    //���t���[���̑S�v���C���[�f�[�^�i�[�p
    //List<Player> nowFramePlayerData = new List<Player>();

    //////////////////////
    //////// �֐� ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //netData[0].mainPacketData


        myNetData.mainPacketData.comData.myIP = "255.255.255.255";
        myNetData.mainPacketData.comData.targetIP = "127.0.0.1";
        myNetData.mainPacketData.comData.myPort = 8000;
        myNetData.mainPacketData.comData.targetPort = 8001;
        myNetData.mainPacketData.comData.receiveAddress = "/example/server";
        myNetData.mainPacketData.comData.sendAddress = "/example/client";

        //if (server == null)
        //{
        //    server = OscServer.GetOrCreate(myNetData.mainPacketData.comData.myPort);
        //}

        //server.TryAddMethodPair(myNetData.mainPacketData.comData.receiveAddress, ReadValue, MainThreadMethod);

        if (client == null)
        {
            client = new OscClient(myNetData.mainPacketData.comData.targetIP, myNetData.mainPacketData.comData.targetPort);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SendValue();
        }
    }

    private void OnEnable()
    {

        
        

    }

    private void OnDisable()
    {
        //server.RemoveMethod(myNetData.mainPacketData.comData.receiveAddress, ReadValue);
    }

    private void ReadValue(OscMessageValues values)
    {
        values.ReadBlobElement(0, ref myNetData.receiveData);
    }

    private void SendValue()
    {
        myNetData.sendData = netInstance.StructToByte(myNetData);
        client.Send(myNetData.mainPacketData.comData.receiveAddress, myNetData.sendData, myNetData.sendData.Length);
    }

    private void MainThreadMethod()
    {

    }
}
