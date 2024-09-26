using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLogCanvas : MonoBehaviour
{
    [SerializeField] GameObject killLogPrefab;
    GameObject killLogsParent;

    void Start()
    {
        killLogsParent = transform.GetChild(0).gameObject;
    }

    public void AddKillLog(Player _player)
    {
        for (int i = 0; i < killLogsParent.transform.childCount; i++)
        {
            killLogsParent.transform.GetChild(i).GetComponent<KillLogBehavior>().LogNumAdd();
        }

        GameObject killLogInstance = Instantiate(killLogPrefab, killLogsParent.transform);
        killLogInstance.GetComponent<KillLogBehavior>().SetText(_player);
    }
}
