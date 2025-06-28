using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



//==================================================
// �̵� �ൿ
// moveVec�� �ܺο��� �Է��ϸ�, moveSpeed�� �ӵ��� �̵�
//==================================================
[RequireComponent(typeof(Rigidbody))]
public class MoveAction : MonoBehaviour
{
    // ������Ʈ�� ���� ����ȿ��
    protected Rigidbody rigid;

    // �ʱ�ȭ
    protected virtual void Awake()
    {
        // Rigidbody �ʱ�ȭ
        rigid = GetComponent<Rigidbody>();
    }
    

    //==================================================
    // �̵� �޼���
    //==================================================

    // �̵��� ����
    public Vector3 moveVec { get; set; }

    // �̵� �ӵ�
    [SerializeField] protected float moveSpeed = 5;

    // �̵� ���� ����
    public bool isMove { get; set; } = false;


    // �̵� �޼���
    // ������ġ += ���� * �̵� ���� * �̵� ���� ����
    public virtual void Move()
    { rigid.MovePosition(rigid.position + moveVec * moveSpeed * Time.deltaTime); }

    // ȸ��
    protected virtual void Turn()
    { transform.LookAt(transform.position + moveVec); }
}