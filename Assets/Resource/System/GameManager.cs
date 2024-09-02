using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum GAME_STATE { TITLE = 0, LOAD, OPTION, CONSOLE, IN_GAME, };

public class GameManager : MonoBehaviour
{
    public Managers managerMaster;
    public void SetManagerMaster(Managers _managerMaster) { managerMaster = _managerMaster; }


    public GAME_STATE state = GAME_STATE.TITLE;
    public GAME_STATE prevState = GAME_STATE.TITLE;

    public GameObject[] canvases;
    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject loadCanvas;
    [SerializeField] GameObject optionCanvas;
    [SerializeField] GameObject consoleCanvas;
    [SerializeField] GameObject ingameCanvas;


    void Start()
    {
        canvases = new GameObject[5];
        canvases[0] = titleCanvas;
        canvases[1] = loadCanvas;
        canvases[2] = optionCanvas;
        canvases[3] = consoleCanvas;
        canvases[4] = ingameCanvas;
        ChangeState(GAME_STATE.TITLE);
    }

    public void ChangeState(GAME_STATE _state)
    {
        GameObject obj = Instantiate(canvases[(int)_state]);

        switch(_state)
        {
            case GAME_STATE.TITLE:
                obj.GetComponent<TitleCanvasBehavior>().SetGameManager(this);
                break;
            case GAME_STATE.LOAD:
                obj.GetComponent<LoadCanvasBehavior>().SetGameManager(this);
                break;
            case GAME_STATE.OPTION:
                obj.GetComponent<OptionCanvasBehavior>().SetGameManager(this);
                break;
            case GAME_STATE.CONSOLE:
                break;
            case GAME_STATE.IN_GAME:
                break;
        }

        prevState = state;
        state = _state;
    }
}
