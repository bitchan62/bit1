using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


[RequireComponent(typeof(Animator))]
public class ActorAnimation : MonoBehaviour
{
    // �ִϸ�����
    protected Animator animator;

    // �ʱ�ȭ
    protected virtual void Awake()
    { animator = GetComponent<Animator>(); }


    // � Animation�� ������� üũ


    public bool isMove
    {
        set
        { animator.SetBool("IsMove", value); }
    }

    public bool isJump
    {
        set
        { animator.SetBool("IsJump", value); }
    }

    public bool isAttack
    {
        set
        { if (value) { animator.SetTrigger("IsAttack"); } }
    }

    public bool isDie { protected get; set; } = false;
    //  animatior.SetBool("IsDie", isDie);  // <- ���� �������� ���� �� ���Ƽ� ��� ����


    // ���̾� 0������
    // ���� ����ǰ� �ִ� �ִϸ��̼� �̸� Ȯ��
    public virtual bool CheckAnimationName(string animationStateName)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName(animationStateName) && state.normalizedTime < 1.0f;
    }

}