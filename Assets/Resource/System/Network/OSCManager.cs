using OscCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class OSCManager : MonoBehaviour
{
    //////////////////////////////
    //////// 本番使用変数 ////////
    //////////////////////////////

    //送信データ構造体
    public AllGameData.AllData allData = new AllGameData.AllData();

    //自身のローカルインゲームデータ
    public IngameData.PlayerNetData myNetIngameData = new IngameData.PlayerNetData();

    //自身のローカルマッチングシーンデータ
    public MachingRoomData.RoomData roomData = new MachingRoomData.RoomData();

    //構造体変換処理があるため生成
    SendDataCreator netInstance = new SendDataCreator();

    //最大プレイヤー人数
    const int maxPlayer = 6;

    const string broadcastAddress = "255.255.255.255";

    int tempPort;

    const int startPort = 8000;

    [SerializeField]
    float sendPerSecond = 60.0f;

    string address = "/main";

    ///////// OSCcore周り ////////

    //送信先保存リスト
    //クライアントならホスト宛ての1つ、ホストならクライアント5人分が入る
    //ハンドシェイク時はホストからの応答を確認するため必ず一つだけ入る
    List<OscClient> clientList = new List<OscClient>();


    OscServer tempServer;
    OscServer mainServer;

    //自分がサーバかどうか
    bool isServer = false;
    bool isServerResponse = false;

    bool isFinishHandshake = false;

    const float waitHandshakeResponseTime = 2f;


    ////////////////////////////////
    //////// デバック用変数 ////////
    ////////////////////////////////

    // とりあえずシングルトンで運用（調停や証明書周りが決まってきたら修正）
    public static OSCManager OSCinstance;

    [SerializeField]
    int myPort = 8000;

    int testNum = 0;

    //ゲーム内で必要な送受信データリスト
    //　要素数＝プレイヤーID
    public List<AllGameData.AllData> playerDataList = new List<AllGameData.AllData>();

    string testS;

    //////////////////////
    //////// 関数 ////////
    //////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //自分のインスタンス
        OSCinstance = this;

        //インゲーム用データの初期化代入
        allData.pData = new IngameData.PlayerNetData();
        allData.pData = default;

        //ルームデータの初期化
        allData.rData = initRoomData(allData.rData);

        //ロ－カル用も同様に
        roomData = default;
        roomData = initRoomData(roomData);


        //自分のデータだった時だけポート番号を入れる
        for (int i = 0; i < maxPlayer; i++)
        {
            //全て初期値で最大人数分のデータをセットする
            allData.pData.PlayerID = i;
            allData.rData = initRoomData(allData.rData);
            allData.rData.isInData = false;
            playerDataList.Add(allData);
        }

        CreateTempNet();
    }

    // Update is called once per frame
    //インゲームデータ処理中に送信されるろまずいのでUpdateは基本不使用
    void Update()
    {
        //デバック用で任意のタイミングで送れるようにしておく
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        Debug.Log("PlayerID is " + Managers.instance.playerID);
        Debug.Log("IPAddress is " + GetLocalIPAddress());

        foreach (AllGameData.AllData data in playerDataList)
        {
            Debug.Log(data.rData.myID);
        }

        Debug.Log(testNum);

    }

    float sendDataTimer;
    float sendStartTimer;

    private void LateUpdate()
    {
        allData.rData = roomData;
        allData.pData = myNetIngameData;
        playerDataList[Managers.instance.playerID] = allData;

        float fixedDeltaTime = 1.0f / sendPerSecond;

        sendDataTimer += Time.deltaTime;

        if (sendStartTimer >= 2f)
        {
            if (sendDataTimer >= fixedDeltaTime)
            {
                Debug.Log("インゲームデータ送信");
                sendDataTimer += Time.deltaTime;

                //ハンドシェイクが完了していれば毎フレームインゲームデータを送信する
                if (isFinishHandshake)
                {
                    //送信用データリストにある分送信を試みる
                    for (int i = 0; i < playerDataList.Count; i++)
                    {
                        //ルームデータは初期化が行われていないと参照エラーが起きるため仮インスタンスを作成し代入
                        AllGameData.AllData _data = new AllGameData.AllData();
                        _data.rData = initRoomData(_data.rData);

                        SendValue(_data);
                        _data = playerDataList[i];

                        if (roomData.myID != -1)
                        {
                            SendValue(_data);
                        }
                    }
                }

                sendDataTimer = 0;
            }

        }
        else
        {
            sendStartTimer += Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        if (tempServer != null)
        {
            tempServer.Dispose();
        }

        if (mainServer != null)
        {
            mainServer.Dispose();
        }
    }

    ////////////////////////////////////////////////////
    ///////////////　ハンドシェイク用関数　/////////////
    ////////////////////////////////////////////////////

    private string GetLocalIPAddress()
    {
        IPAddress ipv4Address = null;

        // イーサネットのIPv4アドレスを探す
        ipv4Address = GetIPv4AddressByType(NetworkInterfaceType.Ethernet);
        if (ipv4Address != null)
        {
            return ipv4Address.ToString();
        }

        // Wi-FiのIPv4アドレスを探す
        ipv4Address = GetIPv4AddressByType(NetworkInterfaceType.Wireless80211);
        if (ipv4Address != null)
        {
            return ipv4Address.ToString();
        }

        // 両方存在しない場合はブロードキャストアドレスを返す
        return "255.255.255.255";
    }

    private IPAddress GetIPv4AddressByType(NetworkInterfaceType type)
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == type && ni.OperationalStatus == OperationalStatus.Up)
            {
                var ipProperties = ni.GetIPProperties();
                var ipv4Address = ipProperties.UnicastAddresses
                    .Where(ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(ua => ua.Address)
                    .FirstOrDefault();

                if (ipv4Address != null)
                {
                    return ipv4Address;
                }
            }
        }
        return null;
    }

    private void CreateTempNet()
    {
        //サーバがいるかどうか応答を確認するためのクライアントを作成する
        OscClient _client = new OscClient(broadcastAddress, startPort);

        clientList.Add(_client);

        //一時ポート番号でサーバからの応答を待機
        tempPort = GetRandomTempPort();

        tempServer = new OscServer(tempPort);

        tempServer.TryAddMethod(address, ReadValue);

        //1秒ごとにハンドシェイクのデータ送信を試みる
        InvokeRepeating("SendFirstHandshake", 0f, 1f);

        //上のハンドシェイクを指定時間試したらタイムアウトさせる必要があるためこの中で上処理を止める
        //もし返答がなければこの中でサーバを作成する
        StartCoroutine(CheckForResponse());

        return;
    }

    private void SendFirstHandshake()
    {
        AllGameData.AllData _data = new AllGameData.AllData();
        _data.rData = initRoomData(_data.rData);

        _data.pData.mainPacketData.comData.myIP = GetLocalIPAddress();
        _data.pData.mainPacketData.comData.myPort = tempPort;
        _data.rData.myID = -1;

        Debug.Log("ハンドシェイクの送信");


        if (roomData.isHandshaking == true)
        {
            Debug.Log("ハンドシェイクの送信");
            SendValue(_data);
        }

        return;
    }

    IEnumerator CheckForResponse()
    {
        yield return new WaitForSeconds(waitHandshakeResponseTime);

        Debug.Log("コルーチン作動");

        if (roomData.isHandshaking)
        {
            //ハンドシェイク確認用パケット破棄前にサーバがなくなるとバグるためここに記述
            CancelInvoke("SendFirstHandshake");
            Debug.Log("サーバからの返答がありません、サーバ処理へ移行");

            Managers.instance.playerID = 0;
            myNetIngameData.PlayerID = Managers.instance.playerID;
            roomData.myID = Managers.instance.playerID;

            allData.pData = myNetIngameData;
            allData.rData = roomData;

            playerDataList[0] = allData;

            clientList.Clear();

            tempServer.Dispose();

            mainServer = new OscServer(startPort);

            mainServer.TryAddMethod(address, ReadValue);

            isServer = true;

            isFinishHandshake = true;

            mainServer.TryAddMethod(address, ReadValue);
        }
        else
        {
            CancelInvoke("SendFirstHandshake");
            Debug.Log("サーバが存在しました、クライアント処理へ移行");

            tempServer.Dispose();

            mainServer = new OscServer(myNetIngameData.mainPacketData.comData.myPort);

            mainServer.TryAddMethod(address, ReadValue);


            isServer = false;

            isFinishHandshake = true;

            mainServer.TryAddMethod(address, ReadValue);
        }
    }

    private int GetRandomTempPort()
    {
        return UnityEngine.Random.Range(8006, 9000);
    }

    ////////////////////////////////////////////////////
    /////////////////　接続安定後用関数　///////////////
    ////////////////////////////////////////////////////

    /// <summary>
    /// データ送信関数
    /// </summary>
    /// <typeparam name="T">構造体及び値型のデータ</typeparam>
    /// <param name="_struct">実際に送信したい構造体データ</param>
    private void SendValue<T>(T _struct) where T : struct
    {
        byte[] _sendBytes = new byte[0];

        //送信データのバイト配列化
        _sendBytes = netInstance.StructToByte(_struct);


        //サーバなら五回、クライアントなら一回
        foreach (OscClient _client in clientList)
        {
            //データの送信
            _client.Send(address, _sendBytes, _sendBytes.Length);
        }
    }

    private void SendValueTarget<T>(T _struct, OscClient _client) where T : struct
    {
        byte[] _sendBytes = new byte[0];

        //送信データのバイト配列化
        _sendBytes = netInstance.StructToByte(_struct);


        //データの送信
        _client.Send(address, _sendBytes, _sendBytes.Length);
    }

    /// <summary>
    /// サーバ側でデータをキャッチすれば呼び出されます
    /// </summary>
    /// <param name="values">受信したデータ</param>
    /// <remarks>サブスレッド動作のためUnity用のメソッドは動作しません！！！</remarks>
    private void ReadValue(OscMessageValues values)
    {
        byte[] _receiveBytes = new byte[0];

        //受信データのコピー
        values.ReadBlobElement(0, ref _receiveBytes);

        //データの構造体化
        AllGameData.AllData _allData = new AllGameData.AllData();
        _allData.rData = initRoomData(_allData.rData);
        _allData = netInstance.ByteToStruct<AllGameData.AllData>(_receiveBytes);


        if (isServer)
        {
            //testS = "サーバ";

            //受信したプレイヤーデータがゲーム内に存在する場合データリストにセットする
            if (!_allData.rData.isHandshaking)
            {
                testS = "サーバとしてインゲーム受信";
                playerDataList[_allData.pData.PlayerID] = _allData;
            }
            else if (_allData.rData.myID == -1 && _allData.rData.isHandshaking)
            {
                //testS = "サーバとしてコネクション受信";


                testNum++;

                for (int i = 0; i < playerDataList.Count; i++)
                {
                    if (playerDataList[i].rData.myID == -1)
                    {


                        AllGameData.AllData _handshakeAllData = new AllGameData.AllData();
                        _handshakeAllData.rData = initRoomData(_handshakeAllData.rData);

                        _handshakeAllData.pData.mainPacketData.comData.myIP = GetLocalIPAddress();
                        _handshakeAllData.pData.mainPacketData.comData.myPort = startPort + i;
                        _handshakeAllData.pData.PlayerID = i;
                        _handshakeAllData.rData.myID = i;
                        _handshakeAllData.rData.isHandshaking = false;

                        playerDataList[i] = _handshakeAllData;
                        
                        OscClient _tempClient = new OscClient(broadcastAddress, _allData.pData.mainPacketData.comData.myPort);

                        SendValueTarget(_handshakeAllData, _tempClient);

                        OscClient _client = new OscClient(broadcastAddress, startPort + i);
                        clientList.Add(_client);

                        break;
                    }
                }
            }
        }
        else
        {
            if (!isFinishHandshake)
            {
                //testS = "クライアントとしてハンドシェイク受信";
                Managers.instance.playerID = _allData.rData.myID;
                roomData.myID = _allData.rData.myID;

                myNetIngameData = _allData.pData;
                roomData = _allData.rData;

                allData.pData = myNetIngameData;
                allData.rData = roomData;

                playerDataList[Managers.instance.playerID] = allData;

                clientList.Clear();

                OscClient _client = new OscClient(_allData.pData.mainPacketData.comData.myIP, startPort);

                clientList.Add(_client);

            }
            else
            {
                testS = "クライアントとしてインゲーム受信";

                playerDataList[_allData.pData.PlayerID] = _allData;
            }
        }
    }

    /// <summary>
    /// ルームデータの初期化処理
    /// </summary>
    /// <param name="_roomData">初期化したいルームデータ</param>
    /// <returns>ルームデータの初期化値</returns>
    MachingRoomData.RoomData initRoomData(MachingRoomData.RoomData _roomData)
    {
        _roomData.myBannerNum = -1;
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetSelectedCharacterID(i, 0); }
        for (int i = 0; i < MachingRoomData.bannerMaxCount; i++) { _roomData.SetReadyPlayers(i, false); }
        _roomData.hostPlayer = 0;
        _roomData.gameStart = false;
        _roomData.isInData = true;
        _roomData.isHandshaking = true;
        _roomData.myID = -1;

        return _roomData;
    }

    /// <summary>
    /// サーバ側でデータをキャッチすれば呼び出されます
    /// </summary>
    /// <remarks>メインスレッド動作のためUnity用のメソッドも動作</remarks>
    private void MainThreadMethod()
    {

    }

    /// <summary>
    /// ルームデータのゲッター
    /// </summary>
    /// <param name="_num">取得したいプレイヤーID</param>
    /// <returns>指定したプレイヤーIDのルームデータ</returns>
    public MachingRoomData.RoomData GetRoomData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata.rData;
    }

    /// <summary>
    /// インゲームデータのゲッター
    /// </summary>
    /// <param name="_num">取得したいプレイヤーID</param>
    /// <returns>指定したプレイヤーIDのインゲームデータ</returns>
    public IngameData.PlayerNetData GetIngameData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata.pData;
    }

    /// <summary>
    /// 全通信データのゲッター
    /// </summary>
    /// <param name="_num">取得したいプレイヤーID</param>
    /// <returns>指定したプレイヤーIDの全データ</returns>
    public AllGameData.AllData GetAllData(int _num)
    {
        AllGameData.AllData _alldata = playerDataList[_num];

        return _alldata;
    }

    /// <summary>
    /// 指定したプレイヤーIDの取得
    /// </summary>
    /// <param name="_num">取得したいプレイヤーID</param>
    /// <returns>プレイヤーID</returns>
    public int GetPlayerID(int _num)
    {
        return playerDataList[_num].pData.PlayerID;
    }
}