using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//==================================================
// ���� �⺻ ����
//==================================================
public class MeleeBasicAttack : AttackAction
{
    [SerializeField] protected float attackAngle = 90f;  // ���� ����

    protected override void Awake()
    {
        base.Awake();
        doAttack = DoAttack;
    }



    protected void DoAttack()
    {
        // OverlapSphere�� ����Ͽ� ���� ���� ���� ��� �ݶ��̴��� ã��
        // <- ����� ���̾ Ž��
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        // ������ ��� �ݶ��̴��� ���ؼ� ����
        foreach (Collider hitCollider in hitColliders)
        {
            // Ÿ�� �±׸� ���� ������Ʈ���� Ȯ��
            if (!hitCollider.CompareTag(targetTag)) { continue; }

            // Ÿ�� ���� ���� ���
            Vector3 directionToTarget = hitCollider.transform.position - transform.position;
            directionToTarget.y = 0;  // Y�� ���� 0���� ���� (���� ���� ����)
            
            // �ڽ��� ���� ���Ϳ� Ÿ�� ���� ���� ������ ���� ���
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
            
            // ���� ������ ���� ������ ���ݺ��� ������ Ȯ��
            if (angleToTarget <= attackAngle / 2)
            {
                // DamageReaction ������Ʈ�� �ִ��� Ȯ���ϰ�, ������ ó��
                DamageReaction targetActor = hitCollider.GetComponent<DamageReaction>();
                if (targetActor != null)
                {
                    targetActor.TakeDamage(attackDamage);
                    // ����� �ð�ȭ: ������ ������ Ÿ�ٱ��� ������ �� �׸���
                    Debug.DrawLine(transform.position, hitCollider.transform.position, Color.red, 1f);
                }
            }

        }
    } // MeleeBasicAttack


    // ���� ���� �ð�ȭ
    void OnDrawGizmos()
    {
        // ���� ������ �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ���� ������ �ð�ȭ
        Gizmos.color = Color.blue;
        Vector3 rightDir = Quaternion.Euler(0, attackAngle / 2, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, rightDir * attackRange);
        Gizmos.DrawRay(transform.position, leftDir * attackRange);
    }
}