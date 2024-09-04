using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum GAME_STATE { TITLE = 0, OPTION, ROOM, IN_GAME, };

public class GameManager : MonoBehaviour
{
    public Managers managerMaster;
    public void SetManagerMaster(Managers _managerMaster) { managerMaster = _managerMaster; }


    [SerializeField] GAME_STATE startUpState;
    public GAME_STATE state = GAME_STATE.TITLE;
    public GAME_STATE prevState = GAME_STATE.TITLE;

    public GameObject[] canvases;
    [SerializeField] GameObject titleCanvas;
    //[SerializeField] GameObject loadCanvas;
    [SerializeField] GameObject optionCanvas;
    [SerializeField] GameObject roomCanvas;
    [SerializeField] GameObject ingameCanvas;


    void Start()
    {
        canvases = new GameObject[4];
        canvases[0] = titleCanvas;
        //canvases[1] = loadCanvas;
        canvases[1] = optionCanvas;
        canvases[2] = roomCanvas;
        canvases[3] = ingameCanvas;

        state = startUpState;
        prevState = startUpState;

        ChangeState(state);
    }

    public void ChangeState(GAME_STATE _state)
    {
        GameObject obj = Instantiate(canvases[(int)_state]);

        switch(_state)
        {
            case GAME_STATE.TITLE:
                obj.GetComponent<TitleCanvasBehavior>().SetGameManager(this);
                break;
//            case GAME_STATE.LOAD:
//                obj.GetComponent<LoadCanvasBehavior>().SetGameManager(this);
//                break;
            case GAME_STATE.OPTION:
                obj.GetComponent<OptionCanvasBehavior>().SetGameManager(this);
                break;
            case GAME_STATE.ROOM:
                break;
            case GAME_STATE.IN_GAME:
                break;
        }

        prevState = state;
        state = _state;
    }
}
