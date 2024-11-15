using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunBulletBehavior : MonoBehaviour
{
    Vector3 forward;
    float speed;

    bool start;
    public void AttackStart() { start = true; }

    public void SetForward(Vector3 _forward, float _speed)
    {
        forward = _forward;
        speed = _speed;
    }
    void Update()
    {
        if (!start) { return; }
        transform.position += forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground") { Destroy(gameObject); }
    }
}
