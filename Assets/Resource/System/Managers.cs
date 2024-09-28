using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_STATE { LOGO_SPLASH = 0, TITLE, CONNECTION, ROOM, IN_GAME, RESULT, MAX_NUM };

public class Managers : MonoBehaviour
{
    /*シングルトン*/
    public static Managers instance;

    /*セーブデータ*/
    public OptionData optionData;
    [SerializeField] GameObject optionCanvasPrefab;
    GameObject optionCanvasInstance = null;
    public bool UsingOption() { return optionCanvasInstance != null; }

    /*ゲーム全体のステート*/
    public GAME_STATE state = GAME_STATE.LOGO_SPLASH;
    public GAME_STATE prevState = GAME_STATE.LOGO_SPLASH;

    /*ゲーム内で使用するID*/
    public int playerID;
    int characterID = 0;

    /*各種マネージャ*/
    public GameManager gameManager;
    public SaveManager saveManager;
    public TimeManager timeManager;
    public RoomManager roomManager;

    private void Awake()
    {
        if (instance == null)
        {
            //シングルトン化
            instance = this;
            DontDestroyOnLoad(gameObject);
            //各マネージャの指定
            ManagerLoad();
            //オプションデータのロード
            optionData = saveManager.LoadOptionData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void ManagerLoad()
    {
        gameManager = GetComponent<GameManager>();
        saveManager = GetComponent<SaveManager>();
        timeManager = GetComponent<TimeManager>();
        roomManager = GetComponent<RoomManager>();
    }

    public void ChangeScene(GAME_STATE _state)
    {
        SceneManager.LoadScene((int)_state);
    }

    public void ChangeState(GAME_STATE _state)
    {
        prevState = state;
        state = _state;
    }

    public void CreateOptionCanvas()
    {
        optionCanvasInstance = Instantiate(optionCanvasPrefab);
    }

    public OptionData GetOptionData() { return optionData; }
    public void SaveOptionData(OptionData _receiveData)
    {
        optionData = _receiveData;
        saveManager.SaveOptionData(optionData);
    }
}
