using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscCore;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// 本番使用変数 ////////
    //////////////////////////////

    //自身のネットワークデータ
    SendDataCreator.PlayerNetData myNetData = default;

    SendDataCreator netInstance = new SendDataCreator();

    //1フレーム前の全プレイヤーデータ格納用
    List<SendDataCreator.PlayerNetData> beforeNetData = new List<SendDataCreator.PlayerNetData>();


    ///////// OSCcore周り ////////

    OscClient client;
    OscServer server;


    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    //現フレームの全プレイヤーデータ格納用
    //List<Player> nowFramePlayerData = new List<Player>();

    //////////////////////
    //////// 関数 ////////
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
