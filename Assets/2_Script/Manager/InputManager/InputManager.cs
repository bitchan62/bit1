using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    protected virtual void Update()
    { SetInput(); }
    
    //==================================================
    // ���� �Է�
    //==================================================

    // �Է¹޴� ����
    protected float moveHorizontal;
    protected float moveVertical;
    public Vector3 moveVec { get; protected set; }
    
    // moveKey �Է�
    public bool isMoveKeyDown
    {
        get
        { return moveVec != Vector3.zero; }
    }

    // �Է� ������
    [SerializeField] protected float inputDeadZone = 0.1f;

    // �̵� ���� �Է�
    protected void InputWASD()
    // �Է�(WASD, �����)���� ���� ����
    // ����ȭ��(��� �������� ũ�Ⱑ 1��) ���⺤�� ����
    {
        // ���� �Է¹���
        moveHorizontal = Input.GetAxisRaw("Horizontal"); // x�� (�¿�)
        moveVertical = Input.GetAxisRaw("Vertical");     // z�� (�յ�)

        // ������ �˻�
        if (Mathf.Abs(moveHorizontal) < inputDeadZone) { moveHorizontal = 0; }
        if (Mathf.Abs(moveVertical) < inputDeadZone) { moveVertical = 0; }

        // ���� ����
        // 45��(���ͺ�) Ʋ���� ����
        // <- ���� ī�޶� ������ �����ϵ��� �ٲ� ��
        moveVec = (Quaternion.Euler(0, 45, 0)  // �̵� ������ y�� ���� 45�� ȸ�� (ī�޶� ����)
            * (new Vector3(moveHorizontal, 0, moveVertical)).normalized); // �Էµ� ���⺤��
    }



    //==================================================
    // ���� �Է�
    //==================================================

    // ���� �Է� ����
    public bool isJumpKeyDown { get; protected set; }

    // ���� ���� �Է�
    // �����̽� ��
    protected void InputJump()
    { isJumpKeyDown = Input.GetButtonDown("Jump"); }



    //==================================================
    // ���� �Է�
    //==================================================

    // ���� �Է� ����
    public bool isAttackKeyDown { get; protected set; }

    // ���� �Է�
    // ��Ŭ��
    protected void InputAttack()
    { isAttackKeyDown = Input.GetMouseButtonDown(0); }


    //==================================================
    // ���� �Է�
    //==================================================

    // ���� �Է� ����
    // WASD || �����
    // Jump(Space Bar)
    // AttackAction(��Ŭ��)
    protected void SetInput()
    { InputWASD(); InputJump(); InputAttack(); }
}
