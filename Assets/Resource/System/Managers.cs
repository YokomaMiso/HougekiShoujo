using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_STATE { LOGO_SPLASH = 0, TITLE, SELECT_ROOM, CONNECTION, ROOM, IN_GAME, RESULT, MAX_NUM };

public class Managers : MonoBehaviour
{
    /*�V���O���g��*/
    public static Managers instance;

    /*�Z�[�u�f�[�^*/
    public OptionData optionData;
    [SerializeField] GameObject optionCanvasPrefab;
    GameObject optionCanvasInstance = null;
    [SerializeField] GameObject changeSceneCanvasPrefab;
    GameObject changeSceneCanvasInstance = null;

    readonly string[] teamColor = new string[2]{ "#37A0FE", "#F76F6F" };

    public bool UsingCanvas()
    {
        bool usingCanvas = false;
        if (optionCanvasInstance != null) { usingCanvas = true; }
        if (changeSceneCanvasInstance != null) { usingCanvas = true; }

        return usingCanvas;
    }

    /*�Q�[���S�̂̃X�e�[�g*/
    public GAME_STATE state = GAME_STATE.LOGO_SPLASH;
    public GAME_STATE prevState = GAME_STATE.LOGO_SPLASH;
    public GAME_STATE nextState = GAME_STATE.LOGO_SPLASH;

    /*�Q�[�����Ŏg�p����ID*/
    public int playerID;
    int characterID = 0;

    /*�e��}�l�[�W��*/
    public GameManager gameManager;
    public SaveManager saveManager;
    public TimeManager timeManager;
    public RoomManager roomManager;

    private void Awake()
    {
        if (instance == null)
        {
            //�V���O���g����
            instance = this;
            DontDestroyOnLoad(gameObject);
            //�e�}�l�[�W���̎w��
            ManagerLoad();
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
            changeSceneCanvasInstance = Instantiate(changeSceneCanvasPrefab);
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
}
