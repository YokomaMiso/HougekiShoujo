using System.Linq;
using UnityEngine;

public class PerformanceMeasure : MonoBehaviour
{
    static PerformanceMeasure Instance;

    const float MeasureInterval = 1.0f;
    const int AveNum = 10;

    float nextTime;
    int frameCount, measureCount;

    float fps, aveFps;
    readonly float[] fpsHistory = new float[AveNum];

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Create()
    {
#if UNITY_EDITOR
        if (Instance) return;

        var go = new GameObject();
        Instance = go.AddComponent<PerformanceMeasure>();
        DontDestroyOnLoad(go);
#endif
    }

    void Update()
    {
        frameCount++;
        if (nextTime > Time.fixedTime) return;

        fps = frameCount / MeasureInterval;
        fpsHistory[measureCount] = fps;
        aveFps = fpsHistory.Average();

        nextTime = Time.fixedTime + MeasureInterval;
        frameCount = 0;

        measureCount++;
        if (measureCount >= AveNum) measureCount = 0;
    }

    [SerializeField] float x = 5;
    [SerializeField] float y = 5;
    [SerializeField] float w = 200;
    [SerializeField] float h = 20;
    [SerializeField] float i = 0;

    void OnGUI()
    {
        GUI.Label(new Rect(x, y, w, h), $"FPS:{fps}");
        GUI.Label(new Rect(x, y + h + i, w, h), $"Ave10sFPS:{aveFps}");
    }
}

