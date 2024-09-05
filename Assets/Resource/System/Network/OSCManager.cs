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

    [SerializeField]
    int port = 8000;

    [SerializeField]
    int otherPort = 8001;


    //現フレームの全プレイヤーデータ格納用
    //List<Player> nowFramePlayerData = new List<Player>();

    int receiveNum = 0;

    //////////////////////
    //////// 関数 ////////
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
