using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ActorAnimation))]
[RequireComponent(typeof(ChaseAction))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DamageReaction))]
abstract public class Monster : Actor
{
    // ���� ���� ���� �ൿ
    protected Action actionStatus;

    // Ÿ��
    Transform target;


    protected override void Awake()
    {
        base.Awake();
        actionStatus = SpawnState; // �������� ����
    }

    private void Start()
    { target = TargetManager.instance.Targeting(); }

    private void Update()
    { actionStatus(); }


    // ���� ��Ÿ� ���
    bool InAttackRange()
    { return (target.position - this.transform.position).sqrMagnitude <= attackAction.attackRange * attackAction.attackRange; }


    // ���� ����
    private void SpawnState()
    {
        if (!animatior.CheckAnimationName("Spawn")) // ���� �ִϸ��̼� ���� ��
        { actionStatus = IdleStatus; } // ���
    }

    // ��� ����
    private void IdleStatus()
    {
        if (InAttackRange())  // ���� ���� ���¶��
        { actionStatus = AttackStatus; } // ��������
        else
        { actionStatus = MoveStatus; }  // �ƴϸ� �̵�
    }


    // �̵� ����
    private void MoveStatus()
    {
        if (InAttackRange())
        {
            animatior.isMove = false;
            moveAction.isMove = false;
            actionStatus = AttackStatus;
        }
        else
        {
            animatior.isMove = true;
            moveAction.isMove = true;
            moveAction.Move();
        }
    }


    // ����, ���ε� �ִϸ��̼� ��� �߰� Ȯ��
    bool doAttack = false;
    bool doReload = false;

    // ���� ����
    private void AttackStatus()
    {
        // ���� �����ϴٸ�
        if (InAttackRange() && attackAction.isCanAttack)
        {
            attackAction.Attack();
            animatior.isAttack = true; // ���� �ִϸ��̼� ���
        }

        if (animatior.CheckAnimationName("Attack"))
        { doAttack = true; }

        if (doAttack && !animatior.CheckAnimationName("Attack"))
        {
            doAttack = false;
            actionStatus = ReloadStatus; // ���� �ĵ����̷� ���� }
        }
    }


    // ���� �ĵ����� �ִϸ��̼� ���
    private void ReloadStatus()
    {
        if (animatior.CheckAnimationName("Reload"))
        { doReload = true; }
        
        if (doReload && !animatior.CheckAnimationName("Reload"))
        {
            doReload = false;
            actionStatus = IdleStatus;
        }
    }
}