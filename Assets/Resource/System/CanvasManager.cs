using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject[] canvases;
    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject roomCanvas;
    [SerializeField] GameObject ingameCanvas;
    [SerializeField] GameObject optionCanvas;

    //GameObject nowCanvas;

    void Start()
    {
        canvases = new GameObject[4];
        canvases[0] = titleCanvas;
        canvases[1] = roomCanvas;
        canvases[2] = ingameCanvas;
        canvases[3] = optionCanvas;

        Instantiate(canvases[0]);
    }
    public void ChangeCanvas(GAME_STATE _state)
    {
        //Destroy(nowCanvas);
        GameObject obj = Instantiate(canvases[(int)_state]);
        //nowCanvas = obj;
    }
}
