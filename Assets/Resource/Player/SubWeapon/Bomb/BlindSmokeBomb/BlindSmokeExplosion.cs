using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlindSmokeExplosion : ExplosionBehavior
{
    bool hitCheck = false;
    [SerializeField] GameObject blindCanvas;

    public override void SetData(Explosion _data)
    {
        imageAnimator = transform.GetChild(0).GetComponent<Animator>();

        float scale = _data.GetScale();
        transform.localScale = new Vector3(scale, scale, scale);

        imageAnimator.runtimeAnimatorController = _data.GetAnim();
    }

    protected override void Start()
    {
        //lifeTime = imageAnimator.GetCurrentAnimatorStateInfo(0).length - 0.75f;
        lifeTime = 10;
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.tag != hitTag) { return; }

        Player nowPlayer = other.GetComponent<Player>();
        if (nowPlayer == null) { return; }

        if (nowPlayer.GetPlayerID() != Managers.instance.playerID) { return; }

        if (hitCheck) { return; }

        Instantiate(blindCanvas);
        hitCheck = true;
    }
}
