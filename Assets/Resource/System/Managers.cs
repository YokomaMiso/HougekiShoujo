using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_STATE { LOGO_SPLASH = 0, TITLE, SELECT_ROOM, CONNECTION, ROOM, IN_GAME, RESULT, GALLERY, CREDIT, MAX_NUM };
public enum UNLOCK_ITEM { UI_DELETE, TSUBASA, /*STAGE,*/ MAX_NUM };

public enum LANGUAGE_NUM { JAPANESE, ENGLISH, SIMPLE_CHINESE, TRADITIONAL_CHINESE, MAX_NUM };

public class Managers : MonoBehaviour
{
    /*シングルトン*/
    public static Managers instance;

    /*セーブデータ*/
    public OptionData optionData;
    [SerializeField] GameObject optionCanvasPrefab;
    GameObject optionCanvasInstance = null;
    [SerializeField] GameObject changeSceneCanvasPrefab;
    [SerializeField] GameObject changeSceneCanvasForResultPrefab;
    GameObject changeSceneCanvasInstance = null;

    readonly string[] teamColor = new string[2] { "#37A0FE", "#F76F6F" };

    public bool UsingCanvas()
    {
        bool usingCanvas = false;
        if (optionCanvasInstance != null) { usingCanvas = true; }
        if (changeSceneCanvasInstance != null) { usingCanvas = true; }

        return usingCanvas;
    }

    /*ゲーム全体のステート*/
    public GAME_STATE state = GAME_STATE.LOGO_SPLASH;
    public GAME_STATE prevState = GAME_STATE.LOGO_SPLASH;
    public GAME_STATE nextState = GAME_STATE.LOGO_SPLASH;

    /*ゲーム内で使用するID*/
    public int playerID;
    int characterID = 0;

    /*各種マネージャ*/
    public GameManager gameManager;
    public SaveManager saveManager;
    public TimeManager timeManager;
    public RoomManager roomManager;

    /*解放要素*/
    public bool[] unlockFlag = new bool[(int)UNLOCK_ITEM.MAX_NUM];

    /*言語設定*/
    public LANGUAGE_NUM nowLanguage;

    [SerializeField] AudioClip submitSFX;
    [SerializeField] AudioClip cancelSFX;
    [SerializeField] AudioClip cursorSFX;
    public void PlaySFXForUI(int _num)
    {
        AudioClip ac;
        switch (_num)
        {
            default: ac = submitSFX; break;
            case 1: ac = cancelSFX; break;
            case 2: ac = cursorSFX; break;
        }
        SoundManager.PlaySFXForUI(ac);
    }

    //スマートフォンフラグ
    bool smartPhone;
    public bool GetSmartPhoneFlag() { return smartPhone; }

    private void Awake()
    {
        if (instance == null)
        {
            //シングルトン化
            instance = this;
            DontDestroyOnLoad(gameObject);
            //各マネージャの指定
            ManagerLoad();

            //フレームレートの固定
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;

            // 画面の向きを縦のみに設定
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            // 左向きを有効にする
            Screen.autorotateToLandscapeLeft = true;
            // 右向きを有効にする
            Screen.autorotateToLandscapeRight = true;

#if UNITY_ANDROID
            smartPhone = true;
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (state == nextState) { return; }
        if (changeSceneCanvasInstance == null) { SceneManager.LoadScene((int)nextState); }
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
        if (nextState == _state) { return; }

        if (changeSceneCanvasInstance == null)
        {
            if (_state == GAME_STATE.RESULT)
            {
                changeSceneCanvasInstance = Instantiate(changeSceneCanvasForResultPrefab);
            }
            else
            {
                changeSceneCanvasInstance = Instantiate(changeSceneCanvasPrefab);
            }
            SceneChange sceneChange = changeSceneCanvasInstance.GetComponent<SceneChange>();
            sceneChange.SetNextScene(_state);
            sceneChange.RibbonsVisible(nextState != GAME_STATE.CONNECTION);
        }
        nextState = _state;
    }

    public void ChangeState(GAME_STATE _state)
    {
        prevState = state;
        state = _state;
    }

    public void CreateOptionCanvas()
    {
        if (optionCanvasInstance != null) { return; }
        optionCanvasInstance = Instantiate(optionCanvasPrefab);
    }

    public OptionData GetOptionData() { return optionData; }
    public void SaveOptionData(OptionData _receiveData)
    {
        optionData = _receiveData;
        saveManager.SaveOptionData(optionData);
    }

    public Color ColorCordToRGB(int _num)
    {
        if (_num >= teamColor.Length) { return Color.black; }

        if (ColorUtility.TryParseHtmlString(teamColor[_num], out Color color)) return color;
        else return Color.black;
    }

    public Color ColorCordToRGB(string _cord)
    {
        if (ColorUtility.TryParseHtmlString(_cord, out Color color)) return color;
        else return Color.black;
    }
}
