using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingWindow : MonoBehaviour
{
    float timer;
    const float lifeTime = 3.0f;

    Text connectText;
    const string textString = "Connecting";

    [SerializeField] AudioClip connnectSFX;

    void Start()
    {
        connectText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        SoundManager.PlaySFXForUI(connnectSFX, this.transform, true);
    }

    void Update()
    {
        timer += Time.deltaTime;

        string dot = "";
        int dotCount = Mathf.RoundToInt(timer);
        for (int i = 0; i < dotCount; i++) { dot += "."; }

        connectText.text = textString + dot;

        if (timer > lifeTime) { Destroy(gameObject); }
    }
}
