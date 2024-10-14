using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultExitButton : MonoBehaviour
{
    ResultCanvasBehavior ownerCanvas;
    public void SetOwnerCanvas(ResultCanvasBehavior _owner) { ownerCanvas = _owner; }

    void Update()
    {
        if (Managers.instance.UsingCanvas()) { return; }

        if (Input.GetButtonDown("Submit"))
        {
            ownerCanvas.ScoreInit();
            ownerCanvas.ReturnRoom();
        }
    }
}
