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
    SendDataCreator.PlayerNetData myNetData = new SendDataCreator.PlayerNetData();
    
    SendDataCreator.PlayerNetData receivedData = new SendDataCreator.PlayerNetData();
    
    SendDataCreator netInstance = new SendDataCreator();


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

    //byte[] testData = new byte[99999];

    //////////////////////
    //////// 関数 ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //netData[0].mainPacketData

        myNetData = default;
        myNetData.mainPacketData = default;
        myNetData.receivedByteData = new byte[0];
        myNetData.sendByteData = null;
        receivedData = default;

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
        //デバック用で任意のタイミングで送れるようにしておく
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //client.Send(myNetData.mainPacketData.comData.receiveAddress, 1);
            SendValue();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            myNetData.mainPacketData.inGameData.num += 1;
            myNetData.mainPacketData.inGameData.test = "HelloWorld!";

        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(receivedData.mainPacketData.inGameData.num);

            //Debug.Log("送信前バイト情報 : " + myNetData.sendByteData + "\n送信前バイト長 : " + myNetData.sendByteData.Length);
            Debug.Log("送信後バイト情報 : " + myNetData.receivedByteData + "\n送信後バイト長 : ");
            //Debug.Log(receiveNum);
        }
    }

    //送信は
    private void LateUpdate()
    {
        //SendValue();
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
        byte[] testData = new byte[0];

        values.ReadBlobElement(0, ref myNetData.receivedByteData);

        receivedData.mainPacketData = netInstance.ByteToStruct<SendDataCreator.PacketDataForPerFrame>(myNetData.receivedByteData);


        //receiveNum += values.ReadIntElement(0);
    }

    private void SendValue()
    {
        myNetData.sendByteData = netInstance.StructToByte(myNetData.mainPacketData);
        client.Send(myNetData.mainPacketData.comData.receiveAddress, myNetData.sendByteData, myNetData.sendByteData.Length);
    }

    private void MainThreadMethod()
    {
        Debug.Log($"OscCore received = " + receiveNum);
    }

    public void test()
    {
        //netda
        
    }
}                         