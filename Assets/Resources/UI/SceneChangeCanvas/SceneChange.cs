using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class SceneChange : MonoBehaviour
{
    protected AudioClip sfx;

    protected Vector3[] basePos = new Vector3[2] { new Vector3(-230, 420), new Vector3(230, -420) };
    protected Vector3[] startPos = new Vector3[2] { new Vector3(1880, 1590), new Vector3(-1880, -1590) };
    protected Vector3[] endPos = new Vector3[2] { new Vector3(-2485, -830), new Vector3(2485, 830) };

    protected GameObject[] ribbons = new GameObject[2];

    protected float timer;
    protected float lifeTime = 0.5f;

    int visibleNum = 0;
    Color[] ribbonColor = new Color[2] { Color.white, Color.clear };

    GAME_STATE nextScene;
    public void SetNextScene(GAME_STATE _state) { nextScene = _state; }

    protected virtual void Start()
    {
        if (sfx) { SoundManager.PlaySFXForUI(sfx); }

        SetChild();
        SetPosition();
    }

    protected void SetChild()
    {
        for (int i = 0; i < 2; i++)
        {
            ribbons[i] = transform.GetChild(i).gameObject;
            ribbons[i].GetComponent<Image>().color = ribbonColor[visibleNum];
        }
    }
    protected void TimerUpdate() { timer += Time.deltaTime; }
    protected void DestroyCheck(bool _change)
    {
        if (timer > lifeTime)
        {
            if (_change) { SceneManager.LoadScene((int)nextScene); }
            else { Destroy(gameObject); }
        }
    }

    protected virtual void SetPosition() { }

    protected void MoveRibbons(Vector3[] _firstPos, Vector3[] _secondPos)
    {
        for (int i = 0; i < 2; i++)
        {
            float seedValue = Mathf.Clamp01(timer / lifeTime);
            float value = Mathf.Sqrt(seedValue);
            Vector3 currentPos = Vector3.Lerp(_firstPos[i], _secondPos[i], value);
            ribbons[i].transform.localPosition = currentPos;
        }
    }

    public void RibbonsVisible(bool _visible) { if (!_visible) { visibleNum = 1; } }
}
