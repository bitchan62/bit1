using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;



//==================================================
// �ǰ����� ���� ���� ���� / ��� �� ó��
//==================================================
public class DamageReaction : MonoBehaviour
{
    [SerializeField] protected int maxHp = 10;  // �ִ� �����
    [SerializeField] protected int nowHp = 10;  // ���� �����


    // �ǰ�
    public virtual void TakeDamage(int damage)
    {
        if (damage <= nowHp)
        { nowHp -= damage; }
        else
        { nowHp = 0; }

        Debug.Log("�ǰ�: " + gameObject.name);

        // �ǰ� ����
        // DamageReaction();

        // ü���� 0 ���Ϸ� �������� ó��
        if (nowHp <= 0)
        { Die(); }
    }


    // ��� ó��
    protected virtual void Die()
    {

    }
}