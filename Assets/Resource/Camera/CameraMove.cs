using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float distance = 10;
    [SerializeField] float asistValue = 2;

    static readonly Vector3 ingameRotation = new Vector3(45, 0, 0);
    float initialTimer;

    float shakeValue;

    void Start()
    {
    }

    void Update()
    {
        if (GameManager.state != GAME_STATE.IN_GAME) { return; }
        if (initialTimer < 1) { InitialAngle(); }
        else
        {
            CameraShakeUpdate();
            Move(Player.instance.transform.position);
        }
    }

    void InitialAngle()
    {
        initialTimer += Time.deltaTime;
        if (initialTimer >= 1) { initialTimer = 1; }

        Vector3 angle = Vector3.Lerp(Vector3.zero, ingameRotation, initialTimer);
        transform.rotation = Quaternion.Euler(angle);


        Vector3 playerPos = Player.instance.transform.position;
        Vector3 pos = Vector3.Lerp(new Vector3(1.0f, 1.5f, -2.0f), playerPos + new Vector3(0, distance, -distance), initialTimer);
        transform.position = pos;
    }

    void CameraShakeUpdate()
    {
        if (shakeValue == 0) { return; }

        shakeValue -= TimeManager.deltaTime;
        if (shakeValue < 0) { shakeValue = 0; }
    }

    public void Move(Vector3 _pos)
    {
        Vector3 shakeVector3 = Random.insideUnitCircle * shakeValue;
        transform.position = _pos + new Vector3(0, distance, -distance) + shakeVector3;
    }
    public void SetCameraShake(float _shakeValue)
    {
        if (_shakeValue <= shakeValue) { return; }

        shakeValue = _shakeValue;
    }
}
