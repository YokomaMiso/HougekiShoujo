using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLogCanvas : MonoBehaviour
{
    [SerializeField] GameObject killLogPrefab;

    public void AddKillLog(Player _player)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<KillLogBehavior>().LogNumAdd();
        }

        GameObject killLogInstance = Instantiate(killLogPrefab, transform);
        killLogInstance.GetComponent<KillLogBehavior>().SetText(_player);
    }
}
