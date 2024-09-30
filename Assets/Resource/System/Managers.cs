using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_STATE { LOGO_SPLASH = 0, TITLE, ROOM, IN_GAME, RESULT, MAX_NUM };

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
            //�I�v�V�����f�[�^�̃��[�h
            optionData = saveManager.LoadOptionData();
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
            changeSceneCanvasInstance.GetComponent<SceneChange>().SetNextScene(_state);
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
}
