using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReload : MonoBehaviour
{
    [SerializeField] float reloadTime = 2;
    float timer;
    int shellNum = -1;
    public int ReloadBehavior()
    {
        timer += TimeManager.GetDeltaTime();
        if (timer > reloadTime) 
        {
            timer = 0;
            return shellNum;
        }

        return -1;
    }

    public void Reload(int _num) { shellNum = _num;  }
    public bool Reloading() { return timer != 0; }
    public void ReloadCancel() { timer = 0; }

}
