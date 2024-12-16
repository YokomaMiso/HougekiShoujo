using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlastExplosionBehavior : ExplosionBehavior
{
    [SerializeField] AudioClip backBlastSFX;

    protected override void Start()
    {
        lifeTime = imageAnimator.GetCurrentAnimatorStateInfo(0).length * 0.5f;

        GameObject obj = SoundManager.PlaySFX(backBlastSFX);
        obj.transform.position = this.transform.position;

        Vector3 distance = transform.position - Camera.main.transform.position;
        float weight = distance.magnitude / 5;
        if (weight < 1) { weight = 1; }

        float shakeValue = 1.0f / weight * cameraShakeRate;
        Camera.main.GetComponent<CameraMove>().SetCameraShake(shakeValue);
    }
}
