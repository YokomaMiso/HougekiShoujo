using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;
using UnityEngine;
using OscCore;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// 本番使用変数 ////////
    //////////////////////////////

    //自身のネットワークデータ
    public SendDataCreator.PlayerNetData myNetData = new SendDataCreator.PlayerNetData();
    
    //送られてきた相手のデータ
    public SendDataCreator.PlayerNetData receivedData = new SendDataCreator.PlayerNetData();
    
    SendDataCreator netInstance = new SendDataCreator();




    ///////// OSCcore周り ////////

    OscClient client;
    OscServer server;

    //使用するポート番号の始め
    const int startPort = 8000;


    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    // とりあえずシングルトンで運用（調停や証明書周りが決まってきたら修正）
    public static OSCManager OSCinstance;

    //[SerializeField]
    //int port = 8000;

    //[SerializeField]
    //int otherPort = 8001;

    //////////////////////
    //////// 関数 ////////
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
        
        Debug.Log("私のポート番号は : " + myNetData.mainPacketData.comData.myPort);
        Debug.Log("私のプレイヤー番号は : " + Managers.instance.playerID);

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
        ////デバック用で任意のタイミングで送れるようにしておく
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
    /// データ送信
    /// </summary>
    private void SendValue()
    {
        //送信データのバイト配列化
        myNetData.byteData = netInstance.StructToByte(myNetData.mainPacketData);

        //データの送信
        client.Send(myNetData.mainPacketData.comData.receiveAddress, myNetData.byteData, myNetData.byteData.Length);
    }

    /// <summary>
    /// サーバ側でデータをキャッチすれば呼び出されます
    /// </summary>
    /// <param name="values">受信したデータ</param>
    /// <remarks>サブスレッド動作のためUnity用のメソッドは動作しません！！！</remarks>
    private void ReadValue(OscMessageValues values)
    {
        //受信データのコピー
        values.ReadBlobElement(0, ref receivedData.byteData);

        //データの構造体化
        receivedData.mainPacketData = netInstance.ByteToStruct<SendDataCreator.PacketDataForPerFrame>(receivedData.byteData);
    }

    /// <summary>
    /// サーバ側でデータをキャッチすれば呼び出されます
    /// </summary>
    /// <remarks>メインスレッド動作のためUnity用のメソッドも動作</remarks>
    private void MainThreadMethod()
    {

    }

    /// <summary>
    /// 8000以降のポート番号を確認し使用されていないポートを自身に割り当てます
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