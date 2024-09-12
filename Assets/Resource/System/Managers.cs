using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GAME_STATE { TITLE = 0, ROOM, IN_GAME, OPTION, };

public class Managers : MonoBehaviour
{
   public bool onDebug;

    /*�V���O���g��*/
    public static Managers instance;

    /*�Z�[�u�f�[�^*/
    public OptionData optionData;

    /*�Q�[���S�̂̃X�e�[�g*/
    public GAME_STATE state = GAME_STATE.TITLE;
    public GAME_STATE prevState = GAME_STATE.TITLE;

    /*�Q�[�����Ŏg�p����ID*/
    public int playerID;
    int characterID = 0;

    /*�e��}�l�[�W��*/
    public GameManager gameManager;
    public SaveManager saveManager;
    public TimeManager timeManager;
    public CanvasManager canvasManager;
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
    void ManagerLoad()
    {
        gameManager = GetComponent<GameManager>();
        saveManager = GetComponent<SaveManager>();
        timeManager = GetComponent<TimeManager>();
        canvasManager = GetComponent<CanvasManager>();
        roomManager = GetComponent<RoomManager>();
    }

    public void ChangeScene(GAME_STATE _state)
    {
        if (_state >= GAME_STATE.OPTION) { return; }

        SceneManager.LoadScene((int)_state);
    }

    public void ChangeState(GAME_STATE _state)
    {
        prevState = state;
        state = _state;
    }

    public OptionData GetOptionData() { return optionData; }
    public void SaveOptionData(OptionData _receiveData)
    {
        optionData = _receiveData;
        saveManager.SaveOptionData(optionData);
    }
}
