using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// <- ���Ͷ�� �������� ���۵�
public class FireAction : AttackAction
{
    protected override void Awake()
    {
        base.Awake();
        doAttack = DoAttack;
    }

    private void Start()
    {
        target = TargetManager.instance.Targeting();
    }

    // �߻�ü
    [SerializeField] protected GameObject projectile;

    // �ӽ� Ÿ��
    public Transform target; // <- �ݵ�� player���� �����ϰ� ��

    // �߻� ��ġ
    public Transform firePos;

    protected void DoAttack()
    {
        if (projectile != null)
        {
            // ����ü �����ϱ�
            GameObject instantProjectile = Instantiate(projectile, firePos.position, this.transform.rotation); // <- �߻� position ����

            // ����ü �̵� ��� ������
            // <- ���� MoveAction�� GetComponent�� ����, as Ű����� �ٲ㳢��� �� ���� �� ���⵵ ��
            ProjectileMove moveAction = instantProjectile.GetComponent<ProjectileMove>();

            // �߻� ���� ����
            if (moveAction != null)
            { moveAction.SetTarget(target.position); }
            else { Debug.Log("FireAction : �߸��� Projectile ��ϵ� : " + gameObject.name); }
        }
        else { Debug.Log("Projectile �������� ���� : " + gameObject.name); }
    }
}
