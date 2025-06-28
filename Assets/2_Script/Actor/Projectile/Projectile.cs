using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ProjectileHitAttack))]
[RequireComponent (typeof(ProjectileMove))]
public class Projectile : Actor
{

    protected override void Awake()
    {
        base.Awake();
    }


    // �� ������ �̵�
    protected virtual void Update()
    {
        if (moveAction.isMove) { moveAction.Move(); }
    }



}
