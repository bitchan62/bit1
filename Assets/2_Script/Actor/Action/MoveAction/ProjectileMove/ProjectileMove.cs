using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MoveAction
{
    // ����ü ���� �ð�
    [SerializeField] protected float projectileTimer = 10f;

    // �ʱ�ȭ
    protected override void Awake()
    {
        base.Awake();
        // ���� ���� ����
        rigid.useGravity = false;
        rigid.freezeRotation = true;
    }

    // ��ǥ ��ġ�� �Է¹޴� �޼���
    public void SetTarget(Vector3 targetPos)
    {
        // ���� ���� ��� (����ȭ)
        moveVec = (targetPos - transform.position).normalized;
        isMove = true;

        // Ÿ�̸� �� �ش� ����ü ����
        StartCoroutine(Timer.StartTimer(projectileTimer, () => Destroy(this.gameObject)));

        Turn(); // <- �� 1ȸ, �ش� ���� �ٶ�
    }
}
