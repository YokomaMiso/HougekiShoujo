using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBlastCanonBehavior : ProjectileBehavior
{
    [SerializeField] GameObject backBlast;

    protected override void Start()
    {
        base.Start();
        GameObject obj = Instantiate(backBlast, transform.position, transform.rotation);
        obj.GetComponent<BackBlastBehavior>().SetPlayer(ownerPlayer);
    }

    protected override void Update()
    {
        base.Update();
        transform.position += transform.forward * speed * Managers.instance.timeManager.GetDeltaTime();
    }
}
