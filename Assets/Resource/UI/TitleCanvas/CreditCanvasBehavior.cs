using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditCanvasBehavior : MonoBehaviour
{
    TitleCanvasBehavior parent;
    public void SetParent(TitleCanvasBehavior _parent) { parent = _parent; }

    void Update()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")) { parent.ChangeTitleState(TITLE_STATE.SELECT); }
    }
}
