using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE { TITLE = 0, LOAD, OPTION, CONSOLE, IN_GAME, };

public class GameManager : MonoBehaviour
{
    public static GAME_STATE state = GAME_STATE.TITLE;
    public static GAME_STATE prevState = GAME_STATE.TITLE;

    public static GameObject[] canvases;
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

    public static void ChangeState(GAME_STATE _state)
    {
        Instantiate(canvases[(int)_state]);
        prevState = state;
        state = _state;
    }
}
